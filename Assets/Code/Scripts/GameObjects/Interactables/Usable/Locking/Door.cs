using InteractableNS.Usable.Locking;
using MyConstants.EnironmentConstants.DecorConstants;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableNS.Usable
{
    public class Door : Lock
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
        private new void Start()
        {
            base.Start();
            isPreviouslyLocked = IsLocked;

            if (IsOpened)
            {
                switch (PlayEventsOnStart)
                {
                    case true:
                        OpenDoor();
                        break;
                    case false:
                        OpenDoorWithoutEvents();
                        break;
                }
            }
            else if (!IsOpened && Animator.GetBool("IsOpened"))
            {
                switch (PlayEventsOnStart)
                {
                    case true:
                        CloseDoor();
                        break;
                    case false:
                        CloseDoorWithoutEvents();
                        break;
                }
            }
        }
        public void OpenDoor()
        {
            OpenDoorWithoutEvents();
            OnOpenDoorEvent?.Invoke();
        }
        public void OpenDoorWithoutEvents()
        {
            IsLocked = false;
            isPreviouslyLocked = false;
            IsOpened = true;
            SetAnimatorValues(false, IsLocked); //Open Doors
        }
        public void CloseDoor()
        {
            CloseDoorWithoutEvents();
            OnCloseDoorEvent?.Invoke();
        }
        public void CloseDoorWithoutEvents()
        {
            IsOpened = false;
            SetAnimatorValues(true, IsLocked); //Close Doors
        }
        public override void OpenLockWithoutEvents()
        {
            base.OpenLockWithoutEvents();
            isPreviouslyLocked = false;
            PrintMessage(UnlockMessage?.GetText());
        }
        public override void CloseLock()
        {
            CloseLockWithoutEvents();
            OnLockEvent?.Invoke();
        }
        public void CloseLockWithoutEvents()
        {
            base.CloseLock();
            if (IsOpened)
                CloseDoor();
            isPreviouslyLocked = true;
        }
        public void SetAnimatorValues(bool IsOpened, bool IsLocked)
        {
            Animator.SetBool(DoorsConstants.DOORS_ANIMATOR_IS_OPENED, IsOpened);
            Animator.SetBool(DoorsConstants.DOORS_ANIMATOR_IS_LOCKED, IsLocked);
            Animator.SetTrigger(DoorsConstants.DOORS_ANIMATOR_TRIGGER);
        }
        protected override void Interact()
        {
            if (IsLocked && CheckIfDataHasKey())
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
                    PrintMessage(LockedMessage?.GetText());
                    OnLockedEvent?.Invoke();
                }
                else
                {
                    if (IsOpened)
                        CloseDoor();
                    else
                        OpenDoor();
                }
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Door))]
    public class DoorsCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            Door script = (Door)target;
            SerializedProperty property;
            //Locking Data
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locking Data", EditorStyles.boldLabel);
            property = serializedObject.FindProperty("IsGeneric");
            EditorGUILayout.PropertyField(property, new GUIContent("IsGeneric"), true);
            if (!script.IsGeneric) // if bool is false, show other fields
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
            property = serializedObject.FindProperty("UnlockMessage");
            EditorGUILayout.PropertyField(property, new GUIContent("UnlockMessage"), true);
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