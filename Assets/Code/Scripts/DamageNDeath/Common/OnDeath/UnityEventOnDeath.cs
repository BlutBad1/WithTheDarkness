using DamageableNS;
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
            GetComponent<Damageable>().OnDead += InvokeUnityEvent;
        }
        private void OnDisable()
        {
            GetComponent<Damageable>().OnDead -= InvokeUnityEvent;
        }
        public void InvokeUnityEvent() =>
            UnityEvent?.Invoke();
    }
}
