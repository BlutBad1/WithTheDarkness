using HudNS;
using UnityEngine;
using UnityEngine.Serialization;

namespace InteractableNS.Common
{
    [RequireComponent(typeof(Interactable))]
    public class WriteInfoByInteractable : Interactable
    {
        [SerializeField, FormerlySerializedAs("Message")]
        private string message;
        [SerializeField, FormerlySerializedAs("DisapperingSpeed")]
        private float disapperingSpeed;

        protected override void Interact()
        {
            WriteInformationMessage();
        }
        private void WriteInformationMessage()
        {
            MessagePrint messagePrint = UtilitiesNS.Utilities.GetComponentFromGameObject<MessagePrint>(lastWhoInteracted.gameObject);
            if (messagePrint)
                messagePrint.PrintMessage(message, disapperingSpeed);
        }
    }
}