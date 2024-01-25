using InteractableNS.Usable;
using UnityEngine;
using UnityEngine.Serialization;

namespace LocationConnector
{
    public class Connector : MonoBehaviour
    {
        public static Connector Instance { get; private set; }

        [SerializeField, FormerlySerializedAs("OriginGameObject")]
        private GameObject originGameObject;
        [SerializeField, FormerlySerializedAs("EnterRoom")]
        private GameObject enterRoom;
        [SerializeField, FormerlySerializedAs("ExitRoom")]
        private GameObject exitRoom;
        [SerializeField, FormerlySerializedAs("Doors")]
        private Door[] doors;

        public GameObject OriginGameObject { get => originGameObject; }
        public GameObject EnterRoom { get => enterRoom; }
        public GameObject ExitRoom { get => exitRoom; }

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
            foreach (Door door in doors)
                door.CloseDoorWithoutEvents();
        }
    }
}