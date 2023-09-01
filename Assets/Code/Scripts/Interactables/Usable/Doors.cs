using MyConstants;
using OptimizationNS;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableNS.Usable
{
    public class Doors : Interactable
    {
        public Animator Animator;
        public bool IsLocked = false;
        public bool IsOpened = false;
        public bool PlayEventsOnStart = false;
        [Tooltip("When you open a door")]
        public UnityEvent OnOpenDoorEvent;
        [Tooltip("When you close a door")]
        public UnityEvent OnCloseDoorEvent;
        [Tooltip("When you try open a door, but it's locked")]
        public UnityEvent OnLockedDoorEvent;
        [Tooltip("When you unlock a door")]
        public UnityEvent OnUnlockEvent;
        [Tooltip("When you lock a door")]
        public UnityEvent OnLockEvent;
        bool isPreviouslyLocked = false;
        bool isStart = true;
        private bool unlockOnInteracte;
        private bool lockOnInteracte;
        public bool UnlockOnInteracte
        {
            get { return unlockOnInteracte; }
            set { unlockOnInteracte = value; if (UnlockOnInteracte) LockOnInteracte = false; }
        }
        public bool LockOnInteracte
        {
            get { return lockOnInteracte; }
            set { lockOnInteracte = value; if (LockOnInteracte) UnlockOnInteracte = false; }
        }
        private new void Start()
        {
            base.Start();
            if (PlayEventsOnStart)
                isStart = false;
            isPreviouslyLocked = IsLocked;
            if (IsOpened == true)
                OpenDoors();
            else
                CloseDoors();
            isStart = false;
        }
        public void OpenDoors()
        {
            IsLocked = false;
            IsOpened = true;
            CheckIfPreviouslyLocked();
            SetAnimatorValues(false, IsLocked); //Open Doors
            if (!isStart)
                OnOpenDoorEvent?.Invoke();
        }
        public void CloseDoors()
        {
            IsOpened = false;
            SetAnimatorValues(true, IsLocked); //Close Doors
            if (!isStart)
                OnCloseDoorEvent?.Invoke();
        }
        public void LockDoors()
        {
            if (IsOpened)
                CloseDoors();
            IsLocked = true;
            CheckIfPreviouslyLocked();
        }
        public void UnlockDoors()
        {
            IsLocked = false;
            CheckIfPreviouslyLocked();
        }
        private void CheckIfPreviouslyLocked()
        {
            if (isPreviouslyLocked && !IsLocked)
                OnUnlockEvent?.Invoke();
            else if (!isPreviouslyLocked && IsLocked)
                OnLockEvent?.Invoke();
            isPreviouslyLocked = IsLocked;
        }
        public void SetAnimatorValues(bool IsOpened, bool IsLocked)
        {
            Animator.SetBool(EnironmentConstants.Doors.DOORS_ANIMATOR_IS_OPENED, IsOpened);
            Animator.SetBool(EnironmentConstants.Doors.DOORS_ANIMATOR_IS_LOCKED, IsLocked);
            Animator.SetTrigger(EnironmentConstants.Doors.DOORS_ANIMATOR_TRIGGER);
        }
        protected override void Interact()
        {
            if (LockOnInteracte)
            {
                LockDoors();
                return;
            }
            if (UnlockOnInteracte)
            {
                UnlockDoors();
                return;
            }
            if (IsLocked)
            {
                SetAnimatorValues(IsOpened, IsLocked);
                OnLockedDoorEvent?.Invoke();
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