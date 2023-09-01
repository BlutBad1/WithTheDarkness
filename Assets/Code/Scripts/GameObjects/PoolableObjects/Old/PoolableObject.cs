using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    [HideInInspector]
    public ObjectPool Parent;

    public virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this.gameObject);
    }
}