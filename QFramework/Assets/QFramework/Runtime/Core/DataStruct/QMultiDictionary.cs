using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using QFramework.Utility;

namespace QFramework
{
    /// <summary>
    /// 游戏框架多值字典类。
    /// </summary>
    /// <typeparam name="TKey">指定多值字典的主键类型。</typeparam>
    /// <typeparam name="TValue">指定多值字典的值类型。</typeparam>
    public class QMultiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, QLinkedListRange<TValue>>>, IEnumerable
    {
        private readonly QLinkedList<TValue> m_linkedList;
        private readonly Dictionary<TKey, QLinkedListRange<TValue>> m_dictionary;

        /// <summary>
        /// 获取多值字典中实际包含的主键数量。
        /// </summary>
        public int Count => m_dictionary.Count;

        /// <summary>
        /// 初始化游戏框架多值字典类的新实例。
        /// </summary>
        public QMultiDictionary()
        {
            m_linkedList = new QLinkedList<TValue>();
            m_dictionary = new Dictionary<TKey, QLinkedListRange<TValue>>();
        }

        /// <summary>
        /// 获取多值字典中指定主键的范围。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <returns>指定主键的范围。</returns>
        public QLinkedListRange<TValue> this[TKey key]
        {
            get
            {
                m_dictionary.TryGetValue(key, out var range);
                return range;
            }
        }

        /// <summary>
        /// 清理多值字典。
        /// </summary>
        public void Clear()
        {
            m_dictionary.Clear();
            m_linkedList.Clear();
        }

        /// <summary>
        /// 检查多值字典中是否包含指定主键。
        /// </summary>
        /// <param name="key">要检查的主键。</param>
        /// <returns>多值字典中是否包含指定主键。</returns>
        public bool ContainsKey(TKey key)
        {
            return m_dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 检查多值字典中是否包含指定值。
        /// </summary>
        /// <param name="key">要检查的主键。</param>
        /// <param name="value">要检查的值。</param>
        /// <returns>多值字典中是否包含指定值。</returns>
        public bool ContainsValue(TKey key, TValue value)
        {
            if (m_dictionary.TryGetValue(key, out var range))
            {
                return range.Contains(value);
            }

            return false;
        }

        /// <summary>
        /// 尝试获取多值字典中指定主键的范围。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <param name="range">指定主键的范围。</param>
        /// <returns>是否获取成功。</returns>
        public bool TryGetValue(TKey key, out QLinkedListRange<TValue> range)
        {
            return m_dictionary.TryGetValue(key, out range);
        }

        /// <summary>
        /// 向指定的主键增加指定的值。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <param name="value">指定的值。</param>
        public void Add(TKey key, TValue value)
        {
            if (m_dictionary.TryGetValue(key, out var range))
            {
                m_linkedList.AddBefore(range.Last, value);
            }
            else
            {
                LinkedListNode<TValue> first = m_linkedList.AddLast(value);
                LinkedListNode<TValue> last = m_linkedList.AddLast(default(TValue));
                m_dictionary.Add(key, new QLinkedListRange<TValue>(first, last));
            }
        }

        /// <summary>
        /// 从指定的主键中移除指定的值。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <param name="value">指定的值。</param>
        /// <returns>是否移除成功。</returns>
        public bool Remove(TKey key, TValue value)
        {
            if (m_dictionary.TryGetValue(key, out var range))
            {
                for (LinkedListNode<TValue> current = range.First; current != null && current != range.Last; current = current.Next)
                {
                    if (current.Value.Equals(value))
                    {
                        if (current == range.First)
                        {
                            LinkedListNode<TValue> next = current.Next;
                            if (next == range.Last)
                            {
                                m_linkedList.Remove(next);
                                m_dictionary.Remove(key);
                            }
                            else
                            {
                                m_dictionary[key] = new QLinkedListRange<TValue>(next, range.Last);
                            }
                        }
                        m_linkedList.Remove(current);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 从指定的主键中移除所有的值。
        /// </summary>
        /// <param name="key">指定的主键。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveAll(TKey key)
        {
            if (m_dictionary.Remove(key, out var range))
            {
                LinkedListNode<TValue> current = range.First;
                while (current != null)
                {
                    LinkedListNode<TValue> next = current != range.Last ? current.Next : null;
                    m_linkedList.Remove(current);
                    current = next;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>循环访问集合的枚举数。</returns>
        public QEnumerator GetEnumerator()
        {
            return new QEnumerator(m_dictionary);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>循环访问集合的枚举数。</returns>
        IEnumerator<KeyValuePair<TKey, QLinkedListRange<TValue>>> IEnumerable<KeyValuePair<TKey, QLinkedListRange<TValue>>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>循环访问集合的枚举数。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// 循环访问集合的枚举数。
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct QEnumerator : IEnumerator<KeyValuePair<TKey, QLinkedListRange<TValue>>>, IEnumerator
        {
            private Dictionary<TKey, QLinkedListRange<TValue>>.Enumerator m_enumerator;

            internal QEnumerator(Dictionary<TKey, QLinkedListRange<TValue>> dictionary)
            {
                if (dictionary == null)
                {
                    throw new QFrameworkException("Dictionary is invalid.");
                }

                m_enumerator = dictionary.GetEnumerator();
            }

            /// <summary>
            /// 获取当前结点。
            /// </summary>
            public KeyValuePair<TKey, QLinkedListRange<TValue>> Current => m_enumerator.Current;

            /// <summary>
            /// 获取当前的枚举数。
            /// </summary>
            object IEnumerator.Current => m_enumerator.Current;

            /// <summary>
            /// 清理枚举数。
            /// </summary>
            public void Dispose()
            {
                m_enumerator.Dispose();
            }

            /// <summary>
            /// 获取下一个结点。
            /// </summary>
            /// <returns>返回下一个结点。</returns>
            public bool MoveNext()
            {
                return m_enumerator.MoveNext();
            }

            /// <summary>
            /// 重置枚举数。
            /// </summary>
            void IEnumerator.Reset()
            {
                ((IEnumerator<KeyValuePair<TKey, QLinkedListRange<TValue>>>)m_enumerator).Reset();
            }
        }
    }
}