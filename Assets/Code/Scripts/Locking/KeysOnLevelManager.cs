using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjectNS.Locking
{
    [Serializable]
    public class RequiredKey : Key
    {
        [HideInInspector]
        public static List<string> LockingTypes;
        [ListToPopup(typeof(RequiredKey), "LockingTypes", "Gen Key Name")]
        public string GenericKeyName;
        [Min(0)]
        public int Amount;
    }
    public class KeysOnLevelManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        public RequiredKey[] RequiredKeys;
        public bool SetKeysOnStart = false;
        public List<Key> AvailableKeysOnStart;
        public AvailableKeyData AvailableKeyData;
        public static KeysOnLevelManager Instance;
        public void OnAfterDeserialize()
        {
        }
        public void OnBeforeSerialize()
        {
            RequiredKey.LockingTypes = LockingTypeData.Instance?.LockingTypes;
        }
        private void OnEnable()
        {
            if (!Instance)
                Instance = this;
            else if (Instance != this)
                Destroy(this);
            if (!AvailableKeyData)
                Debug.LogWarning("AvailableKeyData is not set!");
            if (SetKeysOnStart && AvailableKeyData)
                AvailableKeyData.AvailableKeys = AvailableKeysOnStart;
        }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequiredKey))]
    public class RequiredKeyDrawer : PropertyDrawer
    {
        private int fieldAmount = 3;
        private float fieldSize = 25;
        private float padding = 2;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool isFoldedOut = property.isExpanded;
            if (isFoldedOut)
                return (EditorGUIUtility.singleLineHeight * fieldAmount) + fieldSize;
            return EditorGUIUtility.singleLineHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), property.isExpanded, label);
            if (property.isExpanded)
            {
                // Draw properties based on their current values.
                SerializedProperty isGenericProp = property.FindPropertyRelative("IsGeneric");
                SerializedProperty keyNameProp = property.FindPropertyRelative("KeyName");
                SerializedProperty genericKeyNameProp = property.FindPropertyRelative("GenericKeyName");
                SerializedProperty amountProp = property.FindPropertyRelative("Amount");
                EditorGUI.indentLevel++;
                position.y += EditorGUIUtility.singleLineHeight + padding;
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), isGenericProp);
                position.y += EditorGUIUtility.singleLineHeight + padding;
                if (isGenericProp.boolValue)
                    EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), genericKeyNameProp);
                else
                    EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), keyNameProp);
                position.y += EditorGUIUtility.singleLineHeight + padding;
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), amountProp);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }
#endif
}