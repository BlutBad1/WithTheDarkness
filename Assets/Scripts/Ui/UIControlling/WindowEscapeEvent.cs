using UIControlling;
using UnityEngine;
using UnityEngine.Events;

public class WindowEscapeEvent : MonoBehaviour
{
    public UnityEvent UnityEvent;
    private void OnEnable()
    {
        WindowEscape.OnEscape += EventInvoke;
    }
    private void OnDisable()
    {
        WindowEscape.OnEscape -= EventInvoke;
    }
    public virtual void EventInvoke() =>
        UnityEvent?.Invoke();
}
