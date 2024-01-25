using ScriptableObjectNS.Locking;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace InteractableNS.Usable.Locking
{
    public class Lock : Interactable
    {
        [SerializeField]
        private KeyData requiredKey;
        [SerializeField]
        private bool addKeyToRegularRequired = true;
        [SerializeField, FormerlySerializedAs("isLocked")]
        private bool isLocked = true;
        [SerializeField, FormerlySerializedAs("OnUnlockEvent")]
        private UnityEvent onUnlockEvent;
        [SerializeField]
        private UnityEvent onLockEvent; //When you lock a door
        [SerializeField, FormerlySerializedAs("OnLockedEvent")]
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
}