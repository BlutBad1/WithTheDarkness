using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EffectsNS.PlayerEffects
{
    public class BlackScreenDimming : MonoBehaviour
    {
        public float FadeSpeed = 0.2f;
        public Image BlackScreen;
        public bool BlackScreenOnStart = true;
        private Coroutine followCoroutine;
        private void Awake() =>
            BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, BlackScreenOnStart ? 1f : BlackScreen.color.a);
        public void SetFadeSpeed(float fadeSpeed) =>
            FadeSpeed = fadeSpeed;
        public void DimmingEnable() =>
            DimmingEnable(true, 0);
        public void DimmingDisable() =>
           DimmingDisable(true, 0);
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
        private IEnumerator Dimming(bool enable, bool scaleFromDeltaTime = true, float waitTime = 0)
        {
            float tempAlpha = BlackScreen.color.a;
            while (tempAlpha <= 1 && enable || tempAlpha >= 0 && !enable)
            {
                BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, tempAlpha);
                if (enable)
                {
                    if (scaleFromDeltaTime)
                        tempAlpha += Time.deltaTime * FadeSpeed;
                    else
                        tempAlpha += FadeSpeed;
                }
                else
                {
                    if (scaleFromDeltaTime)
                        tempAlpha -= Time.deltaTime * FadeSpeed;
                    else
                        tempAlpha -= FadeSpeed;
                }
                yield return scaleFromDeltaTime ? new WaitForSecondsRealtime(waitTime) : new WaitForSeconds(waitTime);
            }
            BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, enable ? 1f : 0f);
        }
    }
}