using UnityEditor;
using UnityEngine;

namespace ChainEditor
{
#if UNITY_EDITOR
    public class EditorStyleHelper
    {
        public static void DrawFrames(Color frameColor, Rect boxRect)
        {
            EditorGUI.DrawRect(new Rect(boxRect.x, boxRect.y, boxRect.width, 1), frameColor);

            EditorGUI.DrawRect(new Rect(boxRect.x, boxRect.yMax - 1, boxRect.width, 1), frameColor);

            EditorGUI.DrawRect(new Rect(boxRect.x, boxRect.y, 1, boxRect.height), frameColor);

            EditorGUI.DrawRect(new Rect(boxRect.xMax - 1, boxRect.y, 1, boxRect.height), frameColor);
        }
        
        public static void DrawSeparatorLine(Color lineColor)
        {
            Rect lineRect = EditorGUILayout.GetControlRect(false, 2);
            lineRect.height = 1;

            Color originalBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = lineColor;

            EditorGUI.DrawRect(lineRect, lineColor);
            GUI.backgroundColor = originalBackgroundColor;
        }
    }

}
#endif
