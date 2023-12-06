using InteractableNS.Usable;
using UnityEngine;

namespace PortalNS
{
    public class ConnectedDoors : MonoBehaviour
    {
        public Door FirstDoor;
        public Door SecondDoor;
        private void Start()
        {
            FirstDoor.OnOpenDoorEvent.AddListener(DoorOpen);
            SecondDoor.OnOpenDoorEvent.AddListener(DoorOpen);
            FirstDoor.OnCloseDoorEvent.AddListener(DoorClose);
            SecondDoor.OnCloseDoorEvent.AddListener(DoorClose);
        }
        public void DoorOpen()
        {
            if (!FirstDoor.IsOpened)
                FirstDoor.OpenDoor();
            else if (!SecondDoor.IsOpened)
                SecondDoor.OpenDoor();
        }
        public void DoorClose()
        {
            if (FirstDoor.IsOpened)
                FirstDoor.CloseDoor();
            else if (SecondDoor.IsOpened)
                SecondDoor.CloseDoor();
        }
    }
}