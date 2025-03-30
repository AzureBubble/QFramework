using System;
using System.Collections.Concurrent;

namespace QFramework
{
    /// <summary>
    /// 内存池管理器
    /// </summary>
    public static partial class MemoryPoolMgr
    {
        /// <summary>
        /// 获取或设置是否开启强制检查。
        /// </summary>
        public static bool EnableStrictCheck { get; set; } = false;
        private static readonly ConcurrentDictionary<Type, MemoryPool> m_memoryObjectDict = new ConcurrentDictionary<Type, MemoryPool>();

        /// <summary>
        /// 获取内存池数量
        /// </summary>
        public static int Count => m_memoryObjectDict.Count;

        #region Get

        /// <summary>
        /// 从内存池获取内存对象。
        /// </summary>
        /// <typeparam name="T">内存对象类型。</typeparam>
        /// <returns>内存对象。</returns>
        public static T Get<T>() where T : class, IMemory, new()
        {
            return GetMemoryPool(typeof(T)).Get<T>();
        }

        /// <summary>
        /// 从内存池获取内存对象。
        /// </summary>
        /// <param name="memoryType">内存对象类型。</param>
        /// <returns>内存对象。</returns>
        public static IMemory Get(Type memoryType)
        {
            InternalCheckMemoryType(memoryType);
            return GetMemoryPool(memoryType).Get();
        }

        private static MemoryPool GetMemoryPool(Type memoryType)
        {
            if (memoryType == null)
            {
                throw new Exception("MemoryType is invalid.");
            }

            if (!m_memoryObjectDict.TryGetValue(memoryType, out var memoryObj))
            {
                memoryObj = new MemoryPool(memoryType);
                m_memoryObjectDict.TryAdd(memoryType, memoryObj);
            }

            return memoryObj;
        }

        #endregion

        #region Release

        public static void Release(IMemory memory)
        {
            if (memory == null)
            {
                throw new Exception("Memory is invalid.");
            }

            Type memoryType = memory.GetType();
            InternalCheckMemoryType(memoryType);
            GetMemoryPool(memoryType).Release(memory);
        }

        #endregion

        #region Add

        public static void Add<T>(int cnt) where T : class, IMemory, new()
        {
            GetMemoryPool(typeof(T)).AddMemoryObjCnt<T>(cnt);
        }

        public static void Add(Type memoryType, int cnt)
        {
            InternalCheckMemoryType(memoryType);
            GetMemoryPool(memoryType).AddMemoryObjCnt(cnt);
        }

        #endregion

        #region Remove

        public static void Remove<T>(int cnt) where T : class, IMemory, new()
        {
            GetMemoryPool(typeof(T)).RemoveMemoryObjCnt(cnt);
        }

        public static void Remove(Type memoryType, int cnt)
        {
            InternalCheckMemoryType(memoryType);
            GetMemoryPool(memoryType).RemoveMemoryObjCnt(cnt);
        }

        public static void RemoveAll(Type memoryType)
        {
            InternalCheckMemoryType(memoryType);
            GetMemoryPool(memoryType).RemoveAll();
        }

        #endregion

        #region Check

        private static void InternalCheckMemoryType(Type memoryType)
        {
            if (!EnableStrictCheck)
            {
                return;
            }

            if (memoryType == null)
            {
                throw new Exception("Memory type is invalid.");
            }

            if (!memoryType.IsClass || memoryType.IsAbstract)
            {
                throw new Exception("Memory type is not a non-abstract class type.");
            }

            if (!typeof(IMemory).IsAssignableFrom(memoryType))
            {
                throw new Exception($"Memory type '{memoryType.FullName}' is invalid.");
            }
        }

        #endregion

        /// <summary>
        /// 清空所有的内存池
        /// </summary>
        public static void ClearAll()
        {
            foreach (var memoryObject in m_memoryObjectDict.Values)
            {
                memoryObject.RemoveAll();
            }
            m_memoryObjectDict.Clear();
        }
    }
}