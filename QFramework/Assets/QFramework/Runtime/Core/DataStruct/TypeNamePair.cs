using System;
using System.Runtime.InteropServices;
using QFramework.Utility;

namespace QFramework
{
    /// <summary>
    /// 类型和名称的组合值。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct TypeNamePair : IEquatable<TypeNamePair>
    {
        private readonly Type m_type;
        private readonly string m_name;

        /// <summary>
        /// 初始化类型和名称的组合值的新实例。
        /// </summary>
        /// <param name="type">类型。</param>
        public TypeNamePair(Type type)
            : this(type, string.Empty)
        {
        }

        /// <summary>
        /// 初始化类型和名称的组合值的新实例。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <param name="name">名称。</param>
        public TypeNamePair(Type type, string name)
        {
            if (type == null)
            {
                throw new QFrameworkException("Type is invalid.");
            }

            m_type = type;
            m_name = name ?? string.Empty;
        }

        /// <summary>
        /// 获取类型。
        /// </summary>
        public Type Type => m_type;

        /// <summary>
        /// 获取名称。
        /// </summary>
        public string Name => m_name;

        /// <summary>
        /// 获取类型和名称的组合值字符串。
        /// </summary>
        /// <returns>类型和名称的组合值字符串。</returns>
        public override string ToString()
        {
            if (m_type == null)
            {
                throw new QFrameworkException("Type is invalid.");
            }

            string typeName = m_type.FullName;
            return string.IsNullOrEmpty(m_name) ? typeName : StringFormatHelper.Format("{0}.{1}", typeName, m_name);
        }

        /// <summary>
        /// 获取对象的哈希值。
        /// </summary>
        /// <returns>对象的哈希值。</returns>
        public override int GetHashCode()
        {
            return m_type.GetHashCode() ^ m_name.GetHashCode();
        }

        /// <summary>
        /// 比较对象是否与自身相等。
        /// </summary>
        /// <param name="obj">要比较的对象。</param>
        /// <returns>被比较的对象是否与自身相等。</returns>
        public override bool Equals(object obj)
        {
            return obj is TypeNamePair && Equals((TypeNamePair)obj);
        }

        /// <summary>
        /// 比较对象是否与自身相等。
        /// </summary>
        /// <param name="value">要比较的对象。</param>
        /// <returns>被比较的对象是否与自身相等。</returns>
        public bool Equals(TypeNamePair value)
        {
            return m_type == value.Type && m_name == value.Name;
        }

        /// <summary>
        /// 判断两个对象是否相等。
        /// </summary>
        /// <param name="a">值 a。</param>
        /// <param name="b">值 b。</param>
        /// <returns>两个对象是否相等。</returns>
        public static bool operator ==(TypeNamePair a, TypeNamePair b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 判断两个对象是否不相等。
        /// </summary>
        /// <param name="a">值 a。</param>
        /// <param name="b">值 b。</param>
        /// <returns>两个对象是否不相等。</returns>
        public static bool operator !=(TypeNamePair a, TypeNamePair b)
        {
            return !(a == b);
        }
    }
}