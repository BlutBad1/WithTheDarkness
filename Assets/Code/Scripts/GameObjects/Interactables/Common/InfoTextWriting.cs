using Data.Text;
using HUDConstantsNS;
using UnityEngine;
using UnityEngine.Serialization;

namespace InteractableNS.Common
{
	public class InfoTextWriting : Interactable
	{
		[SerializeField, FormerlySerializedAs("Text")]
		protected TextData text;

		protected IMessagePrinter messagePrint;

		protected override void Start()
		{
			GetMessagePrinter();
		}
		protected virtual void GetMessagePrinter()
		{
			messagePrint = HUDConstants.MessagePrinter;
		}
		protected override void Interact()
		{
			messagePrint.PrintMessage(text.GetText(), gameObject);
		}
	}
}