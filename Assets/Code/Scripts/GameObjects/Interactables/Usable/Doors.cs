using InteractableNS.Usable.Locking;
using MyConstants;
using MyConstants.EnironmentConstants.DecorConstants;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableNS.Usable
{
    [RequireComponent(typeof(Animator))]
    public class Doors : Lock
    {
        public Animator Animator;
        public bool IsOpened = false;
        public bool PlayEventsOnStart = false;
        [HideInInspector] //"When you open a door
        public UnityEvent OnOpenDoorEvent;
        [HideInInspector] //When you close a door
        public UnityEvent OnCloseDoorEvent;
        [HideInInspector] //When you lock a door
        public UnityEvent OnLockEvent;
        private bool isPreviouslyLocked;
        private bool isCanPlayEvents;
        private new void Start()
        {
            base.Start();
            isCanPlayEvents = PlayEventsOnStart;
            isPreviouslyLocked = IsLocked;
            if (IsOpened)
                OpenDoors();
            else
                CloseDoors();
            isCanPlayEvents = true;
        }
        public void OpenDoors()
        {
            IsLocked = false;
            isPreviouslyLocked = false;
            IsOpened = true;
            SetAnimatorValues(false, IsLocked); //Open Doors
            if (isCanPlayEvents)
                OnOpenDoorEvent?.Invoke();
        }
        public void CloseDoors()
        {
            IsOpened = false;
            SetAnimatorValues(true, IsLocked); //Close Doors
            if (isCanPlayEvents)
                OnCloseDoorEvent?.Invoke();
        }
        public override void OpenLock()
        {
            base.OpenLock();
            isPreviouslyLocked = false;
            PrintMessage(UnlockMessage);
            OnUnlockEvent?.Invoke();
        }
        public override void CloseLock()
        {
            base.CloseLock();
            if (IsOpened)
                CloseDoors();
            isPreviouslyLocked = true;
            OnLockEvent?.Invoke();
        }
        public void SetAnimatorValues(bool IsOpened, bool IsLocked)
        {
            Animator.SetBool(DoorsConstants.DOORS_ANIMATOR_IS_OPENED, IsOpened);
            Animator.SetBool(DoorsConstants.DOORS_ANIMATOR_IS_LOCKED, IsLocked);
            Animator.SetTrigger(DoorsConstants.DOORS_ANIMATOR_TRIGGER);
        }
        protected override void Interact()
        {
            if (CheckIfDataHasKey())
                IsLocked = false;
            if (isPreviouslyLocked && !IsLocked)
            {
                OpenLock();
                return;
            }
            else if (!isPreviouslyLocked && IsLocked)
            {
                CloseLock();
                return;
            }
            else
            {
                if (IsLocked)
                {
                    SetAnimatorValues(IsOpened, IsLocked);
                    PrintMessage(LockedMessage);
                    OnLockedEvent?.Invoke();
                }
                else
                {
                    if (IsOpened)
                        CloseDoors();
                    else
                        OpenDoors();
                }
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Doors))]
    public class DoorsCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            Doors script = (Doors)target;
            SerializedProperty property;
            //Locking Data
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locking Data", EditorStyles.boldLabel);
            script.IsGeneric = EditorGUILayout.Toggle("IsGeneric", script.IsGeneric);
            if (!script.IsGeneric) // if bool is true, show other fields
            {
                property = serializedObject.FindProperty("KeyName");
                EditorGUILayout.PropertyField(property, new GUIContent("KeyName"), true);
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
            property = serializedObject.FindProperty("IsLocked");
            EditorGUILayout.PropertyField(property, new GUIContent("IsLocked"), true);
            property = serializedObject.FindProperty("LockedMessage");
            EditorGUILayout.PropertyField(property, new GUIContent("LockedMessage"), true);
            property = serializedObject.FindProperty("UnlockedMessage");
            EditorGUILayout.PropertyField(property, new GUIContent("UnlockedMessage"), true);
            property = serializedObject.FindProperty("DisapperingSpeed");
            EditorGUILayout.PropertyField(property, new GUIContent("DisapperingSpeed"), true);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            //Door class fields
            property = serializedObject.FindProperty("OnOpenDoorEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("OnOpenDoorEvent"), true);
            property = serializedObject.FindProperty("OnCloseDoorEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("OnCloseDoorEvent"), true);
            property = serializedObject.FindProperty("OnUnlockEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("OnUnlockEvent"), true);
            property = serializedObject.FindProperty("OnLockEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("OnLockEvent"), true);
            property = serializedObject.FindProperty("OnLockedEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("OnLockedEvent"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}