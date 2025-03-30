using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public static class EventString2IdUtil
    {
        private static readonly Dictionary<string, int> m_eventString2IdMap = new Dictionary<string, int>();
        private static readonly Dictionary<int, string> m_eventId2StringMap = new Dictionary<int, string>();

        private static int m_currentEventStringId = 0;

        /// <summary>
        /// 字符串转ID
        /// </summary>
        /// <param name="eventName">事件字符串</param>
        /// <returns></returns>
        public static int EventString2Id(string eventName)
        {
            if (m_eventString2IdMap.TryGetValue(eventName, out var id))
            {
                return id;
            }

            id = ++m_currentEventStringId;
            m_eventString2IdMap[eventName] = id;
            m_eventId2StringMap[id] = eventName;
            return id;
        }

        /// <summary>
        /// ID转字符串
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <returns></returns>
        public static string Id2EventString(int eventID)
        {
            return m_eventId2StringMap.GetValueOrDefault(eventID);
        }
    }
}