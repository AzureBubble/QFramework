using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace GameLogic.UIModule.Utility
{
    public class UIImageEditor
    {
        [MenuItem("GameObject/UI/UI Image", priority = 33)]
        public static void CreateTextPro()
        {
            // 创建 UI Text 物体
            GameObject root = new GameObject("UI Image", typeof(RectTransform), typeof(UIImage));
            // 设置 UI Text 作为 Canvas 的子物体
            UnityEditorUtility.ResetInCanvasFor((RectTransform)root.transform);
        }
    }
}

#endif