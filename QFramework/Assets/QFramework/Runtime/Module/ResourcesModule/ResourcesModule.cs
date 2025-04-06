using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using QFramework.Utility;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

#if UNITY_WEBGL && WEIXINMINIGAME && !UNITY_EDITOR
using WeChatWASM;
#endif

namespace QFramework
{
    public sealed partial class ResourcesModule : Module, IResourcesModule
    {
        public string DefaultPackageName { get; set; } = "DefaultPackage";
        public EPlayMode PlayMode { get; set; } = EPlayMode.OfflinePlayMode;
        public EncryptionType EncryptionType { get; set; } = EncryptionType.None;
        public long Milliseconds { get; set; } = 30;
        public override int Priority => 4;

        public string ApplicableGameVersion { get; }
        public int InternalResourceVersion { get; }
        public bool UpdatableWhilePlaying { get; set; }
        public int DownloadingMaxNum { get; set; }
        public int FailedTryAgainCount { get; set; }

        /// <summary>
        /// 默认资源包。
        /// </summary>
        internal ResourcePackage DefaultPackage { get; private set; }

        /// <summary>
        /// 资源包列表。
        /// </summary>
        private Dictionary<string, ResourcePackage> m_packageMap => new Dictionary<string, ResourcePackage>();

        /// <summary>
        /// 资源信息列表。
        /// </summary>
        private readonly Dictionary<string, AssetInfo> m_assetInfoMap = new Dictionary<string, AssetInfo>();

        /// <summary>
        /// 正在加载的资源列表。
        /// </summary>
        private readonly HashSet<string> m_assetLoadingList = new HashSet<string>();

        public override void OnInit()
        {
        }

        public void Initialize()
        {
            // 初始化Yooaseet资源系统
            YooAssets.Initialize(new ResourceLogger());
            YooAssets.SetOperationSystemMaxTimeSlice(Milliseconds);
            // 创建默认资源包
            DefaultPackage = YooAssets.TryGetPackage(DefaultPackageName);

            if (DefaultPackage == null)
            {
                DefaultPackage = YooAssets.CreatePackage(DefaultPackageName);
                YooAssets.SetDefaultPackage(DefaultPackage);
            }

            // 初始化资源对象池
            IObjectPoolModule objectPoolModule = ModuleSystem.GetModule<IObjectPoolModule>();
            SetObjectPoolModule(objectPoolModule);
        }

        public async UniTask<InitializationOperation> InitPackage(string customPackageName)
        {
#if UNITY_EDITOR
            // 编辑器模式使用
            EPlayMode playMode = (EPlayMode)UnityEditor.EditorPrefs.GetInt("EditorPlayMode");
            Debugger.Warning($"Editor Module Used: {playMode}");
#else
            // 运行时使用
            EPlayMode playMode = PlayMode;
#endif
            if (m_packageMap.TryGetValue(customPackageName, out var resourcePackage))
            {
                if (resourcePackage.InitializeStatus is EOperationStatus.Processing or EOperationStatus.Succeed)
                {
                    Debugger.Error($"ResourceSystem has already init package : {customPackageName}");
                    return null;
                }
                else
                {
                    m_packageMap.Remove(customPackageName);
                }
            }

            // YooAsset: https://www.yooasset.com/docs/guide-runtime/CodeTutorial1
            // 创建资源包裹类
            var package = YooAssets.TryGetPackage(customPackageName);

            if (package == null)
            {
                package = YooAssets.CreatePackage(customPackageName);
            }

            m_packageMap[customPackageName] = package;

            InitializationOperation operation = null;
            IDecryptionServices decryptionServices = CreateDecryptionServices();

            if (playMode == EPlayMode.EditorSimulateMode)
            {
                // 编辑器下的模拟模式
                var buildResult = EditorSimulateModeHelper.SimulateBuild(customPackageName);
                var packageRoot = buildResult.PackageRootDirectory;
                var createParameters = new EditorSimulateModeParameters();
                createParameters.EditorFileSystemParameters =
                    FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
                operation = package.InitializeAsync(createParameters);
            }
            else if (playMode == EPlayMode.OfflinePlayMode)
            {
                // 单机运行模式
                var createParameters = new OfflinePlayModeParameters();
                createParameters.BuildinFileSystemParameters =
                    FileSystemParameters.CreateDefaultBuildinFileSystemParameters(decryptionServices);
                ;
                operation = package.InitializeAsync(createParameters);
            }
            else if (playMode == EPlayMode.HostPlayMode)
            {
                // 联机运行模式
                IRemoteServices remoteServices =
                    new RemoteServices(HotfixRemoteServerURL, FallbackHotfixRemoteServerURL);
                var createParameters = new HostPlayModeParameters
                {
                    BuildinFileSystemParameters =
                        FileSystemParameters.CreateDefaultBuildinFileSystemParameters(decryptionServices),
                    CacheFileSystemParameters =
                        FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices, decryptionServices)
                };
                operation = package.InitializeAsync(createParameters);
            }
            else if (playMode == EPlayMode.WebPlayMode)
            {
                // WebGL运行模式
                //说明：RemoteServices类定义请参考联机运行模式！
                var createParameters = new WebPlayModeParameters();

#if UNITY_WEBGL && WEIXINMINIGAME && !UNITY_EDITOR
                Debugger.Info("=======================WEIXINMINIGAME=======================");
                IWebDecryptionServices webDecryptionServices = CreateWebDecryptionServices();
                // 注意：如果有子目录，请修改此处！
                string packageRoot = $"{WeChatWASM.WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE";
                IRemoteServices remoteServices =
 new RemoteServices(HotfixRemoteServerURL, FallbackHotfixRemoteServerURL);
                createParameters.WebServerFileSystemParameters =
 WechatFileSystemCreater.CreateFileSystemParameters(packageRoot, remoteServices, webDecryptionServices);
#else
                Debugger.Info("=======================UNITY_WEBGL=======================");
                createParameters.WebServerFileSystemParameters =
                    FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
#endif
                operation = package.InitializeAsync(createParameters);
            }

            await operation.ToUniTask();
            Debugger.Info($"Init resource package version. InitState: {operation?.Status}");
            return operation;
        }

        #region 加密服务相关

        /// <summary>
        /// 创建解密服务。
        /// </summary>
        private IDecryptionServices CreateDecryptionServices()
        {
            return EncryptionType switch
            {
                EncryptionType.FileOffSet => new FileOffsetDecryption(),
                EncryptionType.FileStream => new FileStreamDecryption(),
                _ => null
            };
        }

        /// <summary>
        /// 创建Web解密服务。
        /// </summary>
        private IWebDecryptionServices CreateWebDecryptionServices()
        {
            return EncryptionType switch
            {
                EncryptionType.FileOffSet => new FileOffsetWebDecryption(),
                EncryptionType.FileStream => new FileStreamWebDecryption(),
                _ => null
            };
        }

        #endregion

        public override void OnDispose()
        {
        }

        #region 对象池参数相关

        public float AssetObjPoolAutoReleaseInterval { get; set; }
        public int AssetObjPoolCapacity { get; set; }
        public float AssetObjPoolExpireTime { get; set; }
        public int AssetObjPoolPriority { get; set; }

        #endregion

        #region RemoteServicesUrl

        public string HotfixRemoteServerURL { get; set; }
        public string FallbackHotfixRemoteServerURL { get; set; }

        public void SetRemoteServicesUrl(string defaultHostServer, string fallbackHostServer)
        {
            HotfixRemoteServerURL = defaultHostServer;
            FallbackHotfixRemoteServerURL = fallbackHostServer;
        }

        #endregion

        #region 卸载资源相关

        public void UnloadUnusedAssets()
        {
            m_assetObjPool.ReleaseAllUnused();

            foreach (var package in m_packageMap.Values)
            {
                if (package != null && package.InitializeStatus == EOperationStatus.Succeed)
                {
                    package.UnloadUnusedAssetsAsync();
                }
            }
        }

        public void ForceUnloadAllAssets()
        {
#if UNITY_WEBGL
            Debugger.Warning($"WebGL not support invoke {nameof(ForceUnloadAllAssets)}");
			return;
#else
            foreach (var package in m_packageMap.Values)
            {
                if (package != null && package.InitializeStatus == EOperationStatus.Succeed)
                {
                    package.UnloadAllAssetsAsync();
                }
            }
#endif
        }

        public void ForceUnloadUnusedAssets(bool performGCCollect)
        {
            m_forceUnloadUnusedAssetsAction?.Invoke(performGCCollect);
        }

        #endregion

        #region 检查资源相关

        /// <summary>
        /// 是否需要从远端更新下载。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="packageName">资源包名称。</param>
        public bool NeedDownloadAssetsFromRemote(string location, string packageName = "")
        {
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.IsNeedDownloadFromRemote(location)
                : YooAssets.GetPackage(packageName).IsNeedDownloadFromRemote(location);
        }

        /// <summary>
        /// 是否需要从远端更新下载。
        /// </summary>
        /// <param name="assetInfo">资源信息。</param>
        /// <param name="packageName">资源包名称。</param>
        public bool NeedDownloadAssetsFromRemote(AssetInfo assetInfo, string packageName = "")
        {
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.IsNeedDownloadFromRemote(assetInfo)
                : YooAssets.GetPackage(packageName).IsNeedDownloadFromRemote(assetInfo);
        }

        public HasAssetResult CheckAssetIsExist(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }

            if (!CheckLocationValid(location))
            {
                return HasAssetResult.Valid;
            }

            AssetInfo assetInfo = GetAssetInfo(location, packageName);

            if (assetInfo == null)
            {
                return HasAssetResult.NotExist;
            }

            if (NeedDownloadAssetsFromRemote(assetInfo))
            {
                return HasAssetResult.AssetNeedDonwloadOnline;
            }

            return HasAssetResult.AssetOnDisk;
        }

        public bool CheckLocationValid(string location, string packageName = "")
        {
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.CheckLocationValid(location)
                : YooAssets.GetPackage(packageName).CheckLocationValid(location);
        }

        #endregion

        #region 获取资源信息列表相关

        public AssetInfo[] GetAssetInfos(string resTag, string packageName = "")
        {
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.GetAssetInfos(resTag)
                : YooAssets.GetPackage(packageName)?.GetAssetInfos(resTag);
        }

        public AssetInfo[] GetAssetInfos(string[] tags, string packageName = "")
        {
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.GetAssetInfos(tags)
                : YooAssets.GetPackage(packageName)?.GetAssetInfos(tags);
        }

        public AssetInfo GetAssetInfo(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }

            if (string.IsNullOrEmpty(packageName))
            {
                if (m_assetInfoMap.TryGetValue(location, out var assetInfo))
                {
                    return assetInfo;
                }

                assetInfo = YooAssets.GetAssetInfo(location);
                m_assetInfoMap[location] = assetInfo;
                return assetInfo;
            }
            else
            {
                string key = $"{packageName}/{location}";

                if (m_assetInfoMap.TryGetValue(key, out var assetInfo))
                {
                    return assetInfo;
                }

                var package = YooAssets.GetPackage(packageName);

                if (package == null)
                {
                    throw new QFrameworkException($"The package does not exist. Package Name :{packageName}");
                }

                assetInfo = package.GetAssetInfo(location);
                m_assetInfoMap[key] = assetInfo;
                return assetInfo;
            }
        }

        #endregion

        #region 获取资源句柄

        /// <summary>
        /// 获取同步资源句柄。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">资源类型。</typeparam>
        /// <returns>资源句柄。</returns>
        private AssetHandle GetHandleSync<T>(string location, string packageName = "") where T : Object
        {
            return GetHandleSync(location, typeof(T), packageName);
        }

        private AssetHandle GetHandleSync(string location, Type assetType, string packageName = "")
        {
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.LoadAssetSync(location, assetType)
                : YooAssets.GetPackage(packageName).LoadAssetSync(location, assetType);
        }

        private AssetHandle GetHandleAsync<T>(string location, string packageName = "") where T : Object
        {
            return GetHandleAsync(location, typeof(T), packageName);
        }

        private AssetHandle GetHandleAsync(string location, Type assetType, string packageName = "")
        {
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.LoadAssetAsync(location, assetType)
                : YooAssets.GetPackage(packageName).LoadAssetAsync(location, assetType);
        }

        #endregion

        #region 异步加载资源相关

        public async void LoadAssetAsync(string location, int priority, LoadAssetCallbacks loadAssetCallbacks,
            object userData, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new QFrameworkException("Load asset callbacks is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);
            await TryWaitingLoading(assetObjectKey);
            float duration = Time.time;
            AssetObject assetObject = m_assetObjPool.Get(assetObjectKey);

            if (assetObject != null)
            {
                await UniTask.Yield();
                loadAssetCallbacks.LoadAssetSuccessCallback(location, assetObject.TargetObj, Time.time - duration,
                    userData);
                return;
            }

            m_assetLoadingList.Add(assetObjectKey);
            AssetInfo assetInfo = GetAssetInfo(location, packageName);

            if (!string.IsNullOrEmpty(assetInfo.Error))
            {
                m_assetLoadingList.Remove(assetObjectKey);
                string errorMsg = StringFormatHelper.Format("Can not load asset '{0}' because :'{1}'.", location,
                    assetInfo.Error);

                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.AssetNotExist, errorMsg,
                        userData);
                    return;
                }
                throw new QFrameworkException(errorMsg);
            }

            AssetHandle handle = GetHandleAsync(location, assetInfo.AssetType, packageName);

            if (loadAssetCallbacks.LoadAssetUpdateCallback != null)
            {
                InvokeProgress(location, handle, loadAssetCallbacks.LoadAssetUpdateCallback, userData);
            }

            await handle.ToUniTask();

            if (handle.AssetObject == null || handle.Status == EOperationStatus.Failed)
            {
                m_assetLoadingList.Remove(assetObjectKey);
                string errorMsg = StringFormatHelper.Format("Can not load asset '{0}'.", location);

                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.AssetNotReady, errorMsg,
                        userData);
                    return;
                }

                throw new QFrameworkException(errorMsg);
            }
            else
            {
                assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
                m_assetObjPool.Create(assetObject, true);
                m_assetLoadingList.Remove(assetObjectKey);
                loadAssetCallbacks.LoadAssetSuccessCallback(location, handle.AssetObject, Time.time - duration,
                    userData);
            }
        }

        private async UniTaskVoid InvokeProgress(string location, AssetHandle assetHandle,
            LoadAssetUpdateCallback loadAssetUpdateCallback, object userData)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }

            if (loadAssetUpdateCallback != null)
            {
                while (assetHandle != null && assetHandle.IsValid && !assetHandle.IsDone)
                {
                    await UniTask.Yield();
                    loadAssetUpdateCallback.Invoke(location, assetHandle.Progress, userData);
                }
            }
        }

        private readonly TimeoutController m_timeoutController = new TimeoutController();

        private async UniTask TryWaitingLoading(string assetObjectKey)
        {
            if (m_assetLoadingList.Contains(assetObjectKey))
            {
                try
                {
                    await UniTask.WaitUntil(() => !m_assetLoadingList.Contains(assetObjectKey))
#if UNITY_EDITOR
                        .AttachExternalCancellation(m_timeoutController.Timeout(TimeSpan.FromSeconds(60)));
                    m_timeoutController.Reset();
#else
                    ;
#endif
                }
                catch (OperationCanceledException ex)
                {
                    if (m_timeoutController.IsTimeout())
                    {
                        Debugger.Error($"LoadAssetAsync Waiting {assetObjectKey} timeout. reason:{ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 获取资源定位地址的缓存Key。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="packageName">资源包名称。</param>
        /// <returns>资源定位地址的缓存Key。</returns>
        private string GetCacheKey(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName) || packageName.Equals(DefaultPackageName))
            {
                return location;
            }

            return $"{packageName}/{location}";
        }

        public async void LoadAssetAsync(string location, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks,
            object userData, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new QFrameworkException("Load asset callbacks is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);
            await TryWaitingLoading(assetObjectKey);
            var duration = Time.time;
            AssetObject assetObject = m_assetObjPool.Get(assetObjectKey);
            if (assetObject != null)
            {
                await UniTask.Yield();
                loadAssetCallbacks.LoadAssetSuccessCallback(location, assetObject.TargetObj, Time.time - duration, userData);
                return;
            }

            m_assetLoadingList.Add(assetObjectKey);
            AssetInfo assetInfo = GetAssetInfo(location, packageName);

            if (!string.IsNullOrEmpty(assetInfo.Error))
            {
                m_assetLoadingList.Remove(assetObjectKey);
                string errorMsg = StringFormatHelper.Format("Can not load asset '{0}' because :'{1}'.", location, assetInfo.Error);

                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.AssetNotExist, errorMsg, userData);
                    return;
                }
                throw new QFrameworkException(errorMsg);
            }


            AssetHandle handle = GetHandleAsync(location, assetType, packageName);
            if (loadAssetCallbacks.LoadAssetUpdateCallback != null)
            {
                InvokeProgress(location, handle, loadAssetCallbacks.LoadAssetUpdateCallback, userData).Forget();
            }

            await handle.ToUniTask();

            if (handle.AssetObject == null || handle.Status == EOperationStatus.Failed)
            {
                m_assetLoadingList.Remove(assetObjectKey);
                string errorMsg = StringFormatHelper.Format("Can not load asset '{0}'.", location);

                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.AssetNotReady, errorMsg, userData);
                }
                throw new QFrameworkException(errorMsg);
            }
            else
            {
                assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
                m_assetObjPool.Create(assetObject, true);
                m_assetLoadingList.Remove(assetObjectKey);

                loadAssetCallbacks.LoadAssetSuccessCallback(location, handle.AssetObject, Time.time - duration, userData);
            }
        }

        public async UniTaskVoid LoadAssetAsync<T>(string location, Action<T> callback, string packageName = "")
            where T : Object
        {
            if (string.IsNullOrEmpty(location))
            {
                Debugger.Error("Asset name is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);
            await TryWaitingLoading(assetObjectKey);
            AssetObject assetObject = m_assetObjPool.Get(assetObjectKey);
            if (assetObject != null)
            {
                await UniTask.Yield();
                callback?.Invoke(assetObject.TargetObj as T);
                return;
            }

            m_assetLoadingList.Add(assetObjectKey);
            AssetHandle handle = GetHandleAsync<T>(location, packageName);
            handle.Completed += assetHandle =>
            {
                m_assetLoadingList.Remove(assetObjectKey);

                if (assetHandle.AssetObject != null)
                {
                    assetObject = AssetObject.Create(assetObjectKey, assetHandle.AssetObject, assetHandle, this);
                    m_assetObjPool.Create(assetObject, true);
                    callback?.Invoke(assetObject.TargetObj as T);
                }
                else
                {
                    callback?.Invoke(null);
                }
            };
        }

        public UniTask<T[]> LoadSubAssetsAsync<T>(string location, string packageName = "") where T : Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }

            throw new NotImplementedException();
        }

        public async UniTask<T> LoadAssetAsync<T>(string location, CancellationToken cancellationToken = default,
            string packageName = "") where T : Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }
            string assetObjectKey = GetCacheKey(location, packageName);
            await TryWaitingLoading(assetObjectKey);
            AssetObject assetObject = m_assetObjPool.Get(assetObjectKey);

            if (assetObject != null)
            {
                await UniTask.Yield();
                return assetObject.TargetObj as T;
            }

            m_assetLoadingList.Add(assetObjectKey);
            AssetHandle handle = GetHandleAsync<T>(location, packageName);
            bool cancelOrFailed = await handle.ToUniTask().AttachExternalCancellation(cancellationToken)
                .SuppressCancellationThrow();

            if (cancelOrFailed)
            {
                m_assetLoadingList.Remove(assetObjectKey);
                return null;
            }
            assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
            m_assetObjPool.Create(assetObject, true);
            m_assetLoadingList.Remove(assetObjectKey);
            return handle.AssetObject as T;
        }

        public async UniTask<GameObject> LoadGameObjectAsync(string location, Transform parent = null,
            CancellationToken cancellationToken = default,
            string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);
            await TryWaitingLoading(assetObjectKey);
            AssetObject assetObject = m_assetObjPool.Get(assetObjectKey);

            if (assetObject != null)
            {
                await UniTask.Yield();
                return AssetsReference.Instantiate(assetObject.TargetObj as GameObject, parent, this).gameObject;
            }
            m_assetLoadingList.Add(assetObjectKey);
            AssetHandle handle = GetHandleAsync<GameObject>(location, packageName);
            bool cancelOrFailed = await handle.ToUniTask().AttachExternalCancellation(cancellationToken)
                .SuppressCancellationThrow();

            if (cancelOrFailed)
            {
                m_assetLoadingList.Remove(assetObjectKey);
                return null;
            }
            var gameObject = AssetsReference.Instantiate(handle.AssetObject as GameObject, parent, this).gameObject;
            assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
            m_assetObjPool.Create(assetObject, true);
            m_assetLoadingList.Remove(assetObjectKey);
            return gameObject;
        }

        public AssetHandle LoadAssetAsyncHandle<T>(string location, string packageName = "") where T : Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.LoadAssetAsync<T>(location)
                : YooAssets.GetPackage(packageName).LoadAssetAsync<T>(location);
        }

        #endregion

        #region 同步加载资源相关

        public T LoadAsset<T>(string location, string packageName = "") where T : Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }
            string assetObjKey = GetCacheKey(location, packageName);
            AssetObject assetObject = m_assetObjPool.Get(assetObjKey);

            if (assetObject != null)
            {
                return assetObject.TargetObj as T;
            }

            AssetHandle handle = GetHandleSync<T>(location, packageName);
            T result = handle.AssetObject as T;
            assetObject = AssetObject.Create(assetObjKey, handle.AssetObject, handle, this);
            m_assetObjPool.Create(assetObject, true);
            return result;
        }

        public GameObject LoadGameObject(string location, Transform parent = null, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }
            string assetObjKey = GetCacheKey(location, packageName);
            AssetObject assetObject = m_assetObjPool.Get(assetObjKey);

            if (assetObject != null)
            {
                return AssetsReference.Instantiate(assetObject.TargetObj as GameObject, parent, this).gameObject;
            }

            AssetHandle handle = GetHandleSync<GameObject>(location, packageName);
            GameObject result = AssetsReference.Instantiate(handle.AssetObject as GameObject, parent, this).gameObject;
            assetObject = AssetObject.Create(assetObjKey, handle.AssetObject, handle, this);
            m_assetObjPool.Create(assetObject, true);
            return result;
        }

        public T[] LoadSubAssetsSync<T>(string location, string packageName = "") where T : Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }
            throw new NotImplementedException();
        }

        public AssetHandle LoadAssetSyncHandle<T>(string location, string packageName = "") where T : Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new QFrameworkException("Asset name is invalid.");
            }
            return string.IsNullOrEmpty(packageName)
                ? YooAssets.LoadAssetSync<T>(location)
                : YooAssets.GetPackage(packageName).LoadAssetSync<T>(location);
        }

        #endregion

        #region Clear

        public ClearCacheFilesOperation ClearCacheFilesAsync(
            EFileClearMode clearMode = EFileClearMode.ClearUnusedBundleFiles,
            string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(DefaultPackageName)
                : YooAssets.GetPackage(customPackageName);
            return package?.ClearCacheFilesAsync(clearMode);
        }

        public void ClearAllBundleFiles(string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(DefaultPackageName)
                : YooAssets.GetPackage(customPackageName);
            package?.ClearCacheFilesAsync(EFileClearMode.ClearAllBundleFiles);
        }

        #endregion

        #region CreateLoader

        public ResourceDownloaderOperation Downloader { get; set; }

        public ResourceDownloaderOperation CreateResourceDownloader(string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(DefaultPackageName)
                : YooAssets.GetPackage(customPackageName);
            Downloader = package.CreateResourceDownloader(DownloadingMaxNum, FailedTryAgainCount);
            return Downloader;
        }

        #endregion

        #region PackageVersion

        public string PackageVersion { get; set; }

        public string GetPackageVersion(string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(DefaultPackageName)
                : YooAssets.GetPackage(customPackageName);
            return package == null ? string.Empty : package.GetPackageVersion();
        }

        #endregion

        #region Request

        public RequestPackageVersionOperation RequestUpdatePackageVersionAsync(bool appendTimeTicks = false,
            int timeout = 60,
            string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(DefaultPackageName)
                : YooAssets.GetPackage(customPackageName);
            return package?.RequestPackageVersionAsync(appendTimeTicks, timeout);
        }

        public UpdatePackageManifestOperation RequestUpdatePackageManifestAsync(string packageVersion, int timeout = 60,
            string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(DefaultPackageName)
                : YooAssets.GetPackage(customPackageName);
            return package?.UpdatePackageManifestAsync(packageVersion, timeout);
        }

        #endregion

        #region LowMemory

        private Action<bool> m_forceUnloadUnusedAssetsAction;

        public void OnLowMemory()
        {
            Debugger.Warning("Low memory reported...");
            m_forceUnloadUnusedAssetsAction?.Invoke(true);
        }

        public void SetForceUnloadUnusedAssetsAction(Action<bool> action)
        {
            m_forceUnloadUnusedAssetsAction = action;
        }

        #endregion
    }
}