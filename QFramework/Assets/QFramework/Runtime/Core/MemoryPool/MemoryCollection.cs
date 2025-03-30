using System;
using System.Collections.Concurrent;

namespace QFramework
{
    public static partial class MemoryPoolMgr
    {
        /// <summary>
        /// 内存池。
        /// </summary>
        private sealed class MemoryPool
        {
            /// <summary>
            /// 未使用的内存对象队列
            /// </summary>
            private readonly ConcurrentQueue<IMemory> m_memoryQueue;

            /// <summary>
            /// 内存对象类型
            /// </summary>
            private readonly Type m_memoryObjType;

            /// <summary>
            /// 正在使用的内存对象个数
            /// </summary>
            public int UsingMemoryObjCount { get; private set; }

            /// <summary>
            /// 内存对象被申请的总次数
            /// </summary>
            public int GetMemoryObjCount { get; private set; }

            /// <summary>
            /// 内存对象被添加到池中的总次数
            /// </summary>
            public int AddMemoryObjCount { get; private set; }

            /// <summary>
            /// 内存对象从池中移除的总次数
            /// </summary>
            public int RemoveMemoryObjCount { get; private set; }

            /// <summary>
            /// 内存对象被释放的总次数
            /// </summary>
            public int ReleaseMemoryObjCount { get; private set; }

            public MemoryPool(Type memoryObjType)
            {
                m_memoryQueue = new ConcurrentQueue<IMemory>();
                m_memoryObjType = memoryObjType;
                UsingMemoryObjCount = 0;
                GetMemoryObjCount = 0;
                AddMemoryObjCount = 0;
                RemoveMemoryObjCount = 0;
                ReleaseMemoryObjCount = 0;
            }

            public bool Contains(IMemory memory)
            {
                foreach (var item in m_memoryQueue)
                {
                    if (item == memory)
                    {
                        return true;
                    }
                }
                return false;
            }

            #region Get

            public T Get<T>() where T : class, IMemory , new()
            {
                if (typeof(T) != m_memoryObjType)
                {
                    throw new Exception("Type is invalid.");
                }

                UsingMemoryObjCount++;
                GetMemoryObjCount++;

                if (m_memoryQueue.TryDequeue(out var memory))
                {
                    return memory as T;
                }

                AddMemoryObjCount++;
                return new T();
            }

            public IMemory Get()
            {
                UsingMemoryObjCount++;
                GetMemoryObjCount++;

                if (m_memoryQueue.TryDequeue(out var memory))
                {
                    return memory;
                }

                AddMemoryObjCount++;
                return Activator.CreateInstance(m_memoryObjType) as IMemory;
            }

            #endregion

            #region Release

            public void Release(IMemory memory)
            {
                memory.OnRelease();
                if (EnableStrictCheck && Contains(memory))
                {
                    throw new Exception("The memory has been released.");
                }

                m_memoryQueue.Enqueue(memory);

                ReleaseMemoryObjCount++;
                UsingMemoryObjCount--;
            }

            #endregion

            #region Add

            public void AddMemoryObjCnt<T>(int cnt) where T : class, IMemory, new()
            {
                if (typeof(T) != m_memoryObjType)
                {
                    throw new Exception("Type is invalid.");
                }

                if (m_memoryQueue == null)
                {
                    return;
                }
                AddMemoryObjCount += cnt;

                while (cnt-- > 0)
                {
                    m_memoryQueue.Enqueue(new T());
                }
            }

            public void AddMemoryObjCnt(int cnt)
            {
                if (m_memoryQueue == null)
                {
                    return;
                }
                AddMemoryObjCount += cnt;

                while (cnt-- > 0)
                {
                    m_memoryQueue.Enqueue(Activator.CreateInstance(m_memoryObjType) as IMemory);
                }
            }

            #endregion

            #region Remove

            public void RemoveMemoryObjCnt(int cnt)
            {
                if (m_memoryQueue == null)
                {
                    return;
                }

                if (cnt > m_memoryQueue.Count)
                {
                    cnt = m_memoryQueue.Count;
                }

                RemoveMemoryObjCount += cnt;

                while (cnt-- > 0)
                {
                    m_memoryQueue.TryDequeue(out _);
                }
            }

            public void RemoveAll()
            {
                if (m_memoryQueue != null)
                {
                    RemoveMemoryObjCount += m_memoryQueue.Count;

                    while (m_memoryQueue.TryDequeue(out _)) ;
                }
            }

            #endregion
        }
    }
}