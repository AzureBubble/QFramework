using System;
using System.Buffers;
using QFramework.Utility;

namespace QFramework
{
    public sealed partial class ObjectPoolMgr : Module, IObjectPoolModule, IUpdateModule
    {
        /// <summary>
        /// 内部对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        private sealed class PoolObject<T> : IMemory where T : BasePoolObject
        {
            private T m_object;
            private int m_getCount;

            /// <summary>
            /// 获取对象的获取计数。
            /// </summary>
            public int GetCount => m_getCount;

            public string PoolObjName => m_object.ObjName;

            /// <summary>
            /// 获取对象是否被加锁。
            /// </summary>
            public bool Locked
            {
                get => m_object.Locked;
                set => m_object.Locked = value;
            }

            /// <summary>
            /// 获取对象的优先级。
            /// </summary>
            public int Priority
            {
                get => m_object.Priority;
                set => m_object.Priority = value;
            }

            /// <summary>
            /// 获取自定义释放检查标记。
            /// </summary>
            public bool CustomCanReleaseFlag => m_object.CustomCanReleaseFlag;

            /// <summary>
            /// 获取对象上次使用时间。
            /// </summary>
            public DateTime LastUseTime => m_object.LastUseTime;

            /// <summary>
            /// 获取对象是否正在使用。
            /// </summary>
            public bool IsUsing => m_getCount > 0;


            /// <summary>
            /// 初始化内部对象的新实例。
            /// </summary>
            public PoolObject()
            {
                m_object = null;
                m_getCount = 0;
            }

            /// <summary>
            /// 创建内部对象。
            /// </summary>
            /// <param name="obj">对象。</param>
            /// <param name="isGot">对象是否已被获取。</param>
            /// <returns>创建的内部对象。</returns>
            public static PoolObject<T> Create(T obj, bool isGot)
            {
                if (obj == null)
                {
                    throw new QFrameworkException("Object is invalid.");
                }

                PoolObject<T> internalObject = MemoryPoolMgr.Get<PoolObject<T>>();
                internalObject.m_object = obj;
                internalObject.m_getCount = isGot ? 1 : 0;
                if (isGot)
                {
                    obj.OnGet();
                }

                return internalObject;
            }

            /// <summary>
            /// 查看对象。
            /// </summary>
            /// <returns>对象。</returns>
            public T Peek()
            {
                return m_object;
            }

            /// <summary>
            /// 获取对象。
            /// </summary>
            /// <returns>对象。</returns>
            public T Get()
            {
                m_getCount++;
                m_object.LastUseTime = DateTime.UtcNow;
                m_object.OnGet();
                return m_object;
            }

            /// <summary>
            /// 回收对象。
            /// </summary>
            public void Recycle()
            {
                m_object.OnRecycle();
                m_object.LastUseTime = DateTime.UtcNow;
                m_getCount--;
                if (m_getCount < 0)
                {
                    throw new QFrameworkException(StringFormatHelper.Format("Object '{0}' spawn count is less than 0.", PoolObjName));
                }
            }

            /// <summary>
            /// 清理内部对象。
            /// </summary>
            public void OnRelease()
            {
                m_object = null;
                m_getCount = 0;
            }

            /// <summary>
            /// 释放对象。
            /// </summary>
            /// <param name="isShutdown">是否是关闭对象池时触发。</param>
            public void Release(bool isShutdown)
            {
                m_object.Release(isShutdown);
                MemoryPoolMgr.Release(m_object);
            }
        }
    }
}