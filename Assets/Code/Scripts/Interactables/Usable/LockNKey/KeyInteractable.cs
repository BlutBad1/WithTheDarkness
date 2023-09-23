using ScriptableObjectNS.Locking;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InteractableNS.Usable.Locking
{
    public class KeyInteractable : Interactable, ISerializationCallbackReceiver
    {
        [HideInInspector]
        public bool IsGeneric = false;
        [HideInInspector]
        public string KeyName;
        [HideInInspector]
        public static List<string> LockingTypes;
        [HideInInspector, ListToPopup(typeof(KeyInteractable), "LockingTypes")]
        public string GenericKeyName;
        [HideInInspector]
        public Key Key = new Key();
        [HideInInspector]
        public AvailableKeyData AvailableKeyData;
        protected override void Interact()
        {
            base.Interact();
            if (!IsGeneric && AvailableKeyData.AvailableKeys.Find(x => x.KeyName == Key.KeyName) == null)
                AvailableKeyData.AvailableKeys.Add(Key);
            else if (IsGeneric)
                AvailableKeyData.AvailableKeys.Add(Key);
        }
        private List<string> lockingTypes;
        public void OnBeforeSerialize()
        {
            LockingTypes = LockingTypeData.Instance.LockingTypes;
        }

        public void OnAfterDeserialize()
        {
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(KeyInteractable))]
    public class KeyInteractableCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            KeyInteractable script = (KeyInteractable)target;
            SerializedProperty property;
            //Locking Data
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locking Data", EditorStyles.boldLabel);
            script.IsGeneric = EditorGUILayout.Toggle("IsGeneric", script.IsGeneric);
            if (!script.IsGeneric) // if bool is true, show other fields
            {
                script.KeyName = EditorGUILayout.TextField("KeyName", script.KeyName);
                script.Key.KeyName = script.KeyName;
                script.Key.IsGeneric = false;
            }
            else
            {
                property = serializedObject.FindProperty("GenericKeyName");
                EditorGUILayout.PropertyField(property, new GUIContent("KeyName"), true);
                script.Key.KeyName = script.GenericKeyName;
                script.Key.IsGeneric = true;
            }
            property = serializedObject.FindProperty("AvailableKeyData");
            EditorGUILayout.PropertyField(property, new GUIContent("AvailableKeyData"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
