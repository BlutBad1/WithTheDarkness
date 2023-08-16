using HudNS;
using MyConstants;
using UnityEngine;
namespace InteractableNS.Usable
{
    public class Lock : Interactable
    {
        public Animator Animator;
        public bool IsLocked = true;
        public string LockedMessage;
        public string UnlockedMessage;
        public float DisapperingSpeed;
        public void OpenLock() =>
            IsLocked = false;
        public void CloseLock() =>
            IsLocked = true;
        protected override void Interact()
        {
            Animator.SetTrigger(EnironmentConstants.Lock.LOCK_ANIMATOR_TRIGGER);
            Animator.SetBool(EnironmentConstants.Lock.LOCK_ANIMATOR_BOOL, IsLocked);
            if (!IsLocked)
                GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage(UnlockedMessage, DisapperingSpeed);
            else
                GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage(LockedMessage, DisapperingSpeed);
        }
    }
}