using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    [DisallowMultipleComponent]
    internal class ResourcesExtComponent : MonoBehaviour
    {
        public static ResourcesExtComponent Instance { get; private set; }

        private readonly TimeoutController m_timeoutController = new TimeoutController();

        /// <summary>
        /// 正在加载中的资源列表
        /// </summary>
        private readonly HashSet<string> m_assetLoadingSet = new HashSet<string>();

        /// <summary>
        /// 检查是否可以释放资源间隔时间
        /// </summary>
        [SerializeField]
        private float m_checkCanReleaseInterval = 30.0f;

        private float m_checkCanReleaseTime = 0.0f;

        /// <summary>
        /// 对象池自动释放时间间隔
        /// </summary>
        [SerializeField]
        private float m_autoReleaseInterval = 60.0f;

        /// <summary>
        /// 保存加载完成的图片对象
        /// </summary>
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private LinkedList<LoadAssetObject> m_loadAssetObjectsLinkedList;

        /// <summary>
        /// 加载的图片资源对象池
        /// </summary>
        private IObjectPool<AssetItemObject> m_assetItemObjectPool;

#if UNITY_EDITOR
        public LinkedList<LoadAssetObject> LoadAssetObjectsLinkedList
        {
            get => m_loadAssetObjectsLinkedList;
            set => m_loadAssetObjectsLinkedList = value;
        }
#endif

        private async void Start()
        {
            Instance = this;
            await UniTask.DelayFrame(1);
            IObjectPoolModule objPoolMgr = ModuleSystem.GetModule<IObjectPoolModule>();
            m_assetItemObjectPool = objPoolMgr?.CreateMultiGetObjectPool<AssetItemObject>("SetAssetPool",
                m_autoReleaseInterval, 16, 60, 0);
            m_loadAssetObjectsLinkedList = new LinkedList<LoadAssetObject>();
            InitResourcesModule();
        }

        private void Update()
        {
            m_checkCanReleaseTime += Time.unscaledDeltaTime;

            if (m_checkCanReleaseTime < (double)m_checkCanReleaseInterval)
            {
                return;
            }

            ReleaseUnused();
        }

        private void SetAsset(ISetAssetObject setAssetObject, Object assetObject)
        {
            m_loadAssetObjectsLinkedList.AddLast(new LoadAssetObject(setAssetObject, assetObject));
            setAssetObject.SetAsset(assetObject);
        }

        private async UniTask TryWaitingLoading(string assetObjectKey)
        {
            if (m_assetLoadingSet.Contains(assetObjectKey))
            {
                try
                {
                    await UniTask.WaitUntil(
                            () => !m_assetLoadingSet.Contains(assetObjectKey))
#if UNITY_EDITOR
                        .AttachExternalCancellation(m_timeoutController.Timeout(TimeSpan.FromSeconds(60)));
                    m_timeoutController.Reset();
#else
                    ;
#endif
                }
                catch (OperationCanceledException ex)
                {
                    if (m_timeoutController.IsTimeout())
                    {
                        Debugger.Error($"LoadAssetAsync Waiting {assetObjectKey} timeout. reason:{ex.Message}");
                    }
                }
            }
        }

        #region ResourcesModule

        /// <summary>
        /// 资源管理模块
        /// </summary>
        private IResourcesModule m_resourcesModule;

        private static LoadAssetCallbacks m_loadAssetCallbacks;

        public IResourcesModule ResourcesModule => m_resourcesModule;

        private void InitResourcesModule()
        {
            m_resourcesModule = ModuleSystem.GetModule<IResourcesModule>();
            m_loadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFailure);
        }

        private void OnLoadAssetFailure(string assetName, LoadResourceStatus status, string errormessage, object userdata)
        {
            m_assetLoadingSet.Remove(assetName);
            Debugger.Error("Can not load asset from '{1}' with error message '{2}'.", assetName, errormessage);
        }

        private void OnLoadAssetSuccess(string assetName, object asset, float duration, object userdata)
        {
            m_assetLoadingSet.Remove(assetName);
            ISetAssetObject setAssetObject = (ISetAssetObject)userdata;
            Object assetObject = asset as Object;

            if (assetObject != null)
            {
                m_assetItemObjectPool.Create(AssetItemObject.Create(setAssetObject.Location, assetObject), true);
                SetAsset(setAssetObject, assetObject);
            }
            else
            {
                Debugger.Error($"Load failure asset type is {asset.GetType()}.");
            }
        }

        /// <summary>
        /// 通过资源系统设置资源。
        /// </summary>
        /// <param name="setAssetObject">需要设置的对象。</param>
        public async UniTaskVoid SetAssetByResources<T>(ISetAssetObject setAssetObject) where T : UnityEngine.Object
        {
            await TryWaitingLoading(setAssetObject.Location);

            if (m_assetItemObjectPool.CanGet(setAssetObject.Location))
            {
                var assetObject = m_assetItemObjectPool.Get(setAssetObject.Location).TargetObj as T;
                SetAsset(setAssetObject, assetObject);
            }
            else
            {
                m_assetLoadingSet.Add(setAssetObject.Location);
                m_resourcesModule.LoadAssetAsync(setAssetObject.Location, typeof(T), 0, m_loadAssetCallbacks, setAssetObject);
            }
        }

        #endregion

        /// <summary>
        /// 回收无引用的缓存资产。
        /// </summary>
#if ODIN_INSPECTOR
        [Button("Release Unused")]
#endif
        private void ReleaseUnused()
        {
            if (m_loadAssetObjectsLinkedList == null)
            {
                return;
            }

            LinkedListNode<LoadAssetObject> current = m_loadAssetObjectsLinkedList.First;
            while (current != null)
            {
                var next = current.Next;

                if (current.Value.SetAssetObject != null && current.Value.SetAssetObject.CanRelease())
                {
                    m_assetItemObjectPool.Recycle(current.Value.SetAssetObject);
                    MemoryPoolMgr.Release(current.Value.SetAssetObject);
                    m_loadAssetObjectsLinkedList.Remove(current);
                }

                current = next;
            }

            m_checkCanReleaseTime = 0.0f;
        }
    }
}