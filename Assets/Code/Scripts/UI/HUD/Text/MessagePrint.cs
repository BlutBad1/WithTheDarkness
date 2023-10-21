using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HudNS
{
    public class MessagePrint : MonoBehaviour
    {
        public TextMeshProUGUI DefaultShowcaser;
        private TextMeshProUGUI showcaser;
        Dictionary<TextMeshProUGUI, Coroutine> coroutinesLeft = new Dictionary<TextMeshProUGUI, Coroutine>();
        public void PrintMessage(string message, float disapperingSpeed, TextMeshProUGUI showcaser)
        {
            this.showcaser = showcaser;
            printMessage(message, disapperingSpeed);
        }
        public void PrintMessage(string message, float disapperingSpeed)
        {
            this.showcaser = DefaultShowcaser;
            printMessage(message, disapperingSpeed);
        }
        void printMessage(string message, float disapperingSpeed)
        {
            showcaser.text = message;
            showcaser.alpha = 1;
            if (!coroutinesLeft.ContainsKey(showcaser))
                coroutinesLeft.Add(showcaser, StartCoroutine(MessageDisappering(showcaser, disapperingSpeed)));
            else
            {
                StopCoroutine(coroutinesLeft[showcaser]);
                coroutinesLeft[showcaser] = StartCoroutine(MessageDisappering(showcaser, disapperingSpeed));
            }
        }
        IEnumerator MessageDisappering(TextMeshProUGUI ShowcaserCoroutine, float disapperingSpeed)
        {
            float tempAlpha = ShowcaserCoroutine.alpha;
            while (tempAlpha > 0)
            {
                ShowcaserCoroutine.color = new Color(ShowcaserCoroutine.color.r, ShowcaserCoroutine.color.g, ShowcaserCoroutine.color.b, tempAlpha);
                if (tempAlpha >= 0.01)
                    tempAlpha -= Time.deltaTime * disapperingSpeed;
                else
                    tempAlpha = 0;
                yield return null;
            }
            ShowcaserCoroutine.text = "";
        }
    }
}