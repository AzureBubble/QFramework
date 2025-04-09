using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace QFramework
{
    internal class SceneModule : Module, ISceneModule
    {
        /// <summary>
        /// 当前主场景名称。
        /// </summary>
        public string CurrentMainSceneName { get; private set; } = string.Empty;

        private SceneHandle m_currentMainScene;

        /// <summary>
        /// 所有的子场景Handle
        /// </summary>
        private readonly Dictionary<string, SceneHandle> m_subScenes = new Dictionary<string, SceneHandle>();

        /// <summary>
        /// 正在加载中的场景
        /// </summary>
        private readonly HashSet<string> m_handlingScene = new HashSet<string>();

        public override void OnInit()
        {
            m_currentMainScene = null;
            CurrentMainSceneName = SceneManager.GetSceneByBuildIndex(0).name;
        }

        public override void OnDispose()
        {
            var iter = m_subScenes.Values.GetEnumerator();
            while (iter.MoveNext())
            {
                SceneHandle subScene = iter.Current;
                if (subScene != null)
                {
                    subScene.UnloadAsync();
                }
            }

            iter.Dispose();
            m_subScenes.Clear();
            m_handlingScene.Clear();
            CurrentMainSceneName = string.Empty;
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="location">场景的定位地址</param>
        /// <param name="sceneMode">场景加载模式</param>
        /// <param name="suspendLoad">加载完毕时是否主动挂起</param>
        /// <param name="priority">优先级</param>
        /// <param name="gcCollect">加载主场景是否回收垃圾。</param>
        /// <param name="progressCallBack">加载进度回调。</param>
        public async UniTask<Scene> LoadSceneAsync(string location, LoadSceneMode sceneMode = LoadSceneMode.Single, bool suspendLoad = false, uint priority = 100, bool gcCollect = true, Action<float> progressCallBack = null)
        {
            if (!m_handlingScene.Add(location))
            {
                Debugger.Error($"Could not load scene while loading. Scene: {location}");
                return default;
            }

            if (sceneMode == LoadSceneMode.Additive)
            {
                if (m_subScenes.TryGetValue(location, out SceneHandle subScene))
                {
                    throw new Exception($"Could not load subScene while already loaded. Scene: {location}");
                }

                subScene = YooAssets.LoadSceneAsync(location, sceneMode, LocalPhysicsMode.None, suspendLoad, priority);

                if (progressCallBack != null)
                {
                    while (!subScene.IsDone && subScene.IsValid)
                    {
                        progressCallBack.Invoke(subScene.Progress);
                        await UniTask.Yield();
                    }
                }
                else
                {
                    await subScene.ToUniTask();
                }

                m_subScenes.TryAdd(location, subScene);

                m_handlingScene.Remove(location);

                return subScene.SceneObject;
            }
            else
            {
                if (m_currentMainScene != null && !m_currentMainScene.IsDone)
                {
                    throw new Exception($"Could not load MainScene while loading. CurrentMainScene: {CurrentMainSceneName}.");
                }

                CurrentMainSceneName = location;

                m_currentMainScene = YooAssets.LoadSceneAsync(location, sceneMode, LocalPhysicsMode.None, suspendLoad, priority);

                if (progressCallBack != null)
                {
                    while (!m_currentMainScene.IsDone && m_currentMainScene.IsValid)
                    {
                        progressCallBack.Invoke(m_currentMainScene.Progress);
                        await UniTask.Yield();
                    }
                }
                else
                {
                    await m_currentMainScene.ToUniTask();
                }

                ModuleSystem.GetModule<IResourcesModule>().ForceUnloadUnusedAssets(gcCollect);

                m_handlingScene.Remove(location);

                return m_currentMainScene.SceneObject;
            }
        }

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="location">场景的定位地址</param>
        /// <param name="sceneMode">场景加载模式</param>
        /// <param name="suspendLoad">加载完毕时是否主动挂起</param>
        /// <param name="priority">优先级</param>
        /// <param name="callBack">加载回调。</param>
        /// <param name="gcCollect">加载主场景是否回收垃圾。</param>
        /// <param name="progressCallBack">加载进度回调。</param>
        public void LoadScene(string location, LoadSceneMode sceneMode = LoadSceneMode.Single, bool suspendLoad = false,
            uint priority = 100, Action<Scene> callBack = null, bool gcCollect = true, Action<float> progressCallBack = null)
        {
            if (!m_handlingScene.Add(location))
            {
                Debugger.Error($"Could not load scene while loading. Scene: {location}");
                return;
            }

            if (sceneMode == LoadSceneMode.Additive)
            {
                if (m_subScenes.TryGetValue(location, out SceneHandle subScene))
                {
                    throw new Exception($"Could not load subScene while already loaded. Scene: {location}");
                    return;
                }
                subScene = YooAssets.LoadSceneAsync(location, sceneMode, LocalPhysicsMode.None, suspendLoad, priority);

                if (callBack != null)
                {
                    subScene.Completed += handle =>
                    {
                        m_handlingScene.Remove(location);
                        callBack?.Invoke(subScene.SceneObject);
                    };
                }

                if (progressCallBack != null)
                {
                    InvokeProgress(subScene, progressCallBack).Forget();
                }
                m_subScenes.TryAdd(location, subScene);
            }
            else
            {
                if (m_currentMainScene != null && !m_currentMainScene.IsDone)
                {
                    Debugger.Warning($"Could not load MainScene while loading. CurrentMainScene: {CurrentMainSceneName}.");
                    return;
                }

                CurrentMainSceneName = location;
                m_currentMainScene = YooAssets.LoadSceneAsync(location, sceneMode, LocalPhysicsMode.None, suspendLoad, priority);

                if (callBack != null)
                {
                    m_currentMainScene.Completed += handle =>
                    {
                        m_handlingScene.Remove(location);
                        callBack?.Invoke(m_currentMainScene.SceneObject);
                    };
                }

                if (progressCallBack != null)
                {
                    InvokeProgress(m_currentMainScene, progressCallBack).Forget();
                }

                ModuleSystem.GetModule<IResourcesModule>().ForceUnloadUnusedAssets(gcCollect);
            }
        }

        private async UniTaskVoid InvokeProgress(SceneHandle subScene, Action<float> progressCallBack)
        {
            if (subScene == null)
            {
                return;
            }

            while (!subScene.IsDone && subScene.IsValid)
            {
                await UniTask.Yield();
                progressCallBack?.Invoke(subScene.Progress);
            }
        }

        /// <summary>
        /// 激活场景（当同时存在多个场景时用于切换激活场景）。
        /// </summary>
        /// <param name="location">场景资源定位地址。</param>
        /// <returns>是否操作成功。</returns>
        public bool ActivateScene(string location)
        {
            if (CurrentMainSceneName.Equals(location))
            {
                if (m_currentMainScene != null)
                {
                    return m_currentMainScene.ActivateScene();
                }
                return false;
            }

            if (m_subScenes.TryGetValue(location, out SceneHandle subScene))
            {
                return subScene.ActivateScene();
            }
            Debugger.Warning($"IsMainScene invalid location:{location}");
            return false;
        }

        /// <summary>
        /// 解除场景加载挂起操作。
        /// </summary>
        /// <param name="location">场景资源定位地址。</param>
        /// <returns>是否操作成功。</returns>
        public bool UnSuspend(string location)
        {
            if (CurrentMainSceneName.Equals(location))
            {
                if (m_currentMainScene != null)
                {
                    return m_currentMainScene.UnSuspend();
                }
                return false;
            }

            if (m_subScenes.TryGetValue(location, out SceneHandle subScene))
            {
                return subScene.UnSuspend();
            }
            Debugger.Warning($"IsMainScene invalid location:{location}");
            return false;
        }

        /// <summary>
        /// 是否为主场景。
        /// </summary>
        /// <param name="location">场景资源定位地址。</param>
        /// <returns>是否主场景。</returns>
        public bool IsMainScene(string location)
        {
            var curScene = SceneManager.GetActiveScene();

            if (CurrentMainSceneName.Equals(location))
            {
                if (m_currentMainScene == null)
                {
                    return false;
                }

                // if (curScene.name.Equals(m_currentMainScene.SceneName))
                // {
                //     return true;
                // }

                return m_currentMainScene.SceneName == curScene.name;
            }

            if (curScene.name == m_currentMainScene?.SceneName)
            {
                return true;
            }

            Debugger.Warning($"IsMainScene invalid location:{location}");
            return false;
        }

        /// <summary>
        /// 异步卸载子场景。
        /// </summary>
        /// <param name="location">场景资源定位地址。</param>
        /// <param name="progressCallBack">进度回调。</param>
        public async UniTask<bool> UnloadAsync(string location, Action<float> progressCallBack = null)
        {
            if (m_subScenes.TryGetValue(location, out SceneHandle subScene))
            {
                if (subScene.SceneObject == default)
                {
                    Debugger.Error($"Could not unload Scene while not loaded. Scene: {location}");
                    return false;
                }

                if (!m_handlingScene.Add(location))
                {
                    Debugger.Warning($"Could not unload Scene while loading. Scene: {location}");
                    return false;
                }

                var operation = subScene.UnloadAsync();

                if (progressCallBack != null)
                {
                    while (!operation.IsDone && operation.Status != EOperationStatus.Failed)
                    {
                        progressCallBack?.Invoke(operation.Progress);
                        await UniTask.Yield();
                    }
                }
                else
                {
                    await operation.ToUniTask();
                }

                m_subScenes.Remove(location);
                m_handlingScene.Remove(location);
                return true;
            }
            Debugger.Warning($"UnloadAsync invalid location:{location}");
            return false;
        }

        /// <summary>
        /// 异步卸载子场景。
        /// </summary>
        /// <param name="location">场景资源定位地址。</param>
        /// <param name="callBack">卸载完成回调。</param>
        /// <param name="progressCallBack">进度回调。</param>
        public void Unload(string location, Action callBack = null, Action<float> progressCallBack = null)
        {
            if (m_subScenes.TryGetValue(location, out SceneHandle subScene))
            {
                if (subScene.SceneObject == default)
                {
                    Debugger.Error($"Could not unload Scene while not loaded. Scene: {location}");
                    return;
                }

                if (!m_handlingScene.Add(location))
                {
                    Debugger.Warning($"Could not unload Scene while loading. Scene: {location}");
                    return;
                }

                subScene.UnloadAsync();
                if (callBack != null)
                {
                    subScene.UnloadAsync().Completed += handle =>
                    {
                        m_subScenes.Remove(location);
                        m_handlingScene.Remove(location);
                        callBack?.Invoke();
                    };
                }

                if (progressCallBack != null)
                {
                    InvokeProgress(subScene, progressCallBack).Forget();
                }
            }

            Debugger.Warning($"UnloadAsync invalid location:{location}");
        }

        /// <summary>
        /// 是否包含场景。
        /// </summary>
        /// <param name="location">场景资源定位地址。</param>
        /// <returns>是否包含场景。</returns>
        public bool ContainScene(string location)
        {
            if (CurrentMainSceneName.Equals(location))
            {
                return true;
            }

            return m_subScenes.TryGetValue(location, out var _);
        }
    }
}