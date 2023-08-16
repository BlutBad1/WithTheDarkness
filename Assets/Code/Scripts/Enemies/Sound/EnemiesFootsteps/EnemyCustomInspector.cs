using UnityEditor;
using UnityEngine;
namespace EnemySoundNS
{
#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyFootsteps))]
    public class EnemyCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Rect rect = new Rect();
            SerializedProperty surfaceEnemyManager = serializedObject.FindProperty("surfaceEnemyManager");
            EditorGUILayout.LabelField("DataBase Settings", new GUIStyle() { fontStyle = FontStyle.BoldAndItalic });
            EditorGUILayout.PropertyField(surfaceEnemyManager);
            SerializedProperty distanceBetweenSteps = serializedObject.FindProperty("distanceBetweenSteps");
            rect = EditorGUILayout.GetControlRect();
            EditorGUI.indentLevel++;
            // Distance between steps
            EditorGUI.PropertyField(rect, distanceBetweenSteps);
            for (int i = 0; i < 2; i++) EditorGUILayout.Space();
            EditorGUI.indentLevel -= 2;
            SerializedProperty Agent = serializedObject.FindProperty("Agent");
            SerializedProperty audioSource = serializedObject.FindProperty("audioSource");
            SerializedProperty minVolume = serializedObject.FindProperty("minVolume");
            SerializedProperty maxVolume = serializedObject.FindProperty("maxVolume");
            SerializedProperty landVolume = serializedObject.FindProperty("landVolume");
            SerializedProperty debugMode = serializedObject.FindProperty("debugMode");
            SerializedProperty groundCheckHeight = serializedObject.FindProperty("groundCheckHeight");
            SerializedProperty groundCheckRadius = serializedObject.FindProperty("groundCheckRadius");
            SerializedProperty groundCheckDistance = serializedObject.FindProperty("groundCheckDistance");
            SerializedProperty groundLayers = serializedObject.FindProperty("groundLayers");
            EditorGUILayout.LabelField("General Settings", new GUIStyle() { fontStyle = FontStyle.BoldAndItalic });
            rect = EditorGUILayout.GetControlRect();
            //Agent
            EditorGUI.PropertyField(rect, Agent);
            // Audio Source
            rect.y = rect.yMax + 2;
            EditorGUI.PropertyField(rect, audioSource);
            // Min Volume
            rect.y = rect.yMax + 2;
            EditorGUI.PropertyField(rect, minVolume);
            // Max Volume
            rect.y = rect.yMax + 2;
            EditorGUI.PropertyField(rect, maxVolume);
            // Land Volume
            rect.y = rect.yMax + 2;
            EditorGUI.PropertyField(rect, landVolume);
            for (int i = 0; i < 14; i++) EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ground Check Settings", new GUIStyle() { fontStyle = FontStyle.BoldAndItalic });
            rect = EditorGUILayout.GetControlRect();
            // Debug Mode
            EditorGUI.PropertyField(rect, debugMode);
            // Ground Check Height
            rect.y = rect.yMax + 2;
            EditorGUI.PropertyField(rect, groundCheckHeight);
            // Ground Check Radius
            rect.y = rect.yMax + 2;
            EditorGUI.PropertyField(rect, groundCheckRadius);
            // Ground Check Distance
            rect.y = rect.yMax + 2;
            EditorGUI.PropertyField(rect, groundCheckDistance);
            // Ground Layers
            rect.y = rect.yMax + 2;
            EditorGUI.PropertyField(rect, groundLayers);
            for (int i = 0; i < 12; i++) EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
    // Custom property inspector of type 'RegisteredMaterial'
    [CustomPropertyDrawer(typeof(EnemyRegisteredMaterial))]
    public class MaterialDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty texture = property.FindPropertyRelative("texture");
            SerializedProperty surfaceIndex = property.FindPropertyRelative("surfaceIndex");
            SerializedProperty attachedSurfaces = property.FindPropertyRelative("attachedSurfaces");
            string[] attachedSurfacesNames = new string[attachedSurfaces.arraySize];
            for (int x = 0; x < attachedSurfaces.arraySize; x++)
                attachedSurfacesNames[x] = attachedSurfaces.GetArrayElementAtIndex(x).stringValue;
            // Showing labels for the fields
            position.y -= 12f;
            GUI.Label(position, "Texture");
            position.x = (position.width / 2f) + 50f;
            GUI.Label(position, "GroundType");
            // Set the new rect 
            position.height = 16f;
            position.y = position.yMax + 12f;
            position.x = 48f;
            position.width /= 2.2f;
            // Draw the texture field
            EditorGUI.PropertyField(position, texture, GUIContent.none);
            // Draw the type field
            position.x = position.xMax + 15f;
            position.y -= 2f;
            surfaceIndex.intValue = EditorGUI.Popup(position, surfaceIndex.intValue, attachedSurfacesNames);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 32f; }
    }
#endif
}