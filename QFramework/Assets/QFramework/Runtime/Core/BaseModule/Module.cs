using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// 游戏框架模块抽象类。
    /// <remarks>实现游戏框架具体逻辑。</remarks>
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        public virtual int Priority => 0;

        /// <summary>
        /// 初始化游戏框架接口。
        /// </summary>
        public abstract void OnInit();

        /// <summary>
        /// 关闭并清理游戏框架模块。
        /// </summary>
        public abstract void OnDispose();
    }

    public interface IUpdateModule
    {
        /// <summary>
        /// 游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        void OnUpdate(float elapseSeconds, float realElapseSeconds);
    }

    public interface IModuleInstance<T> where T : Module
    {
        public T Instance { get; }
    }
}