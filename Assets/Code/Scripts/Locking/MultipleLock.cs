using InteractableNS.Usable.Locking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LockingLogic
{
    public class MultipleLock : MonoBehaviour
    {
        public List<Lock> Locks;
        public UnityEvent OnAllLocksOpened;
        private void Start()
        {
            foreach (Lock _lock in Locks)
                _lock.OnUnlockEvent?.AddListener(OnLocksUnLock);
        }
        public void OnLocksUnLock()
        {
            foreach (Lock _lock in Locks)
            {
                if (_lock.IsLocked)
                    return;
            }
            OnAllLocksOpened?.Invoke();
        }
    }
}
