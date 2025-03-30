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
        public string CurrentMainSceneName { get; }

        private string m_currentMainSceneName = string.Empty;

        private SceneHandle m_currentMainScene;

        private readonly Dictionary<string, SceneHandle> m_subScenes = new Dictionary<string, SceneHandle>();
        private readonly HashSet<string> m_handlingScene = new HashSet<string>();

        public override void OnInit()
        {
            m_currentMainScene = null;
            m_currentMainSceneName = SceneManager.GetSceneByBuildIndex(0).name;
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
            m_currentMainSceneName = string.Empty;
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
                // Log.Error($"Could not load scene while loading. Scene: {location}");
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
                if (m_currentMainScene is { IsDone: false })
                {
                    throw new Exception($"Could not load MainScene while loading. CurrentMainScene: {m_currentMainSceneName}.");
                }

                m_currentMainSceneName = location;

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

                // ModuleSystem.GetModule<IResourceModule>().ForceUnloadUnusedAssets(gcCollect);

                m_handlingScene.Remove(location);

                return m_currentMainScene.SceneObject;
            }
        }

        public void LoadScene(string location, LoadSceneMode sceneMode = LoadSceneMode.Single, bool suspendLoad = false,
            uint priority = 100, Action<Scene> callBack = null, bool gcCollect = true, Action<float> progressCallBack = null)
        {
            throw new NotImplementedException();
        }

        public bool ActivateScene(string location)
        {
            throw new NotImplementedException();
        }

        public bool UnSuspend(string location)
        {
            throw new NotImplementedException();
        }

        public bool IsMainScene(string location)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> UnloadAsync(string location, Action<float> progressCallBack = null)
        {
            throw new NotImplementedException();
        }

        public void Unload(string location, Action callBack = null, Action<float> progressCallBack = null)
        {
            throw new NotImplementedException();
        }

        public bool IsContainScene(string location)
        {
            throw new NotImplementedException();
        }
    }
}