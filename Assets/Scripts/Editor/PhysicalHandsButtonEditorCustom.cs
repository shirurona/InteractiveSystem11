using Leap.PhysicalHands;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicalHandsButtonCustom), true)]
[CanEditMultipleObjects]
public class PhysicalHandsButtonEditorCustom : PhysicalHandsButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("buttonLongPressTime"), new GUIContent("Long Press Trigger Time", "Event triggered time be press by objects that time."));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OnButtonLongPress"), new GUIContent("Button LongPress Event", "Event triggered when the button long press."));
        serializedObject.ApplyModifiedProperties();
    }
}
