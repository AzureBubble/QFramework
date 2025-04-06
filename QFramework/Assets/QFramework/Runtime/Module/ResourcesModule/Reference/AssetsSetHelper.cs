using QFramework.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework
{
    public static class AssetsSetHelper
    {
        private static IResourcesModule m_resourcesModule;

        private static void GetResourcesModule()
        {
            if (m_resourcesModule == null)
            {
                m_resourcesModule = ModuleSystem.GetModule<IResourcesModule>();
            }
        }

        #region SetMaterial

        public static void SetMaterialAsync(this Image image, string location, string packageName = "")
        {
            image.SetMaterial(location, true, packageName);
        }

        public static void SetMaterialSync(this Image image, string location, string packageName = "")
        {
            image.SetMaterial(location, false, packageName);
        }

        public static void SetMaterial(this Image image, string location, bool isAsync = false, string packageName = "")
        {
            if (image == null)
            {
                throw new QFrameworkException($"SetSprite failed. Because image is null.");
            }

            GetResourcesModule();

            if (!isAsync)
            {
                Material material = m_resourcesModule.LoadAsset<Material>(location, packageName);
                image.material = material;
                AssetsReference.Ref(material, image.gameObject);
            }
            else
            {
                m_resourcesModule.LoadAssetAsync<Material>(location, material =>
                {
                    if (image == null || image.gameObject == null)
                    {
                        m_resourcesModule.UnloadAsset(material);
                        material = null;
                        return;
                    }
                    image.material = material;
                    AssetsReference.Ref(material, image.gameObject);
                }, packageName);
            }
        }

        public static void SetMaterialAsync(this SpriteRenderer spriteRenderer, string location, string packageName = "")
        {
            spriteRenderer.SetMaterial(location, true, packageName);
        }

        public static void SetMaterialSync(this SpriteRenderer spriteRenderer, string location, string packageName = "")
        {
            spriteRenderer.SetMaterial(location, false, packageName);
        }

        public static void SetMaterial(this SpriteRenderer spriteRenderer, string location, bool isAsync = false, string packageName = "")
        {
            if (spriteRenderer == null)
            {
                throw new QFrameworkException($"SetSprite failed. Because image is null.");
            }

            GetResourcesModule();

            if (!isAsync)
            {
                Material material = m_resourcesModule.LoadAsset<Material>(location, packageName);
                spriteRenderer.material = material;
                AssetsReference.Ref(material, spriteRenderer.gameObject);
            }
            else
            {
                m_resourcesModule.LoadAssetAsync<Material>(location, material =>
                {
                    if (spriteRenderer == null || spriteRenderer.gameObject == null)
                    {
                        m_resourcesModule.UnloadAsset(material);
                        material = null;
                        return;
                    }
                    spriteRenderer.material = material;
                    AssetsReference.Ref(material, spriteRenderer.gameObject);
                }, packageName);
            }
        }

        public static void SetMaterialAsync(this MeshRenderer meshRenderer, string location, bool needInstance = true, string packageName = "")
        {
            meshRenderer.SetMaterial(location, needInstance, true, packageName);
        }

        public static void SetMaterialSync(this MeshRenderer meshRenderer, string location, bool needInstance = true, string packageName = "")
        {
            meshRenderer.SetMaterial(location, needInstance, false, packageName);
        }

        public static void SetMaterial(this MeshRenderer meshRenderer, string location, bool needInstance = true, bool isAsync = false, string packageName = "")
        {
            if (meshRenderer == null)
            {
                throw new QFrameworkException($"SetSprite failed. Because image is null.");
            }

            GetResourcesModule();

            if (!isAsync)
            {
                Material material = m_resourcesModule.LoadAsset<Material>(location, packageName);
                meshRenderer.material = needInstance ? Object.Instantiate(material) : material;
                AssetsReference.Ref(material, meshRenderer.gameObject);
            }
            else
            {
                m_resourcesModule.LoadAssetAsync<Material>(location, material =>
                {
                    if (meshRenderer == null || meshRenderer.gameObject == null)
                    {
                        m_resourcesModule.UnloadAsset(material);
                        material = null;
                        return;
                    }
                    meshRenderer.material = needInstance ? Object.Instantiate(material) : material;
                    AssetsReference.Ref(material, meshRenderer.gameObject);
                }, packageName);
            }
        }

        public static void SetSharedMaterialAsync(this MeshRenderer meshRenderer, string location, string packageName = "")
        {
            meshRenderer.SetSharedMaterial(location, true, packageName);
        }

        public static void SetSharedMaterialSync(this MeshRenderer meshRenderer, string location, string packageName = "")
        {
            meshRenderer.SetSharedMaterial(location, false, packageName);
        }

        public static void SetSharedMaterial(this MeshRenderer meshRenderer, string location, bool isAsync = false, string packageName = "")
        {
            if (meshRenderer == null)
            {
                throw new QFrameworkException($"SetSprite failed. Because image is null.");
            }

            GetResourcesModule();

            if (!isAsync)
            {
                Material material = m_resourcesModule.LoadAsset<Material>(location, packageName);
                meshRenderer.sharedMaterial = material;
                AssetsReference.Ref(material, meshRenderer.gameObject);
            }
            else
            {
                m_resourcesModule.LoadAssetAsync<Material>(location, material =>
                {
                    if (meshRenderer == null || meshRenderer.gameObject == null)
                    {
                        m_resourcesModule.UnloadAsset(material);
                        material = null;
                        return;
                    }
                    meshRenderer.sharedMaterial = material;
                    AssetsReference.Ref(material, meshRenderer.gameObject);
                }, packageName);
            }
        }

        #endregion
    }
}