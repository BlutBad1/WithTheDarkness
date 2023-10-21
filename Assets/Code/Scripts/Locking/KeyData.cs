using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjectNS.Locking
{
    [Serializable]
    public class KeyData : Key
    {
        [HideInInspector]
        public static List<string> LockingTypes;
        [ListToPopup(typeof(KeyData), "LockingTypes", "Gen Key Name")]
        public string GenericKeyName;
        [Min(0)]
        public int Amount = 1;
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(KeyData))]
    public class KeyDataDrawer : PropertyDrawer
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

