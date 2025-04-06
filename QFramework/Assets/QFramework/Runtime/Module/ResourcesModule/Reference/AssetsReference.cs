using System;
using System.Collections.Generic;
using QFramework.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    [SerializeField]
    public struct AssetsRefInfo
    {
        public int InstanceId;
        public Object RefAsset;

        public AssetsRefInfo(Object refAsset)
        {
            RefAsset = refAsset;
            InstanceId = refAsset.GetInstanceID();
        }
    }

    public class AssetsReference : MonoBehaviour
    {
        [SerializeField] private GameObject m_sourceGameObject;
        [SerializeField] private List<AssetsRefInfo> m_assetsRefInfos;
        private IResourcesModule m_resourcesModule;

        private void OnDestroy()
        {
            if (m_resourcesModule == null)
            {
                m_resourcesModule = ModuleSystem.GetModule<IResourcesModule>();
            }

            if (m_resourcesModule == null)
            {
                throw new QFrameworkException($"resourceModule is null.");
            }

            if (m_sourceGameObject != null)
            {
                m_resourcesModule.UnloadAsset(m_sourceGameObject);
            }

            ReleaseRefAssetInfoList();
        }

        private void ReleaseRefAssetInfoList()
        {
            if (m_assetsRefInfos != null)
            {
                for (int i = 0; i < m_assetsRefInfos.Count; i++)
                {
                    m_resourcesModule.UnloadAsset(m_assetsRefInfos[i]);
                }
                m_assetsRefInfos.Clear();
            }
        }

        public AssetsReference Ref(GameObject obj, IResourcesModule resourcesModule = null)
        {
            if (obj == null)
            {
                throw new QFrameworkException($"Source gameObject is null.");
            }

            if (obj.scene.name != null)
            {
                throw new QFrameworkException($"Source gameObject is in scene.");
            }

            m_sourceGameObject = obj;
            m_resourcesModule = resourcesModule;
            return this;
        }

        public AssetsReference Ref<T>(T obj, IResourcesModule resourceModule = null) where T : UnityEngine.Object
        {
            if (obj == null)
            {
                throw new QFrameworkException($"Source gameObject is null.");
            }

            m_resourcesModule = resourceModule;
            if (m_assetsRefInfos == null)
            {
                m_assetsRefInfos = new List<AssetsRefInfo>();
            }

            m_assetsRefInfos.Add(new AssetsRefInfo(obj));
            return this;
        }

        public static AssetsReference Instantiate(GameObject obj, Transform parent = null, IResourcesModule resourceModule = null)
        {
            if (obj == null)
            {
                throw new QFrameworkException($"Source gameObject is null.");
            }

            if (obj.scene.name != null)
            {
                throw new QFrameworkException($"Source gameObject is in scene.");
            }

            GameObject instance = Object.Instantiate(obj, parent);
            return instance.AddComponent<AssetsReference>().Ref(obj, resourceModule);
        }

        public static AssetsReference Ref(GameObject obj, GameObject instance, IResourcesModule resourceModule = null)
        {
            if (obj == null)
            {
                throw new QFrameworkException($"Source gameObject is null.");
            }

            if (obj.scene.name != null)
            {
                throw new QFrameworkException($"Source gameObject is in scene.");
            }

            var comp = instance.GetComponent<AssetsReference>();
            return comp != null ? comp : instance.AddComponent<AssetsReference>().Ref(obj, resourceModule);
        }

        public static AssetsReference Ref<T>(T obj, GameObject instance, IResourcesModule resourceModule = null) where T : UnityEngine.Object
        {
            if (obj == null)
            {
                throw new QFrameworkException($"Source gameObject is null.");
            }

            var comp = instance.GetComponent<AssetsReference>();
            return comp != null ? comp : instance.AddComponent<AssetsReference>().Ref(obj, resourceModule);
        }
    }
}