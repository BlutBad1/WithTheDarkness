using UnityEngine;

public class DisableCollidersOnDeath : MonoBehaviour
{
    public Collider[] colliders;
    public Damageable damageable;
    private void Start()
    {
        if (!damageable)
            TryGetComponent(out damageable);
        if (damageable)
            damageable.OnDeath += DisableColliders;


    }

    public void DisableColliders()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}