using InteractableNS.Usable;
using UnityEngine;
namespace LocationConnector
{
    public class Connector : MonoBehaviour
    {
        public GameObject OriginGameObject;
        public GameObject EnterRoom;
        public GameObject ExitRoom;
        public Door[] Doors;
        public static Connector Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        public void CloseDoors()
        {
            foreach (Door door in Doors)
                door.CloseDoorWithoutEvents();
        }
    }
}