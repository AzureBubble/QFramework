using UnityEngine;

namespace QFramework
{
    [SerializeField]
    public class LoadAssetObject
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public ISetAssetObject SetAssetObject { get; }

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public Object AssetTarget { get; }

#if UNITY_EDITOR
        public bool IsSelected { get; set; }
#endif

        public LoadAssetObject(ISetAssetObject setAssetObject, Object assetTarget)
        {
            SetAssetObject = setAssetObject;
            AssetTarget = assetTarget;
        }
    }
}