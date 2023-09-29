using HudNS;
using UnityEngine;
namespace InteractableNS.Common
{
    public class WriteInfo : MonoBehaviour
    {
        [SerializeField]
        public string Message;
        [SerializeField]
        public float DisapperingSpeed;
        [Tooltip("Name of showcaser. If null than default showcaser will show the message.")]
        public string ShowcaserName;
        public void WriteInformationMessage()
        {
            if (!string.IsNullOrEmpty(ShowcaserName))
                GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage(Message, DisapperingSpeed, showcaserName: ShowcaserName);
            else
                GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage(Message, DisapperingSpeed);
        }
    }
}