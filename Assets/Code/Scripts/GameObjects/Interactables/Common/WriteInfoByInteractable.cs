using HudNS;
using UnityEngine;
namespace InteractableNS.Common
{
    [RequireComponent(typeof(Interactable))]
    public class WriteInfoByInteractable : MonoBehaviour
    {
        [SerializeField]
        public string Message;
        [SerializeField]
        public float DisapperingSpeed;
        public void WriteInformationMessage()
        {
            MessagePrint messagePrint = UtilitiesNS.Utilities.GetComponentFromGameObject<MessagePrint>(GetComponent<Interactable>().LastWhoInteracted.gameObject);
            if (messagePrint)
                messagePrint.PrintMessage(Message, DisapperingSpeed);
        }
    }
}