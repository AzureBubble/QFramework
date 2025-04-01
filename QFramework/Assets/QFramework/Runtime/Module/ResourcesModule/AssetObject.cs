using QFramework.Utility;
using YooAsset;

namespace QFramework
{
    public partial class ResourcesModule
    {
        public sealed class AssetObject : BasePoolObject
        {
            private AssetHandle m_assetHandle;
            private ResourcesModule m_resourcesModule;

            public static AssetObject Create(string objName, object target, object assetHandle, ResourcesModule resourcesModule)
            {
                if (assetHandle == null)
                {
                    throw new QFrameworkException("Resource is invalid.");
                }

                if (resourcesModule == null)
                {
                    throw new QFrameworkException("Resource Manager is invalid.");
                }

                AssetObject assetObject = MemoryPoolMgr.Get<AssetObject>();
                assetObject.Initialize(objName, target);
                assetObject.m_assetHandle = assetHandle as AssetHandle;
                assetObject.m_resourcesModule = resourcesModule;
                return assetObject;
            }

            public override void OnRelease()
            {
                base.OnRelease();
                m_assetHandle = null;
            }

            protected internal override void OnGet()
            {
                base.OnGet();
            }

            protected internal override void Release(bool isOnRelease)
            {
                if (!isOnRelease)
                {
                    AssetHandle handle = m_assetHandle;

                    if (handle != null && handle.IsValid)
                    {
                        handle.Dispose();
                    }
                    handle = null;
                }
            }
        }
    }
}