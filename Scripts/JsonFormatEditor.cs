#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JsonFormat))]
public class JsonFormatEditor : Editor
{
    JsonFormat jsonFormat;
    private void OnEnable()
    {
        jsonFormat = target as JsonFormat;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(nameof(jsonFormat.Format),GUILayout.Width(50));
        Undo.RecordObject(target, "Undo Description");
        jsonFormat.Format = EditorGUILayout.TextArea(jsonFormat.Format,GUILayout.MinHeight(700));
        GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif