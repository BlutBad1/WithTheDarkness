using HudNS;
using ScriptableObjectNS.Locking;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableNS.Usable.Locking
{
    public class Lock : Interactable, ISerializationCallbackReceiver
    {
        [HideInInspector]
        public bool IsGeneric = false;
        [HideInInspector]
        public static List<string> LockingTypes;
        [HideInInspector, ListToPopup(typeof(Lock), "LockingTypes")]
        public string GenericKeyName;
        [HideInInspector]
        public string KeyName;
        [HideInInspector]
        public KeyInteractable ConnectedKey;
        [HideInInspector]
        public AvailableKeyData AvailableKeyData;
        [HideInInspector]
        public bool IsLocked = true;
        [HideInInspector]
        public string LockedMessage;
        [HideInInspector]
        public string UnlockMessage;
        [HideInInspector]
        public float DisapperingSpeed;
        [HideInInspector]
        public UnityEvent OnUnlockEvent;
        [HideInInspector]
        public UnityEvent OnLockedEvent;
        public virtual void OpenLock() =>
            IsLocked = false;
        public virtual void CloseLock() =>
            IsLocked = true;

        public virtual bool CheckIfDataHasKey()
        {
            if (IsGeneric)
            {
                Key key = AvailableKeyData.AvailableKeys.Find(x => x.IsGeneric && x.KeyName == GenericKeyName);
                if (key != null)
                {
                    AvailableKeyData.AvailableKeys.Remove(key);
                    return true;
                }
            }
            else
            {
                Key key = AvailableKeyData.AvailableKeys.Find(x => x.KeyName == KeyName);
                if (key != null)
                    return true;
            }
            return false;
        }
        public void OnBeforeSerialize() =>
            LockingTypes = LockingTypeData.Instance.LockingTypes;
        public void OnAfterDeserialize()
        {
        }
        protected override void Interact()
        {
            if (CheckIfDataHasKey())
                OpenLock();
            if (!IsLocked)
            {
                PrintMessage(UnlockMessage);
                OnUnlockEvent?.Invoke();
            }
            else
            {
                PrintMessage(LockedMessage);
                OnLockedEvent?.Invoke();
            }
        }
        public void PrintMessage(string message) =>
            GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage(message, DisapperingSpeed);
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Lock))]
    public class LockCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            Lock script = (Lock)target;
            SerializedProperty property;
            //Locking Data
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locking Data", EditorStyles.boldLabel);
            script.IsGeneric = EditorGUILayout.Toggle("IsGeneric", script.IsGeneric);
            if (!script.IsGeneric) // if bool is true, show other fields
            {
                script.KeyName = EditorGUILayout.TextField("KeyName", script.KeyName);
                property = serializedObject.FindProperty("ConnectedKey");
                EditorGUILayout.PropertyField(property, new GUIContent("ConnectedKey"));
                if (script.ConnectedKey != null)
                    script.KeyName = script.ConnectedKey.Key.KeyName;
            }
            else
            {
                property = serializedObject.FindProperty("GenericKeyName");
                EditorGUILayout.PropertyField(property, new GUIContent("KeyName"), true);
            }
            property = serializedObject.FindProperty("AvailableKeyData");
            EditorGUILayout.PropertyField(property, new GUIContent("AvailableKeyData"), true);
            //Locking Settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locking Settings", EditorStyles.boldLabel);
            script.IsLocked = EditorGUILayout.Toggle("IsLocked", script.IsLocked);
            script.LockedMessage = EditorGUILayout.TextField("LockedMessage", script.LockedMessage);
            script.UnlockMessage = EditorGUILayout.TextField("UnlockedMessage", script.UnlockMessage);
            script.DisapperingSpeed = EditorGUILayout.FloatField("DisapperingSpeed", script.DisapperingSpeed);
            EditorGUILayout.Space();
            property = serializedObject.FindProperty("OnUnlockEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("OnUnlockEvent"), true);
            property = serializedObject.FindProperty("OnLockedEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("OnLockedEvent"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}