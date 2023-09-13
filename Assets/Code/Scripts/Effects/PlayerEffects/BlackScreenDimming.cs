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
        private void Awake() =>
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);

        private void Start() =>
            DimmingDisable();

        public void DimmingEnable(bool scaleFromDeltaTime = true, float waitTime = 0)
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(Dimming(true, scaleFromDeltaTime, waitTime));
        }
        public void DimmingDisable(bool scaleFromDeltaTime = true, float waitTime = 0)
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(Dimming(false, scaleFromDeltaTime, waitTime));
        }
        IEnumerator Dimming(bool enable, bool scaleFromDeltaTime = true, float waitTime = 0)
        {
            float tempAlpha = blackScreen.color.a;
            while (tempAlpha <= 1 && enable || tempAlpha >= 0 && !enable)
            {
                blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, tempAlpha);
                if (enable)
                {
                    if (scaleFromDeltaTime)
                        tempAlpha += Time.deltaTime * fadeSpeed;
                    else
                        tempAlpha += fadeSpeed;
                }
                else
                {
                    if (scaleFromDeltaTime)
                        tempAlpha -= Time.deltaTime * fadeSpeed;
                    else
                        tempAlpha -= fadeSpeed;
                }
                yield return scaleFromDeltaTime ? new WaitForSecondsRealtime(waitTime) : new WaitForSeconds(waitTime);
            }
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, enable ? 1f : 0f);
        }
    }
}