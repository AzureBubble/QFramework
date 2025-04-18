using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.UIModule.Utility
{
    [System.Serializable]
    public class BaseUIText : Text, IMeshModifier
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void ModifyMesh(Mesh mesh)
        {
        }

        public void ModifyMesh(VertexHelper verts)
        {
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
        }

#endif
    }
}