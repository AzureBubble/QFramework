using YooAsset;

namespace QFramework
{
    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    internal class RemoteServices : IRemoteServices
    {
        private readonly string m_defaultHostServer;
        private readonly string m_fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            m_defaultHostServer = defaultHostServer;
            m_fallbackHostServer = fallbackHostServer;
        }

        public string GetRemoteMainURL(string fileName)
        {
            return $"{m_defaultHostServer}/{fileName}";
        }

        public string GetRemoteFallbackURL(string fileName)
        {
            return $"{m_fallbackHostServer}/{fileName}";
        }
    }
}