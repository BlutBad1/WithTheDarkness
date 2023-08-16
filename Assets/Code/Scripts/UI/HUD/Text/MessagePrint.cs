using MyConstants;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HudNS
{
    public class MessagePrint : MonoBehaviour
    {
        TextMeshProUGUI Showcaser;
        static MessagePrint instance;
        Dictionary<TextMeshProUGUI, Coroutine> coroutinesLeft = new Dictionary<TextMeshProUGUI, Coroutine>();
        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this); 
        }
        public void PrintMessage(string message, float disapperingSpeed, TextMeshProUGUI showcaser)
        {
            this.Showcaser = showcaser;
            printMessage(message, disapperingSpeed);
        }
        public void PrintMessage(string message, float disapperingSpeed, string showcaserName)
        {
            this.Showcaser = GameObject.Find(showcaserName).GetComponent<TextMeshProUGUI>();
            printMessage(message, disapperingSpeed);
        }
        public void PrintMessage(string message, float disapperingSpeed)
        {
            this.Showcaser = GameObject.Find(HUDConstants.TEXTSHOWER).GetComponent<TextMeshProUGUI>();
            printMessage(message, disapperingSpeed);
        }
        void printMessage(string message, float disapperingSpeed)
        {
            Showcaser.text = message;
            Showcaser.alpha = 1;
            if (!coroutinesLeft.ContainsKey(Showcaser))
              coroutinesLeft.Add(Showcaser, StartCoroutine(MessageDisappering(Showcaser, disapperingSpeed)));
            else
            {
                StopCoroutine(coroutinesLeft[Showcaser]);
                coroutinesLeft[Showcaser] = StartCoroutine(MessageDisappering(Showcaser, disapperingSpeed));
            }
        }
        IEnumerator MessageDisappering(TextMeshProUGUI ShowcaserCoroutine, float disapperingSpeed)
        {
            float tempAlpha = ShowcaserCoroutine.alpha;
            while (tempAlpha > 0)
            {
                ShowcaserCoroutine.color = new Color(ShowcaserCoroutine.color.r, ShowcaserCoroutine.color.g, ShowcaserCoroutine.color.b, tempAlpha);
                if (tempAlpha >= 0.1)
                    tempAlpha -= Time.deltaTime * disapperingSpeed;
                else
                    tempAlpha = 0;
                yield return null;
            }
            ShowcaserCoroutine.text = "";
        }
    }
}