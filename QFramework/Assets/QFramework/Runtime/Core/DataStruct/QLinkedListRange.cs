using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using QFramework.Utility;

namespace QFramework
{
    /// <summary>
    /// 游戏框架链表范围。
    /// </summary>
    /// <typeparam name="T">指定链表范围的元素类型。</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct QLinkedListRange<T> : IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// 获取链表范围的开始结点。
        /// </summary>
        public LinkedListNode<T> First { get; }

        /// <summary>
        /// 获取链表范围的终结标记结点。
        /// </summary>
        public LinkedListNode<T> Last { get; }

        /// <summary>
        /// 获取链表范围是否有效。
        /// </summary>
        public bool IsValid => First != null && Last != null && First != Last;

        /// <summary>
        /// 获取链表范围的结点数量。
        /// </summary>
        public int Count
        {
            get
            {
                if (!IsValid)
                {
                    return 0;
                }

                int count = 0;
                for (LinkedListNode<T> current = First; current != null && current != Last; current = current.Next)
                {
                    count++;
                }

                return count;
            }
        }

        /// <summary>
        /// 检查是否包含指定值。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <returns>是否包含指定值。</returns>
        public bool Contains(T value)
        {
            if (!IsValid)
            {
                return false;
            }
            for (LinkedListNode<T> current = First; current != null && current != Last; current = current.Next)
            {
                if (current.Value.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 初始化游戏框架链表范围的新实例。
        /// </summary>
        /// <param name="first">链表范围的开始结点。</param>
        /// <param name="last">链表范围的终结标记结点。</param>
        public QLinkedListRange(LinkedListNode<T> first, LinkedListNode<T> last)
        {
            if (first == null || last == null || first == last)
            {
                throw new QFrameworkException("Range is invalid.");
            }

            First = first;
            Last = last;
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>循环访问集合的枚举数。</returns>
        public QEnumerator GetEnumerator()
        {
            return new QEnumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 循环访问集合的枚举数。
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct QEnumerator : IEnumerator<T>, IEnumerator
        {
            private readonly QLinkedListRange<T> m_qLinkedListRange;
            private LinkedListNode<T> m_currentNode;
            private T m_currentValue;

            internal QEnumerator(QLinkedListRange<T> range)
            {
                if (!range.IsValid)
                {
                    throw new QFrameworkException("Range is invalid.");
                }

                m_qLinkedListRange = range;
                m_currentNode = m_qLinkedListRange.First;
                m_currentValue = default(T);
            }

            /// <summary>
            /// 获取当前结点。
            /// </summary>
            public T Current => m_currentValue;

            /// <summary>
            /// 获取当前的枚举数。
            /// </summary>
            object IEnumerator.Current => m_currentValue;

            /// <summary>
            /// 清理枚举数。
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// 获取下一个结点。
            /// </summary>
            /// <returns>返回下一个结点。</returns>
            public bool MoveNext()
            {
                if (m_currentNode == null || m_currentNode == m_qLinkedListRange.Last)
                {
                    return false;
                }

                m_currentValue = m_currentNode.Value;
                m_currentNode = m_currentNode.Next;
                return true;
            }

            /// <summary>
            /// 重置枚举数。
            /// </summary>
            void IEnumerator.Reset()
            {
                m_currentNode = m_qLinkedListRange.First;
                m_currentValue = default(T);
            }
        }
    }
}