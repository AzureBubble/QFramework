﻿using System;

namespace QFramework
{
    /// <summary>
    /// 对象池基类
    /// </summary>
    public abstract class BaseObjectPool
    {
        /// <summary>
        /// 对象池名称
        /// </summary>
        public string PoolName { get; }

        public string FullName => new TypeNamePair(ObjectType, PoolName).ToString();

        /// <summary>
        /// 获取对象池对象类型。
        /// </summary>
        public abstract Type ObjectType { get; }

        /// <summary>
        /// 对象池对象数量
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// 获取对象池中能被释放的对象的数量。
        /// </summary>
        public abstract int CanReleaseCount { get; }

        /// <summary>
        /// 获取是否允许对象被多次获取。
        /// </summary>
        public abstract bool AllowMultiGet { get; }

        /// <summary>
        /// 获取或设置对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public abstract float AutoReleaseInterval { get; set; }

        /// <summary>
        /// 获取或设置对象池的容量。
        /// </summary>
        public abstract int Capacity { get; set; }

        /// <summary>
        /// 获取或设置对象池对象过期秒数。
        /// </summary>
        public abstract float ExpireTime { get; set; }

        /// <summary>
        /// 获取或设置对象池的优先级。
        /// </summary>
        public abstract int Priority { get; set; }

        public BaseObjectPool() : this(null)
        {

        }

        public BaseObjectPool(string name)
        {
            PoolName = name == null ? string.Empty : name;
        }

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        /// <param name="toReleaseCount">尝试释放对象数量。</param>
        public abstract void Release(int toReleaseCount);

        /// <summary>
        /// 获取所有对象信息。
        /// </summary>
        /// <returns>所有对象信息。</returns>
        public abstract PoolObjectInfo[] GetAllObjectInfos();

        /// <summary>
        /// 释放对象池中的所有未使用对象。
        /// </summary>
        public abstract void ReleaseAllUnused();

        public abstract void OnUpdate(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 对象池被释放
        /// </summary>
        public abstract void OnRelease();
    }
}