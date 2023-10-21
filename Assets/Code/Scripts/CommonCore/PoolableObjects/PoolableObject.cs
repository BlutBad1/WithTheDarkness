using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    [HideInInspector]
    public ObjectPool ParentObjectPool;
    public virtual void OnDisable()
    {
        ParentObjectPool.ReturnObjectToPool(this.gameObject);
    }
}