using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatingJoystick))]
public class FloatingJoystickEditor : JoystickEditor
{
    private SerializedProperty hideWhenNotUse;
    private SerializedProperty backToStartPosition;
    private SerializedProperty displayStartup;

    protected override void OnEnable()
    {
        base.OnEnable();

        hideWhenNotUse = serializedObject.FindProperty("hideWhenNotUse");
        backToStartPosition = serializedObject.FindProperty("_backToStartPosition");
        displayStartup = serializedObject.FindProperty("_displayStartup");
    }

    protected override void DrawValues()
    {
        base.DrawValues();
        EditorGUILayout.PropertyField(hideWhenNotUse, new GUIContent("Hide graphics when not using"));
        EditorGUILayout.PropertyField(backToStartPosition, new GUIContent("If we need back Joystick to start"));
        EditorGUILayout.PropertyField(displayStartup, new GUIContent("Display joystick at startup"));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (background != null)
        {
            RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
            backgroundRect.anchorMax = Vector2.zero;
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.pivot = center;
        }
    }
}