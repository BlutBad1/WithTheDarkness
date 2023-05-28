using UnityEngine;

namespace GameObjectsControllingNS
{
    public class ActiveObjectsByTrigger : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] gameObjects;
        void Start()
        {
            foreach (var gameObject in gameObjects)
                gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == MyConstants.CommonConstants.PLAYER)
            {
                foreach (var gameObject in gameObjects)
                    gameObject.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}