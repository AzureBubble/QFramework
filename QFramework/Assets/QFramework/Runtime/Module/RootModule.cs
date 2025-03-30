using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    [DisallowMultipleComponent]
    public class RootModule : MonoBehaviour
    {
        private static RootModule m_instance = null;

        public static RootModule Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<RootModule>();
                }
                return m_instance;
            }
        }

        private void Awake()
        {
            GameTime.StartFrame();
        }

        private void Update()
        {
            GameTime.StartFrame();
            ModuleSystem.OnUpdate(GameTime.DeltaTime, GameTime.UnscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            GameTime.StartFrame();
        }

        private void LateUpdate()
        {
            GameTime.StartFrame();
        }
    }
}