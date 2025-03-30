using QFramework.Utility;

namespace QFramework
{
    #region 卸载场景相关回调委托

    /// <summary>
    /// 卸载场景成功回调函数。
    /// </summary>
    /// <param name="sceneAssetName">要卸载的场景资源名称。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void UnloadSceneSuccessCallback(string sceneAssetName, object userData);

    /// <summary>
    /// 卸载场景失败回调函数。
    /// </summary>
    /// <param name="sceneAssetName">要卸载的场景资源名称。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void UnloadSceneFailureCallback(string sceneAssetName, object userData);

    #endregion
    
    /// <summary>
    /// 卸载场景回调函数集。
    /// </summary>
    public sealed class UnloadSceneCallbacks
    {
        /// <summary>
        /// 获取卸载场景成功回调函数。
        /// </summary>
        public UnloadSceneSuccessCallback UnloadSceneSuccessCallback { get; }

        /// <summary>
        /// 获取卸载场景失败回调函数。
        /// </summary>
        public UnloadSceneFailureCallback UnloadSceneFailureCallback { get; }

        /// <summary>
        /// 初始化卸载场景回调函数集的新实例。
        /// </summary>
        /// <param name="unloadSceneSuccessCallback">卸载场景成功回调函数。</param>
        public UnloadSceneCallbacks(UnloadSceneSuccessCallback unloadSceneSuccessCallback)
            : this(unloadSceneSuccessCallback, null)
        {
        }

        /// <summary>
        /// 初始化卸载场景回调函数集的新实例。
        /// </summary>
        /// <param name="unloadSceneSuccessCallback">卸载场景成功回调函数。</param>
        /// <param name="unloadSceneFailureCallback">卸载场景失败回调函数。</param>
        public UnloadSceneCallbacks(UnloadSceneSuccessCallback unloadSceneSuccessCallback, UnloadSceneFailureCallback unloadSceneFailureCallback = null)
        {
            if (unloadSceneSuccessCallback == null)
            {
                throw new QFrameworkException("Unload scene success callback is invalid.");
            }

            UnloadSceneSuccessCallback = unloadSceneSuccessCallback;
            UnloadSceneFailureCallback = unloadSceneFailureCallback;
        }
    }
}