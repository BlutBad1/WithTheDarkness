using UnityEngine;

namespace GameObjectsControllingNS
{
    public class ResetDefaultPosition : MonoBehaviour
    {
        Vector3 DefaultPosition;
        public GameObject GameObject;
        private void Start()
        {
            if (!GameObject)
                GameObject = gameObject;
            DefaultPosition = GameObject.transform.position;
        }
        public void ResetPosition() =>
            GameObject.transform.position = DefaultPosition;
    }
}