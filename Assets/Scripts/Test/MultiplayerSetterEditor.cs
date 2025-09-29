using Testing;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MultiplayerSetter))]
public class MultiplayerSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Reference the SelectionManager script
        MultiplayerSetter setter = (MultiplayerSetter)target;

        // Create a button in the Inspector
        if (GUILayout.Button("Set Selections to Two"))
        {
            // Call the method when the button is clicked
            setter.SetSelectionsToTwo();
        }
    }
}