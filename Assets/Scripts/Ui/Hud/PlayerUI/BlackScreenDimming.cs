using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HudNS
{


    public class BlackScreenDimming : MonoBehaviour
    {
        public float fadeSpeed = 0.2f;
        public Image blackScreen;
        Coroutine followCoroutine;
        private void Awake()
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
        }
        private void Start()
        {
            DimmingDisable();
        }

        public void DimmingEnable()
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(Dimming(true));

        }
        public void DimmingDisable()
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(Dimming(false));
        }
        IEnumerator Dimming(bool enable)
        {
            float timeElapsed = 0f;
            float tempAlpha = blackScreen.color.a;
            while (tempAlpha <= 1 && enable || tempAlpha >= 0 && !enable)
            {
                blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, tempAlpha);
                timeElapsed += Time.deltaTime;
                if (enable)
                    tempAlpha += Time.deltaTime * fadeSpeed;
                else
                    tempAlpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }

        }
    }
}