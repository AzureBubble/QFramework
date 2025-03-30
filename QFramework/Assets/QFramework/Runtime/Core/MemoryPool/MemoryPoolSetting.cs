using UnityEngine;
using UnityEngine.Serialization;

namespace QFramework
{
    /// <summary>
    /// 内存强制检查类型。
    /// </summary>
    public enum MemoryStrictCheckType : byte
    {
        /// <summary>
        /// 总是启用。
        /// </summary>
        AlwaysEnable = 0,

        /// <summary>
        /// 仅在开发模式时启用。
        /// </summary>
        OnlyEnableWhenDevelopment,

        /// <summary>
        /// 仅在编辑器中启用。
        /// </summary>
        OnlyEnableInEditor,

        /// <summary>
        /// 总是禁用。
        /// </summary>
        AlwaysDisable,
    }

    /// <summary>
    /// 内存池模块设置。
    /// </summary>
    [DisallowMultipleComponent]
    public class MemoryPoolSetting : MonoBehaviour
    {
        [SerializeField]
        private MemoryStrictCheckType m_enableStrictCheck = MemoryStrictCheckType.OnlyEnableWhenDevelopment;

        /// <summary>
        /// 获取或设置是否开启强制检查。
        /// </summary>
        public bool EnableStrictCheck
        {
            get => MemoryPoolMgr.EnableStrictCheck;
            set
            {
                MemoryPoolMgr.EnableStrictCheck = value;
                if (value)
                {
                    Debugger.Info("Strict checking is enabled for the Memory Pool. It will drastically affect the performance.");
                }
            }
        }

        private void Start()
        {
            switch (m_enableStrictCheck)
            {
                case MemoryStrictCheckType.AlwaysEnable:
                    EnableStrictCheck = true;
                    break;

                case MemoryStrictCheckType.OnlyEnableWhenDevelopment:
                    EnableStrictCheck = Debug.isDebugBuild;
                    break;

                case MemoryStrictCheckType.OnlyEnableInEditor:
                    EnableStrictCheck = Application.isEditor;
                    break;

                default:
                    EnableStrictCheck = false;
                    break;
            }

            Destroy(gameObject);
        }
    }
}