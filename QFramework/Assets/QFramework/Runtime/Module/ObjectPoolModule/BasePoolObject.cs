using System;
using QFramework.Utility;

namespace QFramework
{
    /// <summary>
    /// 对象基类。
    /// </summary>
    public abstract class BasePoolObject : IMemory
    {
        private string m_objName;
        public string ObjName => m_objName;
        private object m_targetObj;
        public object TargetObj => m_targetObj;
        public bool Locked { get; set; }
        public int Priority { get; set; }
        public DateTime LastUseTime { get; set; }

        /// <summary>
        /// 获取自定义释放检查标记。
        /// </summary>
        public virtual bool CustomCanReleaseFlag => true;

        public BasePoolObject()
        {
            m_objName = null;
            m_targetObj = null;
            Locked = false;
            Priority = 0;
            LastUseTime = default(DateTime);
        }

        /// <summary>
        /// 初始化对象基类。
        /// </summary>
        /// <param name="target">对象。</param>
        protected void Initialize(object target)
        {
            Initialize(null, target, false, 0);
        }

        /// <summary>
        /// 初始化对象基类。
        /// </summary>
        /// <param name="name">对象名称。</param>
        /// <param name="target">对象。</param>
        protected void Initialize(string name, object target)
        {
            Initialize(name, target, false, 0);
        }

        /// <summary>
        /// 初始化对象基类。
        /// </summary>
        /// <param name="name">对象名称。</param>
        /// <param name="target">对象。</param>
        /// <param name="locked">对象是否被加锁。</param>
        protected void Initialize(string name, object target, bool locked)
        {
            Initialize(name, target, locked, 0);
        }

        /// <summary>
        /// 初始化对象基类。
        /// </summary>
        /// <param name="name">对象名称。</param>
        /// <param name="target">对象。</param>
        /// <param name="priority">对象的优先级。</param>
        protected void Initialize(string name, object target, int priority)
        {
            Initialize(name, target, false, priority);
        }

        /// <summary>
        /// 初始化对象基类。
        /// </summary>
        /// <param name="name">对象名称。</param>
        /// <param name="target">对象。</param>
        /// <param name="locked">对象是否被加锁。</param>
        /// <param name="priority">对象的优先级。</param>
        protected void Initialize(string name, object target, bool locked, int priority)
        {
            if (target == null)
            {
                throw new QFrameworkException(StringFormatHelper.Format("Target '{0}' is invalid.", name));
            }

            m_objName = name == null ? string.Empty : name;
            m_targetObj = target;
            Locked = locked;
            Priority = priority;
            LastUseTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 获取对象时的事件。
        /// </summary>
        protected internal virtual void OnGet()
        {
        }

        /// <summary>
        /// 回收对象时的事件。
        /// </summary>
        protected internal virtual void OnRecycle()
        {
        }

        /// <summary>
        /// 释放对象。
        /// </summary>
        /// <param name="isOnRelease">是否是释放对象池时触发。</param>
        protected internal abstract void Release(bool isOnRelease);

        /// <summary>
        /// 释放对象基类。
        /// </summary>
        public virtual void OnRelease()
        {
            m_objName = null;
            m_targetObj = null;
            Locked = false;
            Priority = 0;
            LastUseTime = default(DateTime);
        }
    }
}