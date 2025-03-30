using System;
using System.Collections.Generic;

namespace QFramework
{
    public delegate void GameTimerHandler(object[] args);

    [Serializable]
    public class GameTimer
    {
        public int TimerId = 0;
        public float Timer = 0;
        public float Interval = 0;
        public float TriggerTime = 0;
        public GameTimerHandler Handler = null;
        public bool IsLoop = false;
        public bool IsNeedRemove = false;
        public bool IsRunning = false;
        public bool IsUnscale = false; //是否使用非缩放的时间
        public object[] Args = null; //回调参数
    }

    public class GameTimerModule : Module, IGameTimerModule, IUpdateModule
    {
        private int m_curTimerId = 0;
        private readonly QLinkedList<GameTimer> m_gameTimers = new QLinkedList<GameTimer>();
        private readonly QLinkedList<GameTimer> m_unscaleGameTimers = new QLinkedList<GameTimer>();

        public override void OnInit()
        {
        }

        #region UpdateTimer

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            UpdateGameTimer(elapseSeconds);
            UpdateUnscaleGameTimer(realElapseSeconds);
        }

        private void UpdateGameTimer(float elapseSeconds)
        {
            bool isLoopCall = false;

            var curNode = m_gameTimers.First;
            var nowTime = GameTime.Time;

            while (curNode != null)
            {
                var nextNode = curNode.Next;

                if (curNode.Value.IsNeedRemove)
                {
                    m_gameTimers.Remove(curNode);
                    curNode = nextNode;
                    continue;
                }

                if (!curNode.Value.IsRunning)
                {
                    curNode = nextNode;
                    continue;
                }

                // curNode.Value.Timer -= elapseSeconds;
                if (curNode.Value.TriggerTime <= nowTime) // curNode.Value.Timer <= 0
                {
                    curNode.Value.Handler?.Invoke(curNode.Value.Args);

                    if (curNode.Value.IsLoop)
                    {
                        // curNode.Value.Timer += curNode.Value.Interval;
                        curNode.Value.TriggerTime = nowTime + curNode.Value.Interval;
                        if (curNode.Value.TriggerTime <= nowTime) // curNode.Value.Timer <= 0 ||
                        {
                            isLoopCall = true;
                        }
                    }
                    else
                    {
                        m_gameTimers.Remove(curNode);
                    }
                }

                curNode = nextNode;
                if (curNode == m_gameTimers.First)
                {
                    return;
                }
            }

            if (isLoopCall)
            {
                LoopCallInBadFrame();
            }
        }

        private void LoopCallInBadFrame()
        {
            bool isLoopCall = false;
            var nowTime = GameTime.Time;

            foreach (var gameTimer in m_gameTimers)
            {
                if (gameTimer.IsLoop && gameTimer.TriggerTime <= nowTime) // gameTimer.Timer <= 0
                {
                    gameTimer.Handler?.Invoke(gameTimer.Args);

                    // gameTimer.Timer += gameTimer.Interval;
                    gameTimer.TriggerTime = nowTime + gameTimer.Interval;
                    if (gameTimer.TriggerTime <= nowTime)//gameTimer.Timer <= 0
                    {
                        isLoopCall = true;
                    }
                }
            }

            if (isLoopCall)
            {
                LoopCallInBadFrame();
            }
        }

        private void UpdateUnscaleGameTimer(float realElapseSeconds)
        {
            bool isLoopCall = false;

            var curNode = m_unscaleGameTimers.First;
            var nowTime = GameTime.UnscaledTime;

            while (curNode != null)
            {
                var nextNode = curNode.Next;

                if (curNode.Value.IsNeedRemove)
                {
                    m_unscaleGameTimers.Remove(curNode);
                    curNode = nextNode;
                    continue;
                }

                if (!curNode.Value.IsRunning)
                {
                    curNode = nextNode;
                    continue;
                }

                // curNode.Value.Timer -= realElapseSeconds;

                if (curNode.Value.TriggerTime <= nowTime) //curNode.Value.Timer <= 0
                {
                    curNode.Value.Handler?.Invoke(curNode.Value.Args);

                    if (curNode.Value.IsLoop)
                    {
                        // curNode.Value.Timer += curNode.Value.Interval;
                        curNode.Value.TriggerTime = nowTime + curNode.Value.Interval;
                        if (curNode.Value.TriggerTime <= nowTime) // curNode.Value.Timer <= 0
                        {
                            isLoopCall = true;
                        }
                    }
                    else
                    {
                        m_unscaleGameTimers.Remove(curNode);
                    }
                }
                curNode = nextNode;
                if (curNode == m_unscaleGameTimers.First)
                {
                    return;
                }
            }

            if (isLoopCall)
            {
                LoopCallUnscaledInBadFrame();
            }
        }

        private void LoopCallUnscaledInBadFrame()
        {
            bool isLoopCall = false;
            var nowTime = GameTime.UnscaledTime;

            foreach (var gameTimer in m_unscaleGameTimers)
            {
                if (gameTimer.IsLoop && gameTimer.TriggerTime <= nowTime) // gameTimer.Timer <= 0
                {
                    gameTimer.Handler?.Invoke(gameTimer.Args);

                    // gameTimer.Timer += gameTimer.Interval;
                    gameTimer.TriggerTime = nowTime + gameTimer.Interval;
                    if (gameTimer.TriggerTime <= nowTime) //gameTimer.Timer <= 0
                    {
                        isLoopCall = true;
                    }
                }
            }

            if (isLoopCall)
            {
                LoopCallUnscaledInBadFrame();
            }
        }

        #endregion

        public override void OnDispose()
        {
            DestroyAllGameTimer();
            DestroySystemTimer();
        }

        #region 创建计时器

        private void InsertGameTimer(GameTimer gameTimer)
        {
            if (gameTimer.IsUnscale)
            {
                if (m_unscaleGameTimers.Count <= 0)
                {
                    m_unscaleGameTimers.AddLast(gameTimer);
                    return;
                }

                var curNode = m_unscaleGameTimers.First;

                while (curNode != null && curNode.Value.TriggerTime <= gameTimer.TriggerTime)
                {
                    curNode = curNode.Next;
                }

                if (curNode == null)
                {
                    m_unscaleGameTimers.AddLast(gameTimer);
                }
                else
                {
                    m_unscaleGameTimers.AddBefore(curNode, gameTimer);
                }
            }
            else
            {
                if (m_gameTimers.Count <= 0)
                {
                    m_gameTimers.AddLast(gameTimer);
                    return;
                }

                var curNode = m_gameTimers.First;

                while (curNode != null && curNode.Value.TriggerTime <= gameTimer.TriggerTime)
                {
                    curNode = curNode.Next;
                }

                if (curNode == null)
                {
                    m_gameTimers.AddLast(gameTimer);
                }
                else
                {
                    m_gameTimers.AddBefore(curNode, gameTimer);
                }
            }
        }

        public GameTimer CreatUnscaleLoopGameTimer(float interval, GameTimerHandler callback, params object[] args)
        {
            return CreateGameTimer(interval, true, true, callback, args);
        }

        public GameTimer CreatLoopGameTimer(float interval, GameTimerHandler callback, params object[] args)
        {
            return CreateGameTimer(interval, true, false, callback, args);
        }

        public GameTimer CreatOnceGameTimer(float interval, GameTimerHandler callback, params object[] args)
        {
            return CreateGameTimer(interval, false, false, callback, args);
        }

        public GameTimer CreatUnscaleOnceGameTimer(float interval, GameTimerHandler callback, params object[] args)
        {
            return CreateGameTimer(interval, false, true, callback, args);
        }

        public GameTimer CreatGameTimer(float interval, bool isLoop, GameTimerHandler callback, params object[] args)
        {
            return CreateGameTimer(interval, isLoop, false, callback, args);
        }

        public GameTimer CreateUnscaleGameTimer(float interval, bool isLoop, GameTimerHandler callback, params object[] args)
        {
            return CreateGameTimer(interval, isLoop, true, callback, args);
        }

        public GameTimer CreateGameTimer(float interval, bool isLoop, bool isUnscaled, GameTimerHandler callback, params object[] args)
        {
            GameTimer timer = new GameTimer
            {
                TimerId = ++m_curTimerId,
                Timer = interval,
                Interval = interval,
                Handler = callback,
                IsLoop = isLoop,
                IsUnscale = isUnscaled,
                Args = args,
                IsNeedRemove = false,
                IsRunning = true
            };

            if (isUnscaled)
            {
                timer.TriggerTime = GameTime.UnscaledTime + interval;
            }
            else
            {
                timer.TriggerTime = GameTime.Time + interval;
            }

            InsertGameTimer(timer);
            return timer;
        }

        #endregion

        /// <summary>
        /// 暂停计时器。
        /// </summary>
        /// <param name="timer">计时器。</param>
        public void Pause(GameTimer timer)
        {
            if (timer != null)
            {
                timer.IsRunning = false;
            }
        }

        public void Resume(GameTimer timer)
        {
            if (timer != null)
            {
                timer.IsRunning = true;
            }
        }

        public bool IsRunning(GameTimer timer)
        {
            return timer != null && timer.IsRunning;
        }

        public float GetLeftTime(GameTimer timer)
        {
            if (timer == null)
            {
                return 0;
            }
            return timer.Timer;
        }

        public void Restart(GameTimer timer)
        {
            if (timer != null)
            {
                timer.Timer = timer.Interval;
                timer.IsRunning = true;
            }
        }

        public void ResetGameTimer(GameTimer timer, float interval, bool isLoop, bool isUnscaled, GameTimerHandler callback)
        {
            if (timer != null)
            {
                timer.Timer = interval;
                timer.Interval = interval;
                timer.IsLoop = isLoop;
                timer.IsNeedRemove = false;
                timer.Handler = callback;
                if (timer.IsUnscale != isUnscaled)
                {
                    DestroyGameTimerImmediate(timer);
                    timer.IsUnscale = isUnscaled;
                    InsertGameTimer(timer);
                }
            }
        }

        /// <summary>
        /// 重置计时器。
        /// </summary>
        public void ResetGameTimer(GameTimer timer, float interval, bool isLoop, bool isUnscaled)
        {
            if (timer != null)
            {
                timer.Timer = interval;
                timer.Interval = interval;
                timer.IsLoop = isLoop;
                timer.IsNeedRemove = false;
                if (timer.IsUnscale != isUnscaled)
                {
                    DestroyGameTimerImmediate(timer);
                    timer.IsUnscale = isUnscaled;
                    InsertGameTimer(timer);
                }
            }
        }

        /// <summary>
        /// 立即移除。
        /// </summary>
        /// <param name="timer"></param>
        private void DestroyGameTimerImmediate(GameTimer timer)
        {
            if (timer != null)
            {
                if (timer.IsUnscale && m_unscaleGameTimers.Contains(timer))
                {
                    m_unscaleGameTimers.Remove(timer);
                }

                if(!timer.IsUnscale && m_gameTimers.Contains(timer))
                {
                    m_gameTimers.Remove(timer);
                }
            }
        }

        public void DestroyGameTimer(GameTimer timer)
        {
            if (timer != null)
            {
                timer.IsNeedRemove = true;
            }
        }

        public void DestroyAllGameTimer()
        {
            m_gameTimers.Clear();
            m_gameTimers.ClearCachedNodes();
            m_unscaleGameTimers.Clear();
            m_unscaleGameTimers.ClearCachedNodes();
        }

        #region SystemTimer

        private readonly List<System.Timers.Timer> m_ticker = new List<System.Timers.Timer>();

        /// <summary>
        /// 创建一个一秒触发一次的计时器
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public System.Timers.Timer CreateSystemTimer(Action<object, System.Timers.ElapsedEventArgs> callBack)
        {
            int interval = 1000;
            var timerTick = new System.Timers.Timer(interval);
            timerTick.AutoReset = true;
            timerTick.Enabled = true;
            timerTick.Elapsed += new System.Timers.ElapsedEventHandler(callBack);

            m_ticker.Add(timerTick);

            return timerTick;
        }

        private void DestroySystemTimer()
        {
            foreach (var ticker in m_ticker)
            {
                if (ticker != null)
                {
                    ticker.Stop();
                }
            }
        }

        #endregion
    }
}