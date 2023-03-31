using HudNS;
using MyConstants;
using UnityEngine;
namespace InteractableNS.Common
{

    public class InfoTextWriting : Interactable
    {
        [SerializeField]
        private string message;
        [SerializeField]
        private float disapperingSpeed;


        protected override void Interact()
        {

            GameObject.Find(CommonConstants.TEXTSHOWER).GetComponent<MessagePrint>().PrintMessage(message, disapperingSpeed);

        }

    }
}