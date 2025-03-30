using System;
using System.Text;

namespace QFramework.Utility
{
    public class DefaultStringFormatHelper : StringFormatHelper.IStringFormatHelper
    {
        private const int STRING_BUILDER_CAPACITY = 1024;

        [ThreadStatic]
        private static StringBuilder m_cachedStringBuilder = null;

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T">字符串参数的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg">字符串参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T>(string format, T arg)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <param name="arg9">字符串参数 9。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <param name="arg9">字符串参数 9。</param>
        /// <param name="arg10">字符串参数 10。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <param name="arg9">字符串参数 9。</param>
        /// <param name="arg10">字符串参数 10。</param>
        /// <param name="arg11">字符串参数 11。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <param name="arg9">字符串参数 9。</param>
        /// <param name="arg10">字符串参数 10。</param>
        /// <param name="arg11">字符串参数 11。</param>
        /// <param name="arg12">字符串参数 12。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字符串参数 13 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <param name="arg9">字符串参数 9。</param>
        /// <param name="arg10">字符串参数 10。</param>
        /// <param name="arg11">字符串参数 11。</param>
        /// <param name="arg12">字符串参数 12。</param>
        /// <param name="arg13">字符串参数 13。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字符串参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字符串参数 14 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <param name="arg9">字符串参数 9。</param>
        /// <param name="arg10">字符串参数 10。</param>
        /// <param name="arg11">字符串参数 11。</param>
        /// <param name="arg12">字符串参数 12。</param>
        /// <param name="arg13">字符串参数 13。</param>
        /// <param name="arg14">字符串参数 14。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字符串参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字符串参数 14 的类型。</typeparam>
        /// <typeparam name="T15">字符串参数 15 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <param name="arg9">字符串参数 9。</param>
        /// <param name="arg10">字符串参数 10。</param>
        /// <param name="arg11">字符串参数 11。</param>
        /// <param name="arg12">字符串参数 12。</param>
        /// <param name="arg13">字符串参数 13。</param>
        /// <param name="arg14">字符串参数 14。</param>
        /// <param name="arg15">字符串参数 15。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
            return m_cachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字符串参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字符串参数 14 的类型。</typeparam>
        /// <typeparam name="T15">字符串参数 15 的类型。</typeparam>
        /// <typeparam name="T16">字符串参数 16 的类型。</typeparam>
        /// <param name="format">字符串格式。</param>
        /// <param name="arg1">字符串参数 1。</param>
        /// <param name="arg2">字符串参数 2。</param>
        /// <param name="arg3">字符串参数 3。</param>
        /// <param name="arg4">字符串参数 4。</param>
        /// <param name="arg5">字符串参数 5。</param>
        /// <param name="arg6">字符串参数 6。</param>
        /// <param name="arg7">字符串参数 7。</param>
        /// <param name="arg8">字符串参数 8。</param>
        /// <param name="arg9">字符串参数 9。</param>
        /// <param name="arg10">字符串参数 10。</param>
        /// <param name="arg11">字符串参数 11。</param>
        /// <param name="arg12">字符串参数 12。</param>
        /// <param name="arg13">字符串参数 13。</param>
        /// <param name="arg14">字符串参数 14。</param>
        /// <param name="arg15">字符串参数 15。</param>
        /// <param name="arg16">字符串参数 16。</param>
        /// <returns>格式化后的字符串。</returns>
        public string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (format == null)
            {
                throw new QFrameworkException("Format is invalid.");
            }

            CheckCachedStringBuilder();
            m_cachedStringBuilder.Length = 0;
            m_cachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
            return m_cachedStringBuilder.ToString();
        }

        private static void CheckCachedStringBuilder()
        {
            if (m_cachedStringBuilder == null)
            {
                m_cachedStringBuilder = new StringBuilder(STRING_BUILDER_CAPACITY);
            }
        }
    }
}