namespace QFramework
{
    /// <summary>
    /// 加载资源状态。
    /// </summary>
    public enum LoadResourceStatus : byte
    {
        /// <summary>
        /// 加载资源完成。
        /// </summary>
        LoadAssetSuccess = 0,

        /// <summary>
        /// 资源不存在。
        /// </summary>
        AssetNotExist,

        /// <summary>
        /// 资源尚未准备完毕。
        /// </summary>
        AssetNotReady,

        /// <summary>
        /// 依赖资源错误。
        /// </summary>
        DependencyError,

        /// <summary>
        /// 资源类型错误。
        /// </summary>
        AssetTypeError,

        /// <summary>
        /// 加载资源错误。
        /// </summary>
        LoadAssetError
    }
}