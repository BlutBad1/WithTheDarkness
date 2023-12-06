using UnityEngine;

namespace GameObjectsControllingNS.Spawner
{
    public class SpawnGameObjectOnTransform : MonoBehaviour
    {
        public GameObject GameObjectPrefab;
        public Transform SpawnTransform;
        public void SpawnGameObject(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject go = GameObject.Instantiate(GameObjectPrefab, SpawnTransform.position, SpawnTransform.rotation);
                go.SetActive(true);
            }
        }
    }
}