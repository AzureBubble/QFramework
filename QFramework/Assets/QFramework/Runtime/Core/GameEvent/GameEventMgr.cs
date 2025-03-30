using System;
using System.Collections.Generic;

namespace QFramework
{
    /// <summary>
    /// 游戏事件管理器。
    /// </summary>
    public class GameEventMgr : IMemory
    {
        private struct EventEntryMsg
        {
            public readonly int EventType;
            public readonly Delegate Handler;

            public EventEntryMsg(int eventType, Delegate handler)
            {
                EventType = eventType;
                Handler = handler;
            }
        }

        private readonly List<EventEntryMsg> m_eventMsgs;
        private bool m_isInit = false;

        public GameEventMgr()
        {
            if (!m_isInit)
            {
                m_isInit = true;
                m_eventMsgs = new List<EventEntryMsg>();
            }
        }

        private void AddEventImp(int eventType, Delegate handler)
        {
            m_eventMsgs.Add(new EventEntryMsg(eventType, handler));
        }

        #region UIEvent

        public void AddUIEvent(int eventType, Action handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddUIEvent<T>(int eventType, Action<T> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddUIEvent<T1, T2>(int eventType, Action<T1, T2> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddUIEvent<T1, T2, T3>(int eventType, Action<T1, T2, T3> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddUIEvent<T1, T2, T3, T4>(int eventType, Action<T1, T2, T3, T4> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddUIEvent<T1, T2, T3, T4, T5>(int eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        public void AddUIEvent<T1, T2, T3, T4, T5, T6>(int eventType, Action<T1, T2, T3, T4, T5, T6> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }

        #endregion

        /// <summary>
        /// 释放GameEventMgr
        /// </summary>
        public void OnRelease()
        {
            if (m_isInit)
            {
                for (int i = 0; i < m_eventMsgs.Count; i++)
                {
                    GameEvent.RemoveEventListener(m_eventMsgs[i].EventType, m_eventMsgs[i].Handler);
                }
                m_eventMsgs.Clear();
                m_isInit = false;
            }
        }
    }
}