using UnityEngine;
using UnityEngine.Events;

namespace UnityEventNS
{
    public class UnityEventOnTrigger : MonoBehaviour
    {
        public LayerMask TriggerableLayerMask;
        public UnityEvent EventOnTriggerEnter;
        public UnityEvent EventOnTriggerExit;
        public UnityEvent EventOnTriggerStay;
        private void OnTriggerEnter(Collider other)
        {
            if (TriggerableLayerMask == (TriggerableLayerMask | (1 << other.gameObject.layer)))
                EventOnTriggerEnter?.Invoke();
        }
        private void OnTriggerExit(Collider other)
        {
            if (TriggerableLayerMask == (TriggerableLayerMask | (1 << other.gameObject.layer)))
                EventOnTriggerExit?.Invoke();
        }
        private void OnTriggerStay(Collider other)
        {
            if (TriggerableLayerMask == (TriggerableLayerMask | (1 << other.gameObject.layer)))
                EventOnTriggerStay?.Invoke();
        }
    }
}