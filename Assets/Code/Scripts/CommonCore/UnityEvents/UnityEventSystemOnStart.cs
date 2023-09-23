namespace UnityEventNS
{
    public class UnityEventSystemOnStart : UnityEventSystem
    {
        void Start() =>
            InvokeEvents();
    }
}
