using UnityEngine;

public class ILastTouched : MonoBehaviour
{
    [HideInInspector]
    public Collider iLastEntered;
    [HideInInspector]
    public Collider iLastExited;
    void OnTriggerEnter(Collider col) =>
        iLastEntered = col;
    void OnTriggerExit(Collider col) =>
        iLastExited = col;
}