using GameObjectsConstantsNS.UsableConstants;
using InteractableNS.Usable.Locking;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace InteractableNS.Usable
{
    public class Door : Lock
    {
        [SerializeField, FormerlySerializedAs("Animator"), HideInInspector]
        private Animator animator;
        [SerializeField, FormerlySerializedAs("IsOpened"), HideInInspector]
        private bool isOpened = false;
        [SerializeField, FormerlySerializedAs("PlayEventsOnStart"), HideInInspector]
        private bool playEventsOnStart = false;
        [SerializeField, FormerlySerializedAs("OnOpenDoorEvent"), HideInInspector] //When you open a door
        private UnityEvent onOpenDoorEvent;
        [SerializeField, FormerlySerializedAs("OnCloseDoorEvent"), HideInInspector] //When you close a door
        private UnityEvent onCloseDoorEvent;

        public bool IsOpened { get => isOpened; }
        public UnityEvent OnOpenDoorEvent { get => onOpenDoorEvent; }
        public UnityEvent OnCloseDoorEvent { get => onCloseDoorEvent; }
        public override bool IsLocked
        {
            get => base.IsLocked;
            protected set
            {
                base.IsLocked = value;
                if (IsLocked && IsOpened)
                    CloseDoor();
            }
        }

        protected override void Start()
        {
            base.Start();
            SetDoorStartCondition();
        }
        public void OpenDoor()
        {
            if (!IsOpened)
            {
                OpenDoorWithoutEvents();
                onOpenDoorEvent?.Invoke();
            }
        }
        public void CloseDoor()
        {
            if (isOpened)
            {
                CloseDoorWithoutEvents();
                onCloseDoorEvent?.Invoke();
            }
        }
        public void OpenDoorWithoutEvents()
        {
            isOpened = true;
            OpenLockWithoutEvent();
            SetAnimatorValues(wasOpened: false, isLocked: false); //Open Doors
        }
        public void CloseDoorWithoutEvents()
        {
            isOpened = false;
            SetAnimatorValues(wasOpened: true, isLocked: false); //Close Doors
        }
        protected override void Interact()
        {
            bool wasLocked = IsLocked;
            base.Interact();
            if (wasLocked && !IsLocked)
                return;
            if (!IsLocked)
            {
                SetDoorCondition();
                return;
            }
            SetAnimatorValues(isOpened, IsLocked);
            OnLockedEvent?.Invoke();
        }
        private void SetDoorStartCondition()
        {
            if (!playEventsOnStart)
            {
                if (isOpened)
                    OpenDoorWithoutEvents();
                else
                    CloseDoorWithoutEvents();
            }
            else
                SetDoorCondition();
        }
        private void SetDoorCondition()
        {
            if (isOpened)
                CloseDoor();
            else
                OpenDoor();
        }
        private void SetAnimatorValues(bool wasOpened, bool isLocked)
        {
            animator.SetBool(DoorsConstants.DOORS_ANIMATOR_IS_OPENED, wasOpened);
            animator.SetBool(DoorsConstants.DOORS_ANIMATOR_IS_LOCKED, isLocked);
            animator.SetTrigger(DoorsConstants.DOORS_ANIMATOR_TRIGGER);
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
            property = serializedObject.FindProperty("animator");
            EditorGUILayout.PropertyField(property, new GUIContent("Animator"), true);
            property = serializedObject.FindProperty("addKeyToRegularRequired");
            EditorGUILayout.PropertyField(property, new GUIContent("Add Key To Regular Required"), true);
            property = serializedObject.FindProperty("isLocked");
            EditorGUILayout.PropertyField(property, new GUIContent("Is Locked"), true);
            property = serializedObject.FindProperty("isOpened");
            EditorGUILayout.PropertyField(property, new GUIContent("Is Opened"), true);
            property = serializedObject.FindProperty("playEventsOnStart");
            EditorGUILayout.PropertyField(property, new GUIContent("Play Events OnStart"), true);
            //Locking Settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locking Events", EditorStyles.boldLabel);
            property = serializedObject.FindProperty("onUnlockEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("On Unlock Event"), true);
            property = serializedObject.FindProperty("onLockEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("On Lock Event"), true);
            property = serializedObject.FindProperty("onLockedEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("On Locked Event"), true);
            property = serializedObject.FindProperty("onOpenDoorEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("On Open Door Event"), true);
            property = serializedObject.FindProperty("onCloseDoorEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("On Close Door Event"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}