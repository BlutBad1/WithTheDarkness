using UnityEngine;
using UnityEngine.Events;

namespace UnityEventNS
{
    public class UnityEventSystem : MonoBehaviour
    {
        public UnityEvent Events;
        public void InvokeEvents() =>
            Events.Invoke();
        public void InvokeEventAfterTime(float afterTime = 1f) =>
            Invoke("InvokeEvents", afterTime);
    }
}