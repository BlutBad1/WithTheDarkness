using HudNS;
using UnityEngine;
namespace InteractableNS.Common
{
    public class InfoTextWriting : Interactable
    {
        [SerializeField]
        private string message;
        [SerializeField]
        private float disapperingSpeed;
        protected override void Interact()=>
            GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage(message, disapperingSpeed);
    }
}