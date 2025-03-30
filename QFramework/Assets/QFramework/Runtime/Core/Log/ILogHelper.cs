namespace QFramework
{
    public static partial class QFrameworkLog
    {
        /// <summary>
        /// 游戏框架日志辅助器接口。
        /// </summary>
        public interface ILogHelper
        {
            /// <summary>
            /// 记录日志。
            /// </summary>
            /// <param name="type">游戏框架日志等级。</param>
            /// <param name="message">日志内容。</param>
            void Log(QFrameworkLogType type, object message);
        }
    }
}