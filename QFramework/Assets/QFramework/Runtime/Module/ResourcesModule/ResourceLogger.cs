using System;
using YooAsset;

namespace QFramework
{
    internal class ResourceLogger : ILogger
    {
        public void Log(string message)
        {
            Debugger.Info(message);
        }

        public void Warning(string message)
        {
            Debugger.Warning(message);
        }

        public void Error(string message)
        {
            Debugger.Error(message);
        }

        public void Exception(Exception exception)
        {
            Debugger.Fatal(exception);
        }
    }
}