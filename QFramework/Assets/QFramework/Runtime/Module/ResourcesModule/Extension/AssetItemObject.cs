using UnityEngine;

namespace QFramework
{
    public class AssetItemObject : BasePoolObject
    {
        public static AssetItemObject Create(string location, Object targetObj)
        {
            AssetItemObject item = MemoryPoolMgr.Get<AssetItemObject>();
            item.Initialize(location, targetObj);
            return item;
        }

        protected internal override void Release(bool isOnRelease)
        {
            if (TargetObj == null)
            {
                return;
            }
        }
    }
}