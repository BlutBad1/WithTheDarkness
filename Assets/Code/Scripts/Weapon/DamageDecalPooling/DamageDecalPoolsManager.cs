using PoolableObjects.Constants;
using System;
using UnityEngine;

namespace PoolableObjectsNS
{
	public class DamageDecalPoolsManager : MonoBehaviour
	{
		[SerializeField]
		public DamageDecalData[] pools;
		static DamageDecalPoolsManager instance;
		static public DamageDecalPoolsManager GetInstance()
		{
			return instance;
		}
		void Start()
		{
			if (instance && instance != this)
				Destroy(this);
			instance = this;
			CreatePools();
			GameObject parentPoolableObject = GameObject.Find(PoolableObjectsConstants.POOLABLE_OBJECTS);
			foreach (var pool in pools)
			{
				GameObject poolGameObject = new GameObject(pool.Prefab + " Pool");
				if (parentPoolableObject != null)
					poolGameObject.transform.parent = parentPoolableObject.transform;
				CreateObjects(pool, poolGameObject);
			}
		}
		public void DisablePoolableObjects()
		{
			foreach (var pool in pools)
			{
				for (int i = 0; i < pool.Size; i++)
				{
					pool.poolableObjects[i].SetActive(false);
					pool.Iterator = 0;
				}
			}
		}
		public void DisablePoolableObjects(string objectType)
		{
			DamageDecalData pool = Array.Find(pools, pool => pool.Name == objectType);
			if (pool == null)
			{
#if UNITY_EDITOR
				Debug.Log($"Object \"{objectType}\" is not found!");
#endif
				return;
			}
			for (int i = 0; i < pool.Size; i++)
			{
				pool.poolableObjects[i].SetActive(false);
				pool.Iterator = 0;
			}
		}
		public GameObject GetObject(string objectType)
		{
			DamageDecalData pool = Array.Find(pools, pool => pool.Name == objectType);
			if (pool == null)
			{
#if UNITY_EDITOR
				Debug.Log($"Object \"{objectType}\" is not found!");
#endif
				return null;
			}
			if (pool.Iterator >= pool.Size)
				pool.Iterator = 0;
			GameObject instance = pool.poolableObjects[pool.Iterator];
			instance.SetActive(true);
			pool.Iterator++;
			return instance;
		}
		public GameObject GetPrefab(string objectType)
		{
			DamageDecalData pool = Array.Find(pools, pool => pool.Name == objectType);
			if (pool == null)
			{
#if UNITY_EDITOR
				Debug.Log($"Object \"{objectType}\" is not found!");
#endif
				return null;
			}
			return pool.Prefab;
		}
		private void CreateObjects(DamageDecalData poolableObject, GameObject parent)
		{
			for (int i = 0; i < poolableObject.Size; i++)
			{
				GameObject gameObject = Instantiate(poolableObject.Prefab, Vector3.zero, Quaternion.identity, parent.transform);
				gameObject.transform.parent = parent.transform;
				poolableObject.poolableObjects[i] = gameObject;
				poolableObject.poolableObjects[i].SetActive(false);
			}
		}
		private void CreatePools()
		{
			for (int i = 0; i < pools.Length; i++)
				pools[i].poolableObjects = new GameObject[pools[i].Size];
		}
		public string[] GetAllPoolNames()
		{
			string[] names = new string[pools.Length];
			for (int i = 0; i < pools.Length; i++)
				names[i] = pools[i].Name;
			return names;
		}
	}
}