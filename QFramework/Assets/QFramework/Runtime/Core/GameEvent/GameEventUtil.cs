using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public static partial class GameEventUtil
    {
        private static readonly Dictionary<string, int> m_toStringIdMap = new Dictionary<string, int>();
        private static readonly Dictionary<int, string> m_toStringMap = new Dictionary<int, string>();

        private static int m_currentEventStringId = 0;

        /// <summary>
        /// 字符串转ID
        /// </summary>
        /// <param name="eventName">事件字符串</param>
        /// <returns></returns>
        public static int ToStringId(string eventName)
        {
            if (m_toStringIdMap.TryGetValue(eventName, out var id))
            {
                return id;
            }

            id = ++m_currentEventStringId;
            m_toStringIdMap[eventName] = id;
            m_toStringMap[id] = eventName;
            return id;
        }

        /// <summary>
        /// ID转字符串
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <returns></returns>
        public static string ToString(int eventID)
        {
            return m_toStringMap.GetValueOrDefault(eventID);
        }
    }
}