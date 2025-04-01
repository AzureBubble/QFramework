using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

namespace QFramework
{
    public sealed partial class ResourcesModule : Module, IResourcesModule
    {
        public override void OnInit()
        {
        }

        public void Initialize()
        {
        }

        public UniTask<InitializationOperation> InitPackage(string customPackageName)
        {
            throw new NotImplementedException();
        }

        public override void OnDispose()
        {
        }

        public string ApplicableGameVersion { get; }
        public int InternalResourceVersion { get; }
        public EPlayMode PlayMode { get; set; }
        public EncryptionType EncryptionType { get; set; }
        public bool UpdatableWhilePlaying { get; set; }
        public int DownloadingMaxNum { get; set; }
        public int FailedTryAgainCount { get; set; }
        public string DefaultPackageName { get; set; }
        public long Milliseconds { get; set; }

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
            throw new NotImplementedException();
        }

        #endregion

        #region 卸载资源相关

        public void UnloadUnusedAssets()
        {
            throw new NotImplementedException();
        }

        public void ForceUnloadAllAssets()
        {
            throw new NotImplementedException();
        }

        public void ForceUnloadUnusedAssets(bool performGCCollect)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 检查资源相关

        public HasAssetResult CheckAssetIsExist(string location, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public bool CheckLocationValid(string location, string packageName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 获取资源信息列表相关

        public AssetInfo[] GetAssetInfos(string resTag, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public AssetInfo[] GetAssetInfos(string[] tags, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public AssetInfo GetAssetInfo(string location, string packageName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 异步加载资源相关

        public void LoadAssetAsync(string location, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData,
            string packageName = "")
        {
            throw new NotImplementedException();
        }

        public void LoadAssetAsync(string location, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks,
            object userData, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public UniTaskVoid LoadAssetAsync<T>(string location, Action<T> callback, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        public UniTask<T[]> LoadSubAssetsAsync<T>(string location, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        public UniTask<T> LoadAssetAsync<T>(string location, CancellationToken cancellationToken = default, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        public UniTask<GameObject> LoadGameObjectAsync(string location, Transform parent = null, CancellationToken cancellationToken = default,
            string packageName = "")
        {
            throw new NotImplementedException();
        }

        public AssetHandle LoadAssetAsyncHandle<T>(string location, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 同步加载资源相关

        public T LoadAsset<T>(string location, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        public GameObject LoadGameObject(string location, Transform parent = null, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public T[] LoadSubAssetsSync<T>(string location, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        public AssetHandle LoadAssetSyncHandle<T>(string location, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Clear

        public ClearCacheFilesOperation ClearCacheFilesAsync(EFileClearMode clearMode = EFileClearMode.ClearUnusedBundleFiles,
            string customPackageName = "")
        {
            throw new NotImplementedException();
        }

        public void ClearAllBundleFiles(string customPackageName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CreateLoader

        public ResourceDownloaderOperation Downloader { get; set; }
        public ResourceDownloaderOperation CreateResourceDownloader(string customPackageName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region PackageVersion

        public string PackageVersion { get; set; }

        public string GetPackageVersion(string customPackageName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Request

        public RequestPackageVersionOperation RequestUpdatePackageVersionAsync(bool appendTimeTicks = false, int timeout = 60,
            string customPackageName = "")
        {
            throw new NotImplementedException();
        }

        public UpdatePackageManifestOperation RequestUpdatePackageManifestAsync(string packageVersion, int timeout = 60,
            string customPackageName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region LowMemory

        public void OnLowMemory()
        {
            throw new NotImplementedException();
        }

        public void SetForceUnloadUnusedAssetsAction(Action<bool> action)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}