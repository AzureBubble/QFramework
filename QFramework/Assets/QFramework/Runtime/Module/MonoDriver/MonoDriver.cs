using System;
using System.Collections;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Internal;
using Object = UnityEngine.Object;

namespace QFramework
{
    internal class MonoDriver : Module, IMonoDriver
    {
        private GameObject m_gameObject;
        private MainBehaviour m_behaviour;

        public override void OnInit()
        {
            _MakeEntity();
        }

        private void _MakeEntity()
        {
            if (m_gameObject != null)
            {
                return;
            }

            m_gameObject = new GameObject("[MonoDriver]");
            m_gameObject.SetActive(true);
            Object.DontDestroyOnLoad(m_gameObject);
            m_behaviour = m_gameObject.AddComponent<MainBehaviour>();
        }

        public override void OnDispose()
        {
            m_behaviour?.OnRelease();

            if (m_gameObject != null)
            {
                Object.Destroy(m_gameObject);
            }

            m_gameObject = null;
            m_behaviour = null;
        }

        #region 控制协程Coroutine

        public Coroutine StartCoroutine(string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                return null;
            }

            _MakeEntity();
            return m_behaviour.StartCoroutine(methodName);
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            if (routine == null)
            {
                return null;
            }

            _MakeEntity();
            return m_behaviour.StartCoroutine(routine);
        }

        public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                return null;
            }

            _MakeEntity();
            return m_behaviour.StartCoroutine(methodName, value);
        }

        public void StopCoroutine(string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                return;
            }

            m_behaviour?.StopCoroutine(methodName);
        }

        public void StopCoroutine(IEnumerator routine)
        {
            if (routine == null)
            {
                return;
            }
            m_behaviour?.StopCoroutine(routine);
        }

        public void StopCoroutine(Coroutine routine)
        {
            if (routine == null)
            {
                return;
            }
            m_behaviour?.StopCoroutine(routine);
        }

        public void StopAllCoroutines()
        {
            m_behaviour?.StopAllCoroutines();
        }

        #endregion

        #region 注入UnityUpdate/FixedUpdate/LateUpdate

        /// <summary>
        /// 为给外部提供的 添加帧更新事件。
        /// </summary>
        /// <param name="action"></param>
        public void AddUpdateListener(Action action)
        {
            _MakeEntity();
            AddUpdateListenerImp(action).Forget();
        }

        private async UniTaskVoid AddUpdateListenerImp(Action action)
        {
            await UniTask.Yield();
            m_behaviour.AddUpdateListener(action);
        }

        /// <summary>
        /// 为给外部提供的 添加物理帧更新事件。
        /// </summary>
        /// <param name="action"></param>
        public void AddFixedUpdateListener(Action action)
        {
            _MakeEntity();
            AddFixedUpdateListenerImp(action).Forget();
        }

        private async UniTaskVoid AddFixedUpdateListenerImp(Action action)
        {
            await UniTask.Yield(PlayerLoopTiming.LastEarlyUpdate);
            m_behaviour.AddFixedUpdateListener(action);
        }

        /// <summary>
        /// 为给外部提供的 添加Late帧更新事件。
        /// </summary>
        /// <param name="action"></param>
        public void AddLateUpdateListener(Action action)
        {
            _MakeEntity();
            AddLateUpdateListenerImp(action).Forget();
        }

        private async UniTaskVoid AddLateUpdateListenerImp(Action action)
        {
            await UniTask.Yield(PlayerLoopTiming.LastEarlyUpdate);
            m_behaviour.AddLateUpdateListener(action);
        }

        /// <summary>
        /// 移除帧更新事件。
        /// </summary>
        /// <param name="action"></param>
        public void RemoveUpdateListener(Action action)
        {
            _MakeEntity();
            m_behaviour.RemoveUpdateListener(action);
        }

        /// <summary>
        /// 移除物理帧更新事件。
        /// </summary>
        /// <param name="action"></param>
        public void RemoveFixedUpdateListener(Action action)
        {
            _MakeEntity();
            m_behaviour.RemoveFixedUpdateListener(action);
        }

        /// <summary>
        /// 移除Late帧更新事件。
        /// </summary>
        /// <param name="action"></param>
        public void RemoveLateUpdateListener(Action action)
        {
            _MakeEntity();
            m_behaviour.RemoveLateUpdateListener(action);
        }

        #endregion

        #region Unity Events 注入

        /// <summary>
        /// 为给外部提供的Destroy注册事件。
        /// </summary>
        /// <param name="action"></param>
        public void AddDestroyListener(Action action)
        {
            _MakeEntity();
            m_behaviour.AddDestroyListener(action);
        }

        /// <summary>
        /// 为给外部提供的Destroy反注册事件。
        /// </summary>
        /// <param name="action"></param>
        public void RemoveDestroyListener(Action action)
        {
            _MakeEntity();
            m_behaviour.RemoveDestroyListener(action);
        }

        /// <summary>
        /// 为给外部提供的OnDrawGizmos注册事件。
        /// </summary>
        /// <param name="action"></param>
        public void AddOnDrawGizmosListener(Action action)
        {
            _MakeEntity();
            m_behaviour.AddOnDrawGizmosListener(action);
        }

        /// <summary>
        /// 为给外部提供的OnDrawGizmos反注册事件。
        /// </summary>
        /// <param name="action"></param>
        public void RemoveOnDrawGizmosListener(Action action)
        {
            _MakeEntity();
            m_behaviour.RemoveOnDrawGizmosListener(action);
        }

        /// <summary>
        /// 为给外部提供的OnDrawGizmosSelected注册事件。
        /// </summary>
        /// <param name="action"></param>
        public void AddOnDrawGizmosSelectedListener(Action action)
        {
            _MakeEntity();
            m_behaviour.AddOnDrawGizmosSelectedListener(action);
        }

        /// <summary>
        /// 为给外部提供的OnDrawGizmosSelected反注册事件。
        /// </summary>
        /// <param name="action"></param>
        public void RemoveOnDrawGizmosSelectedListener(Action action)
        {
            _MakeEntity();
            m_behaviour.RemoveOnDrawGizmosSelectedListener(action);
        }

        /// <summary>
        /// 为给外部提供的OnApplicationPause注册事件。
        /// </summary>
        /// <param name="action"></param>
        public void AddOnApplicationPauseListener(Action<bool> action)
        {
            _MakeEntity();
            m_behaviour.AddOnApplicationPauseListener(action);
        }

        /// <summary>
        /// 为给外部提供的OnApplicationPause反注册事件。
        /// </summary>
        /// <param name="action"></param>
        public void RemoveOnApplicationPauseListener(Action<bool> action)
        {
            _MakeEntity();
            m_behaviour.RemoveOnApplicationPauseListener(action);
        }

        #endregion

        private class MainBehaviour : MonoBehaviour
        {
            private event Action UpdateEvent;
            private event Action FixedUpdateEvent;
            private event Action LateUpdateEvent;
            private event Action DestroyEvent;
            private event Action OnDrawGizmosEvent;
            private event Action OnDrawGizmosSelectedEvent;
            private event Action<bool> OnApplicationPauseEvent;

            void Update()
            {
                UpdateEvent?.Invoke();
            }

            void FixedUpdate()
            {
                FixedUpdateEvent?.Invoke();
            }

            void LateUpdate()
            {
                LateUpdateEvent?.Invoke();
            }

            private void OnDestroy()
            {
                DestroyEvent?.Invoke();
            }

            [Conditional("UNITY_EDITOR")]
            private void OnDrawGizmos()
            {
                OnDrawGizmosEvent?.Invoke();
            }

            [Conditional("UNITY_EDITOR")]
            private void OnDrawGizmosSelected()
            {
                OnDrawGizmosSelectedEvent?.Invoke();
            }

            private void OnApplicationPause(bool pauseStatus)
            {
                OnApplicationPauseEvent?.Invoke(pauseStatus);
            }

            public void AddLateUpdateListener(Action action)
            {
                LateUpdateEvent += action;
            }

            public void RemoveLateUpdateListener(Action action)
            {
                LateUpdateEvent -= action;
            }

            public void AddFixedUpdateListener(Action action)
            {
                FixedUpdateEvent += action;
            }

            public void RemoveFixedUpdateListener(Action action)
            {
                FixedUpdateEvent -= action;
            }

            public void AddUpdateListener(Action action)
            {
                UpdateEvent += action;
            }

            public void RemoveUpdateListener(Action action)
            {
                UpdateEvent -= action;
            }

            public void AddDestroyListener(Action action)
            {
                DestroyEvent += action;
            }

            public void RemoveDestroyListener(Action action)
            {
                DestroyEvent -= action;
            }

            [Conditional("UNITY_EDITOR")]
            public void AddOnDrawGizmosListener(Action action)
            {
                OnDrawGizmosEvent += action;
            }

            [Conditional("UNITY_EDITOR")]
            public void RemoveOnDrawGizmosListener(Action action)
            {
                OnDrawGizmosEvent -= action;
            }

            [Conditional("UNITY_EDITOR")]
            public void AddOnDrawGizmosSelectedListener(Action action)
            {
                OnDrawGizmosSelectedEvent += action;
            }

            [Conditional("UNITY_EDITOR")]
            public void RemoveOnDrawGizmosSelectedListener(Action action)
            {
                OnDrawGizmosSelectedEvent -= action;
            }

            public void AddOnApplicationPauseListener(Action<bool> action)
            {
                OnApplicationPauseEvent += action;
            }

            public void RemoveOnApplicationPauseListener(Action<bool> action)
            {
                OnApplicationPauseEvent -= action;
            }

            public void OnRelease()
            {
                UpdateEvent = null;
                FixedUpdateEvent = null;
                LateUpdateEvent = null;
                OnDrawGizmosEvent = null;
                OnDrawGizmosSelectedEvent = null;
                DestroyEvent = null;
                OnApplicationPauseEvent = null;
            }
        }
    }
}