using HudNS;
using UnityEngine;
using UtilitiesNS;

namespace InteractableNS.Common
{
    public class InfoTextWriting : Interactable
    {
        [SerializeField]
        protected string message;
        [SerializeField]
        protected float disapperingSpeed;
        protected override void Interact()
        {
            MessagePrint messagePrint = Utilities.GetComponentFromGameObject<MessagePrint>(LastWhoInteracted.gameObject);
            if (messagePrint)
                messagePrint.PrintMessage(message, disapperingSpeed);
        }
    }
}