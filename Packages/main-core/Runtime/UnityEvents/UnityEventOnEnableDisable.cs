using UnityEngine;
using UnityEngine.Events;

namespace UnityEventNS
{
    public class UnityEventOnEnableDisable : MonoBehaviour
    {
        public UnityEvent EventOnEnable;
        public UnityEvent EventOnDisable;
        private void OnEnable() =>
            EventOnEnable?.Invoke();
        private void OnDisable() =>
            EventOnDisable?.Invoke();
    }
}