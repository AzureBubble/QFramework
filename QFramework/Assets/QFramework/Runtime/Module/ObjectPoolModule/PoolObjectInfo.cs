using System;
using System.Runtime.InteropServices;

namespace QFramework
{
    /// <summary>
    /// 对象信息。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct PoolObjectInfo
    {
        /// <summary>
        /// 获取对象名称。
        /// </summary>
        public string ObjectName { get; }

        /// <summary>
        /// 获取对象是否被加锁。
        /// </summary>
        public bool Locked { get; }

        /// <summary>
        /// 获取对象自定义释放检查标记。
        /// </summary>
        public bool CustomCanReleaseFlag { get; }

        /// <summary>
        /// 获取对象的优先级。
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// 获取对象上次使用时间。
        /// </summary>
        public DateTime LastUseTime { get; }

        /// <summary>
        /// 获取对象是否正在使用。
        /// </summary>
        public bool IsInUse => GetCount > 0;

        /// <summary>
        /// 获取对象的获取计数。
        /// </summary>
        public readonly int GetCount { get; }

        /// <summary>
        /// 初始化对象信息的新实例。
        /// </summary>
        /// <param name="objectName">对象名称。</param>
        /// <param name="locked">对象是否被加锁。</param>
        /// <param name="customCanReleaseFlag">对象自定义释放检查标记。</param>
        /// <param name="priority">对象的优先级。</param>
        /// <param name="lastUseTime">对象上次使用时间。</param>
        /// <param name="getCount">对象的获取计数。</param>
        public PoolObjectInfo(string objectName, bool locked, bool customCanReleaseFlag, int priority, DateTime lastUseTime, int getCount)
        {
            ObjectName = objectName;
            Locked = locked;
            CustomCanReleaseFlag = customCanReleaseFlag;
            Priority = priority;
            LastUseTime = lastUseTime;
            GetCount = getCount;
        }
    }
}