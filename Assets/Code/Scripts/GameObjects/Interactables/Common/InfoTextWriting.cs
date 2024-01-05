using Data.Text;
using HudNS;
using UnityEngine;
using UtilitiesNS;

namespace InteractableNS.Common
{
    public class InfoTextWriting : Interactable
    {
        [SerializeField]
        protected string message;
        public TextData Text;
        [SerializeField]
        protected float disapperingSpeed;
        protected override void Interact()
        {
            MessagePrint messagePrint = Utilities.GetComponentFromGameObject<MessagePrint>(LastWhoInteracted.gameObject);
            messagePrint.PrintMessage(Text ? Text.GetText() : message, disapperingSpeed);
        }
    }
}