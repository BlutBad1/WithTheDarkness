using ScriptableObjectNS.Locking;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace InteractableNS.Usable.Locking
{
    public class Lock : Interactable, ILockingInteractable
    {
        [SerializeField, HideInInspector]
        private bool addKeyToRegularRequired = true;
        [SerializeField]
        private KeyData requiredKey;
        [SerializeField, FormerlySerializedAs("IsLocked"), HideInInspector]
        private bool isLocked = false;
        [SerializeField, FormerlySerializedAs("OnUnlockEvent"), HideInInspector]
        private UnityEvent onUnlockEvent;
        [SerializeField, HideInInspector]
        private UnityEvent onLockEvent; //When you lock a door
        [SerializeField, FormerlySerializedAs("OnLockedEvent"), HideInInspector]
        private UnityEvent onLockedEvent;

        protected KeysOnLevelManager keysOnLevelManager;

        private bool prevLockedState;

        public virtual bool IsLocked
        {
            get => isLocked;
            protected set
            {
                prevLockedState = IsLocked;
                isLocked = value;
            }
        }
        public UnityEvent OnUnlockEvent { get => onUnlockEvent; }
        public UnityEvent OnLockedEvent { get => onLockedEvent; }

        protected override void Start()
        {
            base.Start();
            keysOnLevelManager = KeysOnLevelManager.Instance;
            if (addKeyToRegularRequired)
                keysOnLevelManager.AddKeyToRegularRequired(requiredKey);
            prevLockedState = IsLocked;
        }
        public virtual void OpenLock()
        {
            OpenLockWithoutEvent();
            onUnlockEvent?.Invoke();
        }
        public virtual void CloseLock()
        {
            CloseLockWithoutEvent();
            onLockEvent?.Invoke();
        }
        public void OpenLockWithoutEvent()
        {
            isLocked = false;
            prevLockedState = false;
        }
        public void CloseLockWithoutEvent()
        {
            isLocked = true;
            prevLockedState = true;
        }
        public KeyData GetKeyData() =>
            requiredKey;

        public void SetKeyData(KeyData keyData) =>
           requiredKey = keyData;
        protected override void Interact()
        {
            SetLockState();
            LockStateChange();
        }
        protected virtual void SetLockState()
        {
            if (IsLocked)
            {
                IsLocked = !keysOnLevelManager.HaveKeyInAvailable(requiredKey);
                keysOnLevelManager.RemoveKeyFromAvailable(requiredKey);
            }
        }
        private void LockStateChange()
        {
            if (IsLocked != prevLockedState)
            {
                if (prevLockedState && !IsLocked)
                    OpenLock();
                else if (!prevLockedState && IsLocked)
                    CloseLock();
            }
            else if (IsLocked)
                onLockedEvent?.Invoke();
        }
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
            property = serializedObject.FindProperty("addKeyToRegularRequired");
            EditorGUILayout.PropertyField(property, new GUIContent("Add Key ToRegular Required"), true);
            property = serializedObject.FindProperty("isLocked");
            EditorGUILayout.PropertyField(property, new GUIContent("Is Locked"), true);
            //Locking Settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locking Events", EditorStyles.boldLabel);
            property = serializedObject.FindProperty("onUnlockEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("On Unlock Event"), true);
            property = serializedObject.FindProperty("onLockEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("On Lock Event"), true);
            property = serializedObject.FindProperty("onLockedEvent");
            EditorGUILayout.PropertyField(property, new GUIContent("On Locked Event"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}