using System;
using System.Collections.Generic;
using QFramework.Utility;

namespace QFramework
{
    /// <summary>
    /// 对象池管理器。
    /// </summary>
    public sealed partial class ObjectPoolMgr : Module, IObjectPoolModule, IUpdateModule
    {
        /// <summary>
        /// 默认容量
        /// </summary>
        private const int DEFAULT_CAPACITY = int.MaxValue;

        /// <summary>
        /// 默认失效时间
        /// </summary>
        private const float DEFAULT_EXPIRE_TIME = float.MaxValue;

        /// <summary>
        /// 默认优先级
        /// </summary>
        private const int DEFAULT_PRIORITY = int.MaxValue;

        private readonly Dictionary<TypeNamePair, BaseObjectPool> m_objectPools;
        private readonly List<BaseObjectPool> m_cacheAllObjectPools;
        private readonly Comparison<BaseObjectPool> m_objectPoolComparer;

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        public override int Priority => 6;

        /// <summary>
        /// 获取对象池数量。
        /// </summary>
        public int Count => m_objectPools.Count;

        public ObjectPoolMgr()
        {
            m_objectPools = new Dictionary<TypeNamePair, BaseObjectPool>();
            m_cacheAllObjectPools = new List<BaseObjectPool>();
            m_objectPoolComparer = DefaultObjectPoolComparer;
        }

        public override void OnInit()
        {
            Debugger.Info("Object pool system onInit.");
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var objectPool in m_objectPools.Values)
            {
                objectPool.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 释放所有对象池
        /// </summary>
        public override void OnDispose()
        {
            foreach (var objectPool in m_objectPools.Values)
            {
                objectPool.Release();
            }

            m_objectPools.Clear();
            m_cacheAllObjectPools.Clear();
        }

        #region CheckObjectPoolIsExist

        public bool CheckObjectPoolIsExist<T>() where T : BasePoolObject
        {
            return CheckObjectPoolIsExist(new TypeNamePair(typeof(T)));
        }

        public bool CheckObjectPoolIsExist(Type objectType)
        {
            if (objectType == null)
            {
                throw new QFrameworkException("Object type is invalid.");
            }
            if (!typeof(BasePoolObject).IsAssignableFrom(objectType))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return CheckObjectPoolIsExist(new TypeNamePair(objectType));
        }

        public bool CheckObjectPoolIsExist<T>(string name) where T : BasePoolObject
        {
            return CheckObjectPoolIsExist(new TypeNamePair(typeof(T), name));
        }

        public bool CheckObjectPoolIsExist(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new QFrameworkException("Object type is invalid.");
            }
            if (!typeof(BasePoolObject).IsAssignableFrom(objectType))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return CheckObjectPoolIsExist(new TypeNamePair(objectType, name));
        }

        public bool CheckObjectPoolIsExist(Predicate<BaseObjectPool> condition)
        {
            if (condition == null)
            {
                throw new QFrameworkException("Condition is invalid.");
            }

            foreach (var objectPool in m_objectPools.Values)
            {
                if (condition(objectPool))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckObjectPoolIsExist(TypeNamePair typeNamePair)
        {
            return m_objectPools.ContainsKey(typeNamePair);
        }

        #endregion

        #region GetObjectPool

        public IObjectPool<T> GetObjectPool<T>() where T : BasePoolObject
        {
            return GetObjectPool(new TypeNamePair(typeof(T))) as IObjectPool<T>;
        }

        public BaseObjectPool GetObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new QFrameworkException("Object type is invalid.");
            }

            if (!typeof(BasePoolObject).IsAssignableFrom(objectType))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return GetObjectPool(new TypeNamePair(objectType));
        }

        public IObjectPool<T> GetObjectPool<T>(string name) where T : BasePoolObject
        {
            return (IObjectPool<T>)GetObjectPool(new TypeNamePair(typeof(T), name));
        }

        public BaseObjectPool GetObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new QFrameworkException("Object type is invalid.");
            }

            if (!typeof(BasePoolObject).IsAssignableFrom(objectType))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return GetObjectPool(new TypeNamePair(objectType, name));
        }

        public BaseObjectPool GetObjectPool(Predicate<BaseObjectPool> condition)
        {
            if (condition == null)
            {
                throw new QFrameworkException("Condition is invalid.");
            }

            foreach (var objectPool in m_objectPools.Values)
            {
                if (condition(objectPool))
                {
                    return objectPool;
                }
            }

            return null;
        }

        public BaseObjectPool[] GetObjectPools(Predicate<BaseObjectPool> condition)
        {
            if (condition == null)
            {
                throw new QFrameworkException("Condition is invalid.");
            }

            List<BaseObjectPool> results = new List<BaseObjectPool>();
            foreach (var objectPool in m_objectPools.Values)
            {
                if (condition(objectPool))
                {
                    results.Add(objectPool);
                }
            }

            return results.ToArray();
        }

        public void GetObjectPools(Predicate<BaseObjectPool> condition, ref List<BaseObjectPool> results)
        {
            if (results == null)
            {
                results = new List<BaseObjectPool>();
            }

            GetObjectPools(condition, results);
        }

        public void GetObjectPools(Predicate<BaseObjectPool> condition, List<BaseObjectPool> results)
        {
            if (condition == null)
            {
                throw new QFrameworkException("Condition is invalid.");
            }

            if (results == null)
            {
                throw new QFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (var objectPool in m_objectPools.Values)
            {
                if (condition(objectPool))
                {
                    results.Add(objectPool);
                }
            }
        }

        public BaseObjectPool[] GetAllObjectPools()
        {
            return GetAllObjectPools(false);
        }

        public void GetAllObjectPools(List<BaseObjectPool> results)
        {
            GetAllObjectPools(false, results);
        }

        public void GetAllObjectPools(ref List<BaseObjectPool> results)
        {
            GetAllObjectPools(false, ref results);
        }

        public BaseObjectPool[] GetAllObjectPools(bool sort)
        {
            if (sort)
            {
                List<BaseObjectPool> results = new List<BaseObjectPool>();
                foreach (var objectPool in m_objectPools.Values)
                {
                    results.Add(objectPool);
                }

                results.Sort(m_objectPoolComparer);
                return results.ToArray();
            }
            else
            {
                int index = 0;
                BaseObjectPool[] results = new BaseObjectPool[m_objectPools.Count];
                foreach (var objectPool in m_objectPools.Values)
                {
                    results[index++] = objectPool;
                }

                return results;
            }
        }

        public void GetAllObjectPools(bool sort, List<BaseObjectPool> results)
        {
            if (results == null)
            {
                throw new QFrameworkException("Results is invalid.");
            }
            results.Clear();
            foreach (KeyValuePair<TypeNamePair, BaseObjectPool> objectPool in m_objectPools)
            {
                results.Add(objectPool.Value);
            }

            if (sort)
            {
                results.Sort(m_objectPoolComparer);
            }
        }

        public void GetAllObjectPools(bool sort, ref List<BaseObjectPool> results)
        {
            if (results == null)
            {
                results = new List<BaseObjectPool>();
            }

            GetAllObjectPools(results);
        }

        private BaseObjectPool GetObjectPool(TypeNamePair typeNamePair)
        {
            if (m_objectPools.TryGetValue(typeNamePair, out var pool))
            {
                return pool;
            }
            return null;
        }

        #endregion

        #region CreateObjectPool

        public IObjectPool<T> CreateSingleGetObjectPool<T>() where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, false, DEFAULT_EXPIRE_TIME, DEFAULT_CAPACITY, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType)
        {
            return CreateObjectPool(objectType, String.Empty, false, DEFAULT_EXPIRE_TIME, DEFAULT_CAPACITY, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(string name) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, false, DEFAULT_EXPIRE_TIME, DEFAULT_CAPACITY, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, string name)
        {
            return CreateObjectPool(objectType, name, false, DEFAULT_EXPIRE_TIME, DEFAULT_CAPACITY, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(int capacity) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, false, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, int capacity)
        {
            return CreateObjectPool(objectType, String.Empty, false, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(float expireTime) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, false, expireTime, DEFAULT_CAPACITY, expireTime, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, float expireTime)
        {
            return CreateObjectPool(objectType, String.Empty, false, expireTime, DEFAULT_CAPACITY, expireTime, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(string name, int capacity) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, false, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, string name, int capacity)
        {
            return CreateObjectPool(objectType, name, false, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(string name, float expireTime) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, false, expireTime, DEFAULT_CAPACITY, expireTime, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, string name, float expireTime)
        {
            return CreateObjectPool(objectType, name, false, expireTime, DEFAULT_CAPACITY, expireTime, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(int capacity, float expireTime) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, false, expireTime, capacity, expireTime, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, int capacity, float expireTime)
        {
            return CreateObjectPool(objectType, String.Empty, false, expireTime, capacity, expireTime, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(int capacity, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, false, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, int capacity, int priority)
        {
            return CreateObjectPool(objectType, String.Empty, false, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, priority);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(float expireTime, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, false, expireTime, DEFAULT_CAPACITY, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, float expireTime, int priority)
        {
            return CreateObjectPool(objectType, String.Empty, false, expireTime, DEFAULT_CAPACITY, expireTime, priority);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(string name, int capacity, float expireTime) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, false, expireTime, capacity, expireTime, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            return CreateObjectPool(objectType, name, false, expireTime, capacity, expireTime, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(string name, int capacity, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, false, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, string name, int capacity, int priority)
        {
            return CreateObjectPool(objectType, name, false, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, priority);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(string name, float expireTime, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, false, expireTime, DEFAULT_CAPACITY, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            return CreateObjectPool(objectType, name, false, expireTime, DEFAULT_CAPACITY, expireTime, priority);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(int capacity, float expireTime, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, false, expireTime, capacity, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            return CreateObjectPool(objectType, String.Empty, false, expireTime, capacity, expireTime, priority);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, false, expireTime, capacity, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, string name, int capacity, float expireTime, int priority)
        {
            return CreateObjectPool(objectType, name, false, expireTime, capacity, expireTime, priority);
        }

        public IObjectPool<T> CreateSingleGetObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime,
            int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, false, autoReleaseInterval, capacity, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateSingleGetObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity,
            float expireTime, int priority)
        {
            return CreateObjectPool(objectType, name, false, autoReleaseInterval, capacity, expireTime, priority);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>() where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, true, DEFAULT_EXPIRE_TIME, DEFAULT_CAPACITY, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType)
        {
            return CreateObjectPool(objectType, String.Empty, true, DEFAULT_EXPIRE_TIME, DEFAULT_CAPACITY, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(string name) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, true, DEFAULT_EXPIRE_TIME, DEFAULT_CAPACITY, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, string name)
        {
            return CreateObjectPool(objectType, name, true, DEFAULT_EXPIRE_TIME, DEFAULT_CAPACITY, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(int capacity) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, true, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, int capacity)
        {
            return CreateObjectPool(objectType, String.Empty, true, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(float expireTime) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, true, expireTime, DEFAULT_CAPACITY, expireTime, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, float expireTime)
        {
            return CreateObjectPool(objectType, String.Empty, true, expireTime, DEFAULT_CAPACITY, expireTime, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(string name, int capacity) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, true, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, string name, int capacity)
        {
            return CreateObjectPool(objectType, name, true, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(string name, float expireTime) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, true, expireTime, DEFAULT_CAPACITY, expireTime, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, string name, float expireTime)
        {
            return CreateObjectPool(objectType, name, true, expireTime, DEFAULT_CAPACITY, expireTime, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(int capacity, float expireTime) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, true, expireTime, capacity, expireTime, DEFAULT_PRIORITY) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, int capacity, float expireTime)
        {
            return CreateObjectPool(objectType, String.Empty, true, expireTime, capacity, expireTime, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(int capacity, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, true, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, int capacity, int priority)
        {
            return CreateObjectPool(objectType, String.Empty, true, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, priority);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(float expireTime, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), String.Empty, true, expireTime, DEFAULT_CAPACITY, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, float expireTime, int priority)
        {
            return CreateObjectPool(objectType, String.Empty, true, expireTime, DEFAULT_CAPACITY, expireTime, priority);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(string name, int capacity, float expireTime) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, true, expireTime, capacity, expireTime, DEFAULT_PRIORITY) as ObjectPool<T>;

        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            return CreateObjectPool(objectType, name, true, expireTime, capacity, expireTime, DEFAULT_PRIORITY);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(string name, int capacity, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, true, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, string name, int capacity, int priority)
        {
            return CreateObjectPool(objectType, name, true, DEFAULT_EXPIRE_TIME, capacity, DEFAULT_EXPIRE_TIME, priority);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(string name, float expireTime, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, true, expireTime, DEFAULT_CAPACITY, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            return CreateObjectPool(objectType, name, true, expireTime, DEFAULT_CAPACITY, expireTime, priority);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(int capacity, float expireTime, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), string.Empty, true, expireTime, capacity, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            return CreateObjectPool(objectType, string.Empty, true, expireTime, capacity, expireTime, priority);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, true, expireTime, capacity, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, string name, int capacity, float expireTime, int priority)
        {
            return CreateObjectPool(objectType, name, true, expireTime, capacity, expireTime, priority);
        }

        public IObjectPool<T> CreateMultiGetObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime,
            int priority) where T : BasePoolObject
        {
            return CreateObjectPool(typeof(T), name, true, autoReleaseInterval, capacity, expireTime, priority) as ObjectPool<T>;
        }

        public BaseObjectPool CreateMultiGetObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity,
            float expireTime, int priority)
        {
            return CreateObjectPool(objectType, name, true, autoReleaseInterval, capacity, expireTime, priority);
        }

        private IObjectPool<T> CreateObjectPool<T>(string poolName, bool allowMultiGet, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : BasePoolObject
        {
            TypeNamePair typeNamePair = new TypeNamePair(typeof(T), poolName);
            if (CheckObjectPoolIsExist<T>(poolName))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Already exist object pool '{0}'.", typeNamePair));
            }

            ObjectPool<T> objectPool = new ObjectPool<T>(poolName, allowMultiGet, autoReleaseInterval, capacity, expireTime, priority);
            m_objectPools.Add(typeNamePair, objectPool);
            return objectPool;
        }

        private BaseObjectPool CreateObjectPool(Type objectType, string poolName, bool allowMultiGet, float autoReleaseInterval, int capacity, float expireTime, int priority)
        {
            if (objectType == null)
            {
                throw new QFrameworkException("Object type is invalid.");
            }

            if (!typeof(BasePoolObject).IsAssignableFrom(objectType))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            TypeNamePair typeNamePair = new TypeNamePair(objectType, poolName);
            if (CheckObjectPoolIsExist(objectType, poolName))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Already exist object pool '{0}'.", typeNamePair));
            }

            Type objectPoolType = typeof(ObjectPool<>).MakeGenericType(objectType);
            BaseObjectPool objectPool =
                (BaseObjectPool)Activator.CreateInstance(objectPoolType, poolName, allowMultiGet, autoReleaseInterval, capacity, expireTime, priority);
            m_objectPools.Add(typeNamePair, objectPool);
            return objectPool;
        }

        #endregion

        #region DestoryObjectPool

        public bool DestroyObjectPool<T>() where T : BasePoolObject
        {
            return DestroyObjectPool(new TypeNamePair(typeof(T)));
        }

        public bool DestroyObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new QFrameworkException("Object type is invalid.");
            }
            if (!typeof(BasePoolObject).IsAssignableFrom(objectType))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return DestroyObjectPool(new TypeNamePair(objectType));
        }

        public bool DestroyObjectPool<T>(string name) where T : BasePoolObject
        {
            return DestroyObjectPool(new TypeNamePair(typeof(T), name));
        }

        public bool DestroyObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new QFrameworkException("Object type is invalid.");
            }
            if (!typeof(BasePoolObject).IsAssignableFrom(objectType))
            {
                throw new QFrameworkException(StringFormatHelper.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return DestroyObjectPool(new TypeNamePair(objectType, name));
        }

        public bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : BasePoolObject
        {
            if (objectPool == null)
            {
                throw new QFrameworkException("Object pool is invalid.");
            }
            return DestroyObjectPool(new TypeNamePair(typeof(T), objectPool.PoolName));
        }

        public bool DestroyObjectPool(BaseObjectPool objectPool)
        {
            if (objectPool == null)
            {
                throw new QFrameworkException("Object pool is invalid.");
            }

            return DestroyObjectPool(new TypeNamePair(objectPool.ObjectType, objectPool.PoolName));
        }

        private bool DestroyObjectPool(TypeNamePair typeNamePair)
        {
            if (m_objectPools.TryGetValue(typeNamePair, out var objectPool))
            {
                objectPool.Release();
                return m_objectPools.Remove(typeNamePair);
            }

            return false;
        }

        #endregion

        #region Release

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        public void Release()
        {
            Debugger.Info("Release object pool can release objects...");
            GetAllObjectPools(true, m_cacheAllObjectPools);
            foreach (var objectPool in m_cacheAllObjectPools)
            {
                objectPool.Release();
            }
        }

        /// <summary>
        /// 释放对象池中的所有未使用对象。
        /// </summary>
        public void ReleaseAllUnused()
        {
            Debugger.Info("Release object pool all unused objects...");
            GetAllObjectPools(true, m_cacheAllObjectPools);
            foreach (var objectPool in m_cacheAllObjectPools)
            {
                objectPool.ReleaseAllUnused();
            }
        }

        #endregion

        private static int DefaultObjectPoolComparer(BaseObjectPool a, BaseObjectPool b)
        {
            return a.Priority.CompareTo(b.Priority);
        }
    }
}