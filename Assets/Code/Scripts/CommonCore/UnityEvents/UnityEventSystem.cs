using UnityEngine;
using UnityEngine.Events;

namespace UnityEventNS
{
    public class UnityEventSystem : MonoBehaviour
    {
        public UnityEvent Events;
        public void InvokeEvents() =>
            Events.Invoke();
    }
}