using System;

namespace QFramework
{
    public class GameEvent
    {
        private static readonly EventMgr m_eventMgr = new EventMgr();

        /// <summary>
        /// 全局事件管理器。
        /// </summary>
        public static EventMgr EventMgr => m_eventMgr;

        public static T Get<T>()
        {
            return m_eventMgr.GetInterface<T>();
        }

        #region AddEventListener

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件Handler。</param>
        /// <returns>是否监听成功。</returns>
        public static bool AddEventListener(int eventType, Delegate handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件Handler。</param>
        /// <returns>是否监听成功。</returns>
        public static bool AddEventListener(string eventType, Delegate handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件Handler。</param>
        /// <returns>是否监听成功。</returns>
        public static bool AddEventListener(int eventType, Action handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1>(int eventType, Action<T1> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2>(int eventType, Action<T1, T2> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2, T3>(int eventType, Action<T1, T2, T3> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2, T3, T4>(int eventType, Action<T1, T2, T3, T4> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2, T3, T4, T5>(int eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        /// <typeparam name="T6">事件参数6类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2, T3, T4, T5, T6>(int eventType, Action<T1, T2, T3, T4, T5, T6> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <returns></returns>
        public static bool AddEventListener(string eventType, Action handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1>(string eventType, Action<T1> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2>(string eventType, Action<T1, T2> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2, T3, T4>(string eventType, Action<T1, T2, T3, T4> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2, T3, T4, T5>(string eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 增加事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        /// <typeparam name="T6">事件参数6类型。</typeparam>
        /// <returns></returns>
        public static bool AddEventListener<T1, T2, T3, T4, T5, T6>(string eventType, Action<T1, T2, T3, T4, T5, T6> handler)
        {
            return m_eventMgr.Dispatcher.AddEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        #endregion

        #region RemoveEventListener

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void RemoveEventListener(int eventType, Delegate handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void RemoveEventListener(string eventType, Delegate handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void RemoveEventListener(int eventType, Action handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        public static void RemoveEventListener<T1>(int eventType, Action<T1> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        public static void RemoveEventListener<T1, T2>(int eventType, Action<T1, T2> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        public static void RemoveEventListener<T1, T2, T3>(int eventType, Action<T1, T2, T3> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        public static void RemoveEventListener<T1, T2, T3, T4>(int eventType, Action<T1, T2, T3, T4> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        public static void RemoveEventListener<T1, T2, T3, T4, T5>(int eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        /// <typeparam name="T6">事件参数6类型。</typeparam>
        public static void RemoveEventListener<T1, T2, T3, T4, T5, T6>(int eventType, Action<T1, T2, T3, T4, T5, T6> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        public static void RemoveEventListener(string eventType, Action handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        public static void RemoveEventListener<T1>(string eventType, Action<T1> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        public static void RemoveEventListener<T1, T2>(string eventType, Action<T1, T2> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        public static void RemoveEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        public static void RemoveEventListener<T1, T2, T3, T4>(string eventType, Action<T1, T2, T3, T4> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        public static void RemoveEventListener<T1, T2, T3, T4, T5>(string eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        /// <summary>
        /// 移除事件监听。
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="handler">事件处理回调。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        /// <typeparam name="T5">事件参数5类型。</typeparam>
        /// <typeparam name="T6">事件参数6类型。</typeparam>
        public static void RemoveEventListener<T1, T2, T3, T4, T5, T6>(string eventType, Action<T1, T2, T3, T4, T5, T6> handler)
        {
            m_eventMgr.Dispatcher.RemoveEventListener(GameEventUtil.ToStringId(eventType), handler);
        }

        #endregion

        #region 分发消息接口

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        public static void TriggerEvent<T1>(int eventType)
        {
            m_eventMgr.Dispatcher.TriggerEvent(eventType);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="t1">事件参数1。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        public static void TriggerEvent<T1>(int eventType, T1 t1)
        {
            m_eventMgr.Dispatcher.TriggerEvent(eventType, t1);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        public static void TriggerEvent<T1, T2>(int eventType, T1 t1, T2 t2)
        {
            m_eventMgr.Dispatcher.TriggerEvent(eventType, t1, t2);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <param name="t3">事件参数3。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        public static void TriggerEvent<T1, T2, T3>(int eventType, T1 t1, T2 t2, T3 t3)
        {
            m_eventMgr.Dispatcher.TriggerEvent(eventType, t1, t2, t3);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <param name="t3">事件参数3。</param>
        /// <param name="t4">事件参数4。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        public static void TriggerEvent<T1, T2, T3, T4>(int eventType, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            m_eventMgr.Dispatcher.TriggerEvent(eventType, t1, t2, t3, t4);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
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
        public static void TriggerEvent<T1, T2, T3, T4, T5>(int eventType, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            m_eventMgr.Dispatcher.TriggerEvent(eventType, t1, t2, t3, t4, t5);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
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
        public static void TriggerEvent<T1, T2, T3, T4, T5, T6>(int eventType, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            m_eventMgr.Dispatcher.TriggerEvent(eventType, t1, t2, t3, t4, t5, t6);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        public static void TriggerEvent<T1>(string eventType)
        {
            m_eventMgr.Dispatcher.TriggerEvent(GameEventUtil.ToStringId(eventType));
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="t1">事件参数1。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        public static void TriggerEvent<T1>(string eventType, T1 t1)
        {
            m_eventMgr.Dispatcher.TriggerEvent(GameEventUtil.ToStringId(eventType), t1);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        public static void TriggerEvent<T1, T2>(string eventType, T1 t1, T2 t2)
        {
            m_eventMgr.Dispatcher.TriggerEvent(GameEventUtil.ToStringId(eventType), t1, t2);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <param name="t3">事件参数3。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        public static void TriggerEvent<T1, T2, T3>(string eventType, T1 t1, T2 t2, T3 t3)
        {
            m_eventMgr.Dispatcher.TriggerEvent(GameEventUtil.ToStringId(eventType), t1, t2, t3);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
        /// <param name="t1">事件参数1。</param>
        /// <param name="t2">事件参数2。</param>
        /// <param name="t3">事件参数3。</param>
        /// <param name="t4">事件参数4。</param>
        /// <typeparam name="T1">事件参数1类型。</typeparam>
        /// <typeparam name="T2">事件参数2类型。</typeparam>
        /// <typeparam name="T3">事件参数3类型。</typeparam>
        /// <typeparam name="T4">事件参数4类型。</typeparam>
        public static void TriggerEvent<T1, T2, T3, T4>(string eventType, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            m_eventMgr.Dispatcher.TriggerEvent(GameEventUtil.ToStringId(eventType), t1, t2, t3, t4);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
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
        public static void TriggerEvent<T1, T2, T3, T4, T5>(string eventType, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            m_eventMgr.Dispatcher.TriggerEvent(GameEventUtil.ToStringId(eventType), t1, t2, t3, t4, t5);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventType">事件类型。</param>
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
        public static void TriggerEvent<T1, T2, T3, T4, T5, T6>(string eventType, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            m_eventMgr.Dispatcher.TriggerEvent(GameEventUtil.ToStringId(eventType), t1, t2, t3, t4, t5, t6);
        }

        #endregion


        /// <summary>
        /// 清除事件。
        /// </summary>
        public static void Dispose()
        {
            m_eventMgr.Dispose();
        }
    }
}