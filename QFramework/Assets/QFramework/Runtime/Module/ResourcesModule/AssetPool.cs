using QFramework.Utility;

namespace QFramework
{
    public partial class ResourcesModule
    {
        private IObjectPool<AssetObject> m_assetObjPool;

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float AssetPoolAutoReleaseInterval
        {
            get => m_assetObjPool.AutoReleaseInterval;
            set => m_assetObjPool.AutoReleaseInterval = value;
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int AssetPoolCapacity
        {
            get => m_assetObjPool.Capacity;
            set => m_assetObjPool.Capacity = value;
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float AssetExpireTime
        {
            get => m_assetObjPool.ExpireTime;
            set => m_assetObjPool.ExpireTime = value;
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int AssetPriority
        {
            get => m_assetObjPool.Priority;
            set => m_assetObjPool.Priority = value;
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            if (m_assetObjPool != null)
            {
                m_assetObjPool.Recycle(asset);
            }
        }

        public void SetObjectPoolModule(IObjectPoolModule objPoolMgr)
        {
            if (objPoolMgr == null)
            {
                throw new QFrameworkException("Object pool manager is invalid.");
            }
            m_assetObjPool = objPoolMgr.CreateMultiGetObjectPool<AssetObject>("Asset Pool");
        }
    }
}