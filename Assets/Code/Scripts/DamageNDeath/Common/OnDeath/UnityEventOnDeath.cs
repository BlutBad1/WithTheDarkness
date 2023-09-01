using UnityEngine;
using UnityEngine.Events;

namespace OnDeath
{
    [RequireComponent(typeof(Damageable))]
    public class UnityEventOnDeath : MonoBehaviour
    {
        public UnityEvent UnityEvent;
        private void Start()
        {
            GetComponent<Damageable>().OnDeath += InvokeUnityEvent;
        }
        private void OnDisable()
        {
            GetComponent<Damageable>().OnDeath -= InvokeUnityEvent;
        }
        public void InvokeUnityEvent() =>
            UnityEvent?.Invoke();
    }
}
