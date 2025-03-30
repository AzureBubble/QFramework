using System;
using System.Collections.Generic;
using QFramework.Utility;

namespace QFramework
{
    public sealed partial class ObjectPoolMgr : Module, IObjectPoolModule
    {
        private sealed class ObjectPool<T> : BaseObjectPool, IObjectPool<T> where T : BasePoolObject
        {
            private readonly QMultiDictionary<string, PoolObject<T>> m_objects;
            private readonly Dictionary<object, PoolObject<T>> m_objectMap;
            private readonly ReleaseObjectFilterCallback<T> m_defaultReleaseObjectFilterCallback;
            private readonly List<T> m_cachedCanReleaseObjects;
            private readonly List<T> m_cachedToReleaseObjects;
            private readonly bool m_allowMultiGet;
            private float m_autoReleaseInterval;
            private int m_capacity;
            private float m_expireTime;
            private int m_priority;
            private float m_autoReleaseTime;

            public override Type ObjectType => typeof(T);
            /// <summary>
            /// 获取对象池中对象的数量。
            /// </summary>
            public override int Count => m_objectMap.Count;

            /// <summary>
            /// 获取对象池中能被释放的对象的数量。
            /// </summary>
            public override int CanReleaseCount
            {
                get
                {
                    GetCanReleaseObjects(m_cachedCanReleaseObjects);
                    return m_cachedCanReleaseObjects.Count;
                }
            }

            /// <summary>
            /// 初始化对象池的新实例。
            /// </summary>
            /// <param name="name">对象池名称。</param>
            /// <param name="allowMultiGet">是否允许对象被多次获取。</param>
            /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
            /// <param name="capacity">对象池的容量。</param>
            /// <param name="expireTime">对象池对象过期秒数。</param>
            /// <param name="priority">对象池的优先级。</param>
            public ObjectPool(string name, bool allowMultiGet, float autoReleaseInterval, int capacity, float expireTime, int priority)
                : base(name)
            {
                m_objects = new QMultiDictionary<string, PoolObject<T>>();
                m_objectMap = new Dictionary<object, PoolObject<T>>();
                m_defaultReleaseObjectFilterCallback = DefaultReleaseObjectFilterCallback;
                m_cachedCanReleaseObjects = new List<T>();
                m_cachedToReleaseObjects = new List<T>();
                m_allowMultiGet = allowMultiGet;
                m_autoReleaseInterval = autoReleaseInterval;
                Capacity = capacity;
                ExpireTime = expireTime;
                m_priority = priority;
                m_autoReleaseTime = 0f;
            }

            private void GetCanReleaseObjects(List<T> results)
            {
                if (results == null)
                {
                    throw new QFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (var internalObject in m_objectMap.Values)
                {
                    if (internalObject.IsUsing || internalObject.Locked || !internalObject.CustomCanReleaseFlag)
                    {
                        continue;
                    }

                    results.Add(internalObject.Peek());
                }
            }

            /// <summary>
            /// 获取是否允许对象被多次获取。
            /// </summary>
            public override bool AllowMultiGet => m_allowMultiGet;

            /// <summary>
            /// 获取或设置对象池自动释放可释放对象的间隔秒数。
            /// </summary>
            public override float AutoReleaseInterval
            {
                get => m_autoReleaseInterval;
                set => m_autoReleaseInterval = value;
            }

            /// <summary>
            /// 获取或设置对象池的容量。
            /// </summary>
            public override int Capacity
            {
                get => m_capacity;
                set
                {
                    if (value < 0)
                    {
                        throw new QFrameworkException("Capacity is invalid.");
                    }

                    if (m_capacity == value)
                    {
                        return;
                    }

                    m_capacity = value;
                    Release();
                }
            }

            /// <summary>
            /// 获取或设置对象池对象过期秒数。
            /// </summary>
            public override float ExpireTime
            {
                get => m_expireTime;

                set
                {
                    if (value < 0f)
                    {
                        throw new QFrameworkException("ExpireTime is invalid.");
                    }

                    if (Math.Abs(ExpireTime - value) < 0.01f)
                    {
                        return;
                    }

                    m_expireTime = value;
                    Release();
                }
            }

            /// <summary>
            /// 获取或设置对象池的优先级。
            /// </summary>
            public override int Priority
            {
                get => m_priority;
                set => m_priority = value;
            }

            /// <summary>
            /// 创建对象。
            /// </summary>
            /// <param name="obj">对象。</param>
            /// <param name="isGot">对象是否已被获取。</param>
            public void Create(T obj, bool isGot)
            {
                if (obj == null)
                {
                    throw new QFrameworkException("Object is invalid.");
                }

                PoolObject<T> internalObject = PoolObject<T>.Create(obj, isGot);
                m_objects.Add(obj.ObjName, internalObject);
                m_objectMap.Add(obj.TargetObj, internalObject);

                if (Count > m_capacity)
                {
                    Release();
                }
            }

            /// <summary>
            /// 检查对象。
            /// </summary>
            /// <returns>要检查的对象是否存在。</returns>
            public bool CanGet()
            {
                return CanGet(string.Empty);
            }

            /// <summary>
            /// 检查对象。
            /// </summary>
            /// <param name="objName">对象名称。</param>
            /// <returns>要检查的对象是否存在。</returns>
            public bool CanGet(string objName)
            {
                if (string.IsNullOrEmpty(objName))
                {
                    throw new QFrameworkException("Name is invalid.");
                }

                if (m_objects.TryGetValue(objName, out var objectRange))
                {
                    foreach (PoolObject<T> internalObject in objectRange)
                    {
                        if (m_allowMultiGet || !internalObject.IsUsing)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// 获取对象。
            /// </summary>
            /// <returns>要获取的对象。</returns>
            public T Get()
            {
                return Get(string.Empty);
            }

            /// <summary>
            /// 获取对象。
            /// </summary>
            /// <param name="objName">对象名称。</param>
            /// <returns>要获取的对象。</returns>
            public T Get(string objName)
            {
                if (string.IsNullOrEmpty(objName))
                {
                    throw new QFrameworkException("Name is invalid.");
                }

                if (m_objects.TryGetValue(objName, out var objectRange))
                {
                    foreach (PoolObject<T> internalObject in objectRange)
                    {
                        if (m_allowMultiGet || !internalObject.IsUsing)
                        {
                            return internalObject.Get();
                        }
                    }
                }

                return null;
            }

            /// <summary>
            /// 回收对象。
            /// </summary>
            /// <param name="obj">要回收的对象。</param>
            public void Recycle(T obj)
            {
                if (obj == null)
                {
                    throw new QFrameworkException("Object is invalid.");
                }

                Recycle(obj.TargetObj);
            }

            /// <summary>
            /// 回收对象。
            /// </summary>
            /// <param name="targetObj">要回收的对象。</param>
            public void Recycle(object targetObj)
            {
                if (targetObj == null)
                {
                    throw new QFrameworkException("Target is invalid.");
                }

                PoolObject<T> internalObject = GetObject(targetObj);
                if (internalObject != null)
                {
                    internalObject.Recycle();
                    if (Count > m_capacity && internalObject.GetCount <= 0)
                    {
                        Release();
                    }
                }
                else
                {
                    throw new QFrameworkException(StringFormatHelper.Format(
                        "Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.", new TypeNamePair(typeof(T), PoolName),
                        targetObj.GetType().FullName, targetObj));
                }
            }

            private PoolObject<T> GetObject(object target)
            {
                if (target == null)
                {
                    throw new QFrameworkException("Target is invalid.");
                }

                return m_objectMap.GetValueOrDefault(target);
            }

            /// <summary>
            /// 设置对象是否被加锁。
            /// </summary>
            /// <param name="obj">要设置被加锁的对象。</param>
            /// <param name="locked">是否被加锁。</param>
            public void SetLocked(T obj, bool locked)
            {
                if (obj == null)
                {
                    throw new QFrameworkException("Object is invalid.");
                }

                SetLocked(obj.TargetObj, locked);
            }

            /// <summary>
            /// 设置对象是否被加锁。
            /// </summary>
            /// <param name="targetObj">要设置被加锁的对象。</param>
            /// <param name="locked">是否被加锁。</param>
            public void SetLocked(object targetObj, bool locked)
            {
                if (targetObj == null)
                {
                    throw new QFrameworkException("Target is invalid.");
                }

                PoolObject<T> internalObject = GetObject(targetObj);
                if (internalObject != null)
                {
                    internalObject.Locked = locked;
                }
                else
                {
                    throw new QFrameworkException(StringFormatHelper.Format(
                        "Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.", new TypeNamePair(typeof(T), PoolName),
                        targetObj.GetType().FullName, targetObj));
                }
            }

            /// <summary>
            /// 设置对象的优先级。
            /// </summary>
            /// <param name="obj">要设置优先级的对象。</param>
            /// <param name="priority">优先级。</param>
            public void SetPriority(T obj, int priority)
            {
                if (obj == null)
                {
                    throw new QFrameworkException("Object is invalid.");
                }

                SetPriority(obj.TargetObj, priority);
            }

            /// <summary>
            /// 设置对象的优先级。
            /// </summary>
            /// <param name="targetObj">要设置优先级的对象。</param>
            /// <param name="priority">优先级。</param>
            public void SetPriority(object targetObj, int priority)
            {
                if (targetObj == null)
                {
                    throw new QFrameworkException("Target is invalid.");
                }

                PoolObject<T> internalObject = GetObject(targetObj);
                if (internalObject != null)
                {
                    internalObject.Priority = priority;
                }
                else
                {
                    throw new QFrameworkException(StringFormatHelper.Format(
                        "Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.", new TypeNamePair(typeof(T), PoolName),
                        targetObj.GetType().FullName, targetObj));
                }
            }

            /// <summary>
            /// 释放对象。
            /// </summary>
            /// <param name="obj">要释放的对象。</param>
            /// <returns>释放对象是否成功。</returns>
            public bool ReleaseObject(T obj)
            {
                if (obj == null)
                {
                    throw new QFrameworkException("Object is invalid.");
                }

                return ReleaseObject(obj.TargetObj);
            }

            /// <summary>
            /// 释放对象。
            /// </summary>
            /// <param name="targetObj">要释放的对象。</param>
            /// <returns>释放对象是否成功。</returns>
            public bool ReleaseObject(object targetObj)
            {
                if (targetObj == null)
                {
                    throw new QFrameworkException("Target is invalid.");
                }

                PoolObject<T> internalObject = GetObject(targetObj);
                if (internalObject == null)
                {
                    return false;
                }

                if (internalObject.IsUsing || internalObject.Locked || !internalObject.CustomCanReleaseFlag)
                {
                    return false;
                }

                m_objects.Remove(internalObject.PoolObjName, internalObject);
                m_objectMap.Remove(internalObject.Peek().TargetObj);

                internalObject.Release(false);
                MemoryPoolMgr.Release(internalObject);
                return true;
            }

            /// <summary>
            /// 释放对象池中的可释放对象。
            /// </summary>
            public override void Release()
            {
                Release(Count - m_capacity, m_defaultReleaseObjectFilterCallback);
            }

            /// <summary>
            /// 释放对象池中的可释放对象。
            /// </summary>
            /// <param name="releaseObjectFilterCallback">释放对象筛选函数。</param>
            public void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                Release(Count - m_capacity, releaseObjectFilterCallback);
            }

            /// <summary>
            /// 释放对象池中的可释放对象。
            /// </summary>
            /// <param name="toReleaseCount">尝试释放对象数量。</param>
            public override void Release(int toReleaseCount)
            {
                Release(toReleaseCount, m_defaultReleaseObjectFilterCallback);
            }

            /// <summary>
            /// 释放对象池中的可释放对象。
            /// </summary>
            /// <param name="toReleaseCount">尝试释放对象数量。</param>
            /// <param name="releaseObjectFilterCallback">释放对象筛选函数。</param>
            public void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                if (releaseObjectFilterCallback == null)
                {
                    throw new QFrameworkException("Release object filter callback is invalid.");
                }

                if (toReleaseCount < 0)
                {
                    toReleaseCount = 0;
                }

                DateTime expireTime = DateTime.MinValue;
                if (m_expireTime < float.MaxValue)
                {
                    expireTime = DateTime.UtcNow.AddSeconds(-m_expireTime);
                }

                m_autoReleaseTime = 0f;
                GetCanReleaseObjects(m_cachedCanReleaseObjects);
                List<T> toReleaseObjects = releaseObjectFilterCallback(m_cachedCanReleaseObjects, toReleaseCount, expireTime);
                if (toReleaseObjects == null || toReleaseObjects.Count <= 0)
                {
                    return;
                }

                foreach (T toReleaseObject in toReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            /// <summary>
            /// 释放对象池中的所有未使用对象。
            /// </summary>
            public override void ReleaseAllUnused()
            {
                m_autoReleaseTime = 0f;
                GetCanReleaseObjects(m_cachedCanReleaseObjects);
                foreach (T toReleaseObject in m_cachedCanReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            /// <summary>
            /// 获取所有对象信息。
            /// </summary>
            /// <returns>所有对象信息。</returns>
            public override PoolObjectInfo[] GetAllObjectInfos()
            {
                List<PoolObjectInfo> results = new List<PoolObjectInfo>();
                foreach (KeyValuePair<string, QLinkedListRange<PoolObject<T>>> objectRanges in m_objects)
                {
                    foreach (PoolObject<T> internalObject in objectRanges.Value)
                    {
                        results.Add(new PoolObjectInfo(internalObject.PoolObjName, internalObject.Locked, internalObject.CustomCanReleaseFlag, internalObject.Priority,
                            internalObject.LastUseTime, internalObject.GetCount));
                    }
                }

                return results.ToArray();
            }

            public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                m_autoReleaseTime += realElapseSeconds;
                if (m_autoReleaseTime < m_autoReleaseInterval)
                {
                    return;
                }

                Release();
            }

            public override void OnRelease()
            {
                foreach (KeyValuePair<object, PoolObject<T>> objectInMap in m_objectMap)
                {
                    objectInMap.Value.Release(true);
                    MemoryPoolMgr.Release(objectInMap.Value);
                }

                m_objects.Clear();
                m_objectMap.Clear();
                m_cachedCanReleaseObjects.Clear();
                m_cachedToReleaseObjects.Clear();
            }

            private List<T> DefaultReleaseObjectFilterCallback(List<T> candidateObjects, int toReleaseCount, DateTime expireTime)
            {
                m_cachedToReleaseObjects.Clear();

                if (expireTime > DateTime.MinValue)
                {
                    for (int i = candidateObjects.Count - 1; i >= 0; i--)
                    {
                        if (candidateObjects[i].LastUseTime <= expireTime)
                        {
                            m_cachedToReleaseObjects.Add(candidateObjects[i]);
                            candidateObjects.RemoveAt(i);
                            continue;
                        }
                    }

                    toReleaseCount -= m_cachedToReleaseObjects.Count;
                }

                for (int i = 0; toReleaseCount > 0 && i < candidateObjects.Count; i++)
                {
                    for (int j = i + 1; j < candidateObjects.Count; j++)
                    {
                        if (candidateObjects[i].Priority > candidateObjects[j].Priority
                            || candidateObjects[i].Priority == candidateObjects[j].Priority &&
                            candidateObjects[i].LastUseTime > candidateObjects[j].LastUseTime)
                        {
                            T temp = candidateObjects[i];
                            candidateObjects[i] = candidateObjects[j];
                            candidateObjects[j] = temp;
                        }
                    }

                    m_cachedToReleaseObjects.Add(candidateObjects[i]);
                    toReleaseCount--;
                }

                return m_cachedToReleaseObjects;
            }
        }
    }
}