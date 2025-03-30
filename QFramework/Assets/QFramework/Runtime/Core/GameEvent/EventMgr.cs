using System.Collections.Generic;

namespace QFramework
{
    /// <summary>
    /// 事件管理器。
    /// </summary>
    public class EventMgr
    {
        private class EventEntryData
        {
            public readonly object InterfaceWrap;

            public EventEntryData(object interfaceWrap)
            {
                InterfaceWrap = interfaceWrap;
            }
        }

        /// <summary>
        /// 分发注册器。
        /// </summary>
        public EventDispatcher Dispatcher => new EventDispatcher();

        private readonly Dictionary<string, EventEntryData> m_eventEntryMap = new Dictionary<string, EventEntryData>();

        /// <summary>
        /// 事件管理器获取接口。
        /// </summary>
        /// <typeparam name="T">接口类型。</typeparam>
        /// <returns>接口实例。</returns>
        public T GetInterface<T>()
        {
            string typeName = typeof(T).FullName;

            if (!string.IsNullOrEmpty(typeName) && m_eventEntryMap.TryGetValue(typeName, out var entryData))
            {
                return (T)entryData.InterfaceWrap;
            }
            return default(T);
        }

        /// <summary>
        /// 注册wrap的函数。
        /// </summary>
        /// <typeparam name="T">Wrap接口类型。</typeparam>
        /// <param name="callerWrap">callerWrap接口名字。</param>
        public void RegWrapInterface<T>(T callerWrap)
        {
            string typeName = typeof(T).FullName;
            RegWrapInterface(typeName, callerWrap);
        }

        /// <summary>
        /// 注册wrap的函数。
        /// </summary>
        /// <param name="typeName">类型名称。</param>
        /// <param name="callerWrap">调用接口名。</param>
        public void RegWrapInterface(string typeName, object callerWrap)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                var entry = new EventEntryData(callerWrap);
                m_eventEntryMap[typeName] = entry;
            }
        }

        /// <summary>
        /// 清除释放事件。
        /// </summary>
        public void Dispose()
        {
            m_eventEntryMap.Clear();
            Dispatcher.Clear();
        }
    }
}