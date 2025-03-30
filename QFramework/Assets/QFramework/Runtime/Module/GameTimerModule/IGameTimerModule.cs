using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IGameTimerModule
    {
        /// <summary>
        /// 创建受时间缩放影响的Loop计时器。
        /// </summary>
        /// <param name="callback">计时器回调。</param>
        /// <param name="interval">计时器间隔。</param>
        /// <param name="args">传参。(避免闭包)</param>
        /// <returns>计时器Id。</returns>
        GameTimer CreatUnscaleLoopGameTimer(float interval, GameTimerHandler callback, params object[] args);

        /// <summary>
        /// 创建不受时间缩放影响的Loop计时器。
        /// </summary>
        /// <param name="callback">计时器回调。</param>
        /// <param name="interval">计时器间隔。</param>
        /// <param name="args">传参。(避免闭包)</param>
        /// <returns>计时器Id。</returns>
        GameTimer CreatLoopGameTimer(float interval, GameTimerHandler callback, params object[] args);

        /// <summary>
        /// 创建不受时间缩放影响的Loop计时器。
        /// </summary>
        /// <param name="callback">计时器回调。</param>
        /// <param name="interval">计时器间隔。</param>
        /// <param name="args">传参。(避免闭包)</param>
        /// <returns>计时器Id。</returns>
        GameTimer CreatOnceGameTimer(float interval, GameTimerHandler callback, params object[] args);

        /// <summary>
        /// 创建不受时间缩放影响的Loop计时器。
        /// </summary>
        /// <param name="callback">计时器回调。</param>
        /// <param name="interval">计时器间隔。</param>
        /// <param name="args">传参。(避免闭包)</param>
        /// <returns>计时器Id。</returns>
        GameTimer CreatUnscaleOnceGameTimer(float interval, GameTimerHandler callback, params object[] args);

        /// <summary>
        /// 创建不受时间缩放影响计时器。
        /// </summary>
        /// <param name="callback">计时器回调。</param>
        /// <param name="interval">计时器间隔。</param>
        /// <param name="isLoop">是否循环。</param>
        /// <param name="args">传参。(避免闭包)</param>
        /// <returns>计时器Id。</returns>
        GameTimer CreatGameTimer(float interval, bool isLoop, GameTimerHandler callback, params object[] args);

        /// <summary>
        /// 创建受时间缩放影响计时器。
        /// </summary>
        /// <param name="callback">计时器回调。</param>
        /// <param name="interval">计时器间隔。</param>
        /// <param name="isLoop">是否循环。</param>
        /// <param name="args">传参。(避免闭包)</param>
        /// <returns>计时器Id。</returns>
        GameTimer CreateUnscaleGameTimer(float interval, bool isLoop, GameTimerHandler callback, params object[] args);

        /// <summary>
        /// 创建计时器。
        /// </summary>
        /// <param name="callback">计时器回调。</param>
        /// <param name="interval">计时器间隔。</param>
        /// <param name="isLoop">是否循环。</param>
        /// <param name="isUnscaled">是否不收时间缩放影响。</param>
        /// <param name="args">传参。(避免闭包)</param>
        /// <returns>计时器Id。</returns>
        GameTimer CreateGameTimer(float interval, bool isLoop, bool isUnscaled, GameTimerHandler callback, params object[] args);

        /// <summary>
        /// 暂停计时器。
        /// </summary>
        /// <param name="timer">计时器。</param>
        public void Pause(GameTimer timer);

        /// <summary>
        /// 恢复计时器。
        /// </summary>
        /// <param name="timer">计时器。</param>
        public void Resume(GameTimer timer);

        /// <summary>
        /// 计时器是否在运行中。
        /// </summary>
        /// <param name="timer">计时器。</param>
        /// <returns>否在运行中。</returns>
        public bool IsRunning(GameTimer timer);

        /// <summary>
        /// 获得计时器剩余时间。
        /// </summary>
        public float GetLeftTime(GameTimer timer);

        /// <summary>
        /// 重置计时器,恢复到开始状态。
        /// </summary>
        public void Restart(GameTimer timer);

        /// <summary>
        /// 重置计时器。
        /// </summary>
        public void ResetGameTimer(GameTimer timer, float interval, bool isLoop, bool isUnscaled, GameTimerHandler callback);

        /// <summary>
        /// 重置计时器。
        /// </summary>
        public void ResetGameTimer(GameTimer timer, float interval, bool isLoop, bool isUnscaled);

        /// <summary>
        /// 移除计时器。
        /// </summary>
        /// <param name="timer">计时器Id。</param>
        public void DestroyGameTimer(GameTimer timer);

        /// <summary>
        /// 移除所有计时器。
        /// </summary>
        public void DestroyAllGameTimer();


        System.Timers.Timer CreateSystemTimer(Action<object, System.Timers.ElapsedEventArgs> callBack);
    }
}