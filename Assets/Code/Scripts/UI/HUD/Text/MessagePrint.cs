using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace HudNS
{
	public class MessagePrint : MonoBehaviour, IMessagePrinter
	{
		[SerializeField, FormerlySerializedAs("DefaultShowcaser")]
		private TextMeshProUGUI showcaser;
		[SerializeField]
		private float disapperingSpeed = 0.8f;

		private Coroutine currentCoroutine;

		public void PrintMessage(string message, GameObject fromGO)
		{
			showcaser.text = message;
			showcaser.alpha = 1;
			if (currentCoroutine != null)
				StopCoroutine(currentCoroutine);
			currentCoroutine = StartCoroutine(MessageDisappering(showcaser, disapperingSpeed));
		}
		private IEnumerator MessageDisappering(TextMeshProUGUI showcaser, float disapperingSpeed)
		{
			float tempAlpha = showcaser.alpha;
			while (tempAlpha > 0)
			{
				showcaser.color = new Color(showcaser.color.r, showcaser.color.g, showcaser.color.b, tempAlpha);
				if (tempAlpha >= 0.01)
					tempAlpha -= Time.deltaTime * disapperingSpeed;
				else
					tempAlpha = 0;
				yield return null;
			}
			showcaser.text = "";
		}
	}
}