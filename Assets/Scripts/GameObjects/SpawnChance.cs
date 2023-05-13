using UnityEngine;

namespace GameObjectsControllingNS
{
    public class SpawnChance : MonoBehaviour
    {
        public float chance;
        private void Start()
        {
            if (new System.Random().Next() % 100 > chance)
                Destroy(gameObject);
        }
    }
}