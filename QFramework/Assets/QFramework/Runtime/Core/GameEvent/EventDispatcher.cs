using System;
using System.Collections.Generic;

namespace QFramework
{
    /// <summary>
    /// 封装消息的底层分发和注册。
    /// </summary>
    public class EventDispatcher
    {
        private static readonly Dictionary<int, EventDelegateData> m_eventDict = new Dictionary<int, EventDelegateData>();

        #region 事件监听与移除

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理委托。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddEventListener(int eventType, Delegate handler)
        {
            if (!m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData = new EventDelegateData(eventType);
                m_eventDict.Add(eventType, delegateData);
            }

            return delegateData.AddListener(handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理委托。</param>
        public void RemoveEventListener(int eventType, Delegate handler)
        {
            if (m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData.RemoveListener(handler);
            }
        }

        #endregion

        #region 事件触发

        public void TriggerEvent(int eventType)
        {
            if (m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData.TriggerEvent();
            }
        }

        public void TriggerEvent<T>(int eventType, T t1)
        {
            if (m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData.TriggerEvent(t1);
            }
        }

        public void TriggerEvent<T1, T2>(int eventType, T1 t1, T2 t2)
        {
            if (m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData.TriggerEvent(t1, t2);
            }
        }

        public void TriggerEvent<T1, T2, T3>(int eventType, T1 t1, T2 t2, T3 t3)
        {
            if (m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData.TriggerEvent(t1, t2, t3);
            }
        }

        public void TriggerEvent<T1, T2, T3, T4>(int eventType, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData.TriggerEvent(t1, t2, t3, t4);
            }
        }

        public void TriggerEvent<T1, T2, T3, T4, T5>(int eventType, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData.TriggerEvent(t1, t2, t3, t4, t5);
            }
        }

        public void TriggerEvent<T1, T2, T3, T4, T5, T6>(int eventType, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            if (m_eventDict.TryGetValue(eventType, out var delegateData))
            {
                delegateData.TriggerEvent(t1, t2, t3, t4, t5, t6);
            }
        }

        #endregion

        public void Clear()
        {
            m_eventDict.Clear();
        }
    }
}