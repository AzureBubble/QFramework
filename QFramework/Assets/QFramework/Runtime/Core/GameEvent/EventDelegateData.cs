using System;
using System.Collections.Generic;

namespace QFramework
{
    internal class EventDelegateData
    {
        private readonly int m_eventType;
        private readonly List<Delegate> m_existDelegates = new List<Delegate>();
        private readonly List<Delegate> m_addDelegates = new List<Delegate>();
        private readonly List<Delegate> m_delDelegates = new List<Delegate>();
        private bool m_isExecute = false;
        private bool m_isDirty = false;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        internal EventDelegateData(int eventType)
        {
            m_eventType = eventType;
        }

        /// <summary>
        /// 注册委托监听。
        /// </summary>
        /// <param name="handler">事件处理回调。</param>
        /// <returns>是否添加回调成功。</returns>
        internal bool AddListener(Delegate handler)
        {
            if (m_existDelegates.Contains(handler))
            {
                Debugger.Fatal("Repeated Add Handler");
                return false;
            }

            if (m_isExecute)
            {
                m_isDirty = true;
                m_addDelegates.Add(handler);
            }
            else
            {
                m_existDelegates.Add(handler);
            }
            return true;
        }

        /// <summary>
        /// 移除反注册委托。
        /// </summary>
        /// <param name="handler">事件处理回调。</param>
        internal void RemoveListener(Delegate handler)
        {
            if (m_existDelegates.Contains(handler))
            {
                Debugger.Fatal("Repeated Add Handler");
            }

            if (m_isExecute)
            {
                m_isDirty = true;
                m_delDelegates.Add(handler);
            }
            else
            {
                if (!m_existDelegates.Remove(handler))
                {
                    Debugger.Fatal("Delete handle failed, not exist, EventId: {0}", GameEventUtil.ToString(m_eventType));
                }
            }
        }

        /// <summary>
        /// 检测脏数据修正。
        /// </summary>
        private void CheckModify()
        {
            if (m_isDirty)
            {
                for (int i = 0; i < m_addDelegates.Count; i++)
                {
                    m_existDelegates.Add(m_addDelegates[i]);
                }

                m_addDelegates.Clear();

                for (int i = 0; i < m_delDelegates.Count; i++)
                {
                    m_existDelegates.Remove(m_delDelegates[i]);
                }

                m_delDelegates.Clear();
            }
            m_isExecute = false;
        }

        #region TriggerEvent

        /// <summary>
        /// 回调调用。
        /// </summary>
        public void TriggerEvent()
        {
            m_isExecute = true;

            foreach (var delegateItem in m_existDelegates)
            {
                (delegateItem as Action)?.Invoke();
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="t1">事件参数1。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        public void TriggerEvent<T1>(T1 t1)
        {
            m_isExecute = true;

            foreach (var delegateItem in m_existDelegates)
            {
                (delegateItem as Action<T1>)?.Invoke(t1);
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        public void TriggerEvent<T1, T2>(T1 t1, T2 t2)
        {
            m_isExecute = true;

            foreach (var delegateItem in m_existDelegates)
            {
                (delegateItem as Action<T1, T2>)?.Invoke(t1, t2);
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <param name="t3">事件参数3。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        public void TriggerEvent<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
        {
            m_isExecute = true;

            foreach (var delegateItem in m_existDelegates)
            {
                (delegateItem as Action<T1, T2, T3>)?.Invoke(t1, t2, t3);
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <param name="t3">事件参数3。</param>
        /// <param name="t4">事件参数4。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        public void TriggerEvent<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            m_isExecute = true;

            foreach (var delegateItem in m_existDelegates)
            {
                (delegateItem as Action<T1, T2, T3, T4>)?.Invoke(t1, t2, t3, t4);
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <param name="t3">事件参数3。</param>
        /// <param name="t4">事件参数4。</param>
        /// <param name="t5">事件参数5。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        public void TriggerEvent<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            m_isExecute = true;

            foreach (var delegateItem in m_existDelegates)
            {
                (delegateItem as Action<T1, T2, T3, T4, T5>)?.Invoke(t1, t2, t3, t4, t5);
            }

            CheckModify();
        }

        /// <summary>
        /// 回调调用。
        /// </summary>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <param name="t3">事件参数3。</param>
        /// <param name="t4">事件参数4。</param>
        /// <param name="t5">事件参数5。</param>
        /// <param name="t6">事件参数6。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        /// <typeparam name="T6">事件参数6类型。</typeparam>
        public void TriggerEvent<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            m_isExecute = true;

            foreach (var delegateItem in m_existDelegates)
            {
                (delegateItem as Action<T1, T2, T3, T4, T5, T6>)?.Invoke(t1, t2, t3, t4, t5, t6);
            }

            CheckModify();
        }

        #endregion
    }
}