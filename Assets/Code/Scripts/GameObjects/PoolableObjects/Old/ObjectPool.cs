using MyConstants;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject Prefab;
    private int Size;
    private List<GameObject> AvailableObjectsInPool;
    GameObject parent;
    private ObjectPool(GameObject Prefab, int Size)
    {
        this.Prefab = Prefab;
        this.Size = Size;
        AvailableObjectsInPool = new List<GameObject>();
    }
    public static ObjectPool FindObjectPoolByPrefab(GameObject Prefab)
    {
        GameObject parentPoolableObject = GameObject.Find(CommonConstants.POOLABLE_OBJECTS);
        GameObject gOPool = GameObject.Find($"{CommonConstants.POOLABLE_OBJECTS}/" + Prefab.name + " Pool");
        if (gOPool == null)
            return null;
        else
            return gOPool.GetComponent<ObjectPool>();
    }
    public static ObjectPool CreateInstance(GameObject Prefab, int Size)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);
        GameObject parentPoolableObject = GameObject.Find(CommonConstants.POOLABLE_OBJECTS);
        GameObject poolGameObject = new GameObject(Prefab.name + " Pool");
        poolGameObject.transform.parent = parentPoolableObject.transform;
        pool.parent = poolGameObject;
        pool.CreateObjects(poolGameObject);
        return pool;
    }
    private void CreateObjects(GameObject parent)
    {
        for (int i = 0; i < Size; i++)
            CreateGameObject(parent);
    }
    private GameObject CreateGameObject(GameObject parent)
    {
        GameObject poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
        PoolableObject poolObj = poolableObject.AddComponent<PoolableObject>();
        poolObj.Parent = this;
        poolableObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
        AvailableObjectsInPool.Add(poolableObject);
        return poolableObject;
    }
    public GameObject GetObject()
    {
        GameObject instance;
        if (AvailableObjectsInPool.Count > 0)
        {
            instance = AvailableObjectsInPool[0];
            AvailableObjectsInPool.RemoveAt(0);
        }
        else
            instance = CreateGameObject(parent);
        instance.gameObject.SetActive(true);
        return instance;
    }
    public void ReturnObjectToPool(GameObject Object)
    {
        AvailableObjectsInPool.Add(Object);
    }
}