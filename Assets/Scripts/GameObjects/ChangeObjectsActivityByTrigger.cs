using UnityEngine;

namespace GameObjectsControllingNS
{
    public class ChangeObjectsActivityByTrigger : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] gameObjects;
        public bool IsEnabledAfterTrigger = true;
        private void Start()
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.activeInHierarchy == isActiveAndEnabled)
                    gameObject.SetActive(!IsEnabledAfterTrigger);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == MyConstants.CommonConstants.PLAYER)
            {
                foreach (var gameObject in gameObjects)
                    gameObject.SetActive(IsEnabledAfterTrigger);
                Destroy(gameObject);
            }
        }
    }
}