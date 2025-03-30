using System;

namespace QFramework
{
    /// <summary>
    /// 对象池接口。
    /// </summary>
    /// <typeparam name="T">对象类型。</typeparam>
    public interface IObjectPool<T> where T : BasePoolObject
    {
        /// <summary>
        /// 获取对象池名称。
        /// </summary>
        string PoolName { get; }

        /// <summary>
        /// 获取对象池完整名称。
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// 获取对象池对象类型。
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// 获取对象池中对象的数量。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 获取对象池中能被释放的对象的数量。
        /// </summary>
        int CanReleaseCount{ get; }

        /// <summary>
        /// 获取是否允许对象被多次获取。
        /// </summary>
        bool AllowMultiGet { get; }

        /// <summary>
        /// 获取或设置对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float AutoReleaseInterval { get; set; }

        /// <summary>
        /// 获取或设置对象池的容量。
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        /// 获取或设置对象池对象过期秒数。
        /// </summary>
        float ExpireTime { get; set; }

        /// <summary>
        /// 获取或设置对象池的优先级。
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// 创建对象。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="isGot">对象是否已被获取。</param>
        void Create(T obj, bool isGot);

        /// <summary>
        /// 检查对象。
        /// </summary>
        /// <returns>要检查的对象是否存在。</returns>
        bool CanGet();

        /// <summary>
        /// 检查对象。
        /// </summary>
        /// <param name="objName">对象名称。</param>
        /// <returns>要检查的对象是否存在。</returns>
        bool CanGet(string objName);

        /// <summary>
        /// 获取对象。
        /// </summary>
        /// <returns>要获取的对象。</returns>
        T Get();

        /// <summary>
        /// 获取对象。
        /// </summary>
        /// <param name="objName">对象名称。</param>
        /// <returns>要获取的对象。</returns>
        T Get(string objName);

        /// <summary>
        /// 回收对象。
        /// </summary>
        /// <param name="obj">要回收的对象。</param>
        void Recycle(T obj);

        /// <summary>
        /// 回收对象。
        /// </summary>
        /// <param name="targetObj">要回收的对象。</param>
        void Recycle(object targetObj);

        /// <summary>
        /// 设置对象是否被加锁。
        /// </summary>
        /// <param name="obj">要设置被加锁的对象。</param>
        /// <param name="locked">是否被加锁。</param>
        void SetLocked(T obj, bool locked);

        /// <summary>
        /// 设置对象是否被加锁。
        /// </summary>
        /// <param name="targetObj">要设置被加锁的对象。</param>
        /// <param name="locked">是否被加锁。</param>
        void SetLocked(object targetObj, bool locked);

        /// <summary>
        /// 设置对象的优先级。
        /// </summary>
        /// <param name="obj">要设置优先级的对象。</param>
        /// <param name="priority">优先级。</param>
        void SetPriority(T obj, int priority);

        /// <summary>
        /// 设置对象的优先级。
        /// </summary>
        /// <param name="targetObj">要设置优先级的对象。</param>
        /// <param name="priority">优先级。</param>
        void SetPriority(object targetObj, int priority);

        /// <summary>
        /// 释放对象。
        /// </summary>
        /// <param name="obj">要释放的对象。</param>
        /// <returns>释放对象是否成功。</returns>
        bool ReleaseObject(T obj);

        /// <summary>
        /// 释放对象。
        /// </summary>
        /// <param name="targetObj">要释放的对象。</param>
        /// <returns>释放对象是否成功。</returns>
        bool ReleaseObject(object targetObj);

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        void Release();

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        /// <param name="toReleaseCount">尝试释放对象数量。</param>
        void Release(int toReleaseCount);

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        /// <param name="releaseObjectFilterCallback">释放对象筛选函数。</param>
        void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        /// <param name="toReleaseCount">尝试释放对象数量。</param>
        /// <param name="releaseObjectFilterCallback">释放对象筛选函数。</param>
        void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);

        /// <summary>
        /// 释放对象池中的所有未使用对象。
        /// </summary>
        void ReleaseAllUnused();
    }
}