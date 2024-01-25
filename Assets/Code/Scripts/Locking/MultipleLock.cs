using InteractableNS.Usable.Locking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace LockingLogic
{
    public class MultipleLock : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("Locks")]
        private List<Lock> locks;
        [SerializeField, FormerlySerializedAs("OnAllLocksOpened")]
        private UnityEvent onAllLocksOpened;

        private void Start()
        {
            AddEventToLocks();
        }
        private void AddEventToLocks()
        {
            foreach (Lock _lock in locks)
                _lock.OnUnlockEvent?.AddListener(OnLocksUnLock);
        }
        private void OnLocksUnLock()
        {
            foreach (Lock _lock in locks)
            {
                if (_lock.IsLocked)
                    return;
            }
            onAllLocksOpened?.Invoke();
        }
    }
}
