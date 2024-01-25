using InteractableNS.Usable.Locking;
using MyConstants.EnironmentConstants.DecorConstants;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace InteractableNS.Usable
{
    public class Door : Lock
    {
        [SerializeField, FormerlySerializedAs("Animator")]
        private Animator animator;
        [SerializeField, FormerlySerializedAs("IsOpened")]
        private bool isOpened = false;
        [SerializeField, FormerlySerializedAs("PlayEventsOnStart")]
        private bool playEventsOnStart = false;
        [SerializeField, FormerlySerializedAs("OnOpenDoorEvent")] //When you open a door
        private UnityEvent onOpenDoorEvent;
        [SerializeField, FormerlySerializedAs("OnCloseDoorEvent")] //When you close a door
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
}