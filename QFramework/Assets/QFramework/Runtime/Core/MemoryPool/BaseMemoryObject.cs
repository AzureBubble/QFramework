using System;

namespace QFramework
{
    /// <summary>
    /// 内存池对象基类。
    /// </summary>
    public abstract class BaseMemoryObject : IMemory
    {
        /// <summary>
        /// 从内存池中初始化。
        /// </summary>
        public abstract void InitFromPool();

        /// <summary>
        /// 回收到内存池。
        /// </summary>
        public abstract void RecycleToPool();

        /// <summary>
        /// 释放对象并返回对象池。
        /// </summary>
        public virtual void OnRelease()
        {
        }
    }

    public static partial class MemoryPoolMgr
    {
        /// <summary>
        /// 从内存池获取内存对象。
        /// </summary>
        /// <typeparam name="T">内存对象类型。</typeparam>
        /// <returns>内存对象。</returns>
        public static T Alloc<T>() where T : BaseMemoryObject, new()
        {
            T memory = Get<T>();
            memory.InitFromPool();
            return memory;
        }

        /// <summary>
        /// 将内存对象归还内存池。
        /// </summary>
        /// <param name="memory">内存对象。</param>
        public static void Dealloc(BaseMemoryObject memory)
        {
            if (memory == null)
            {
                throw new Exception("Memory is invalid.");
            }

            memory.RecycleToPool();
            Release(memory);
        }
    }
}