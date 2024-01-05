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
        public void DimmingEnable() =>
            DimmingEnable(FadeSpeed);
        public void DimmingEnable(float fadeSpeed, bool scaleFromDeltaTime = true, float updateTime = 0)
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(Dimming(true, fadeSpeed, scaleFromDeltaTime, updateTime));
        }
        public void DimmingDisable() =>
            DimmingDisable(FadeSpeed);
        public void DimmingDisable(float fadeSpeed, bool scaleFromDeltaTime = true, float updateTime = 0)
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(Dimming(false, fadeSpeed, scaleFromDeltaTime, updateTime));
        }
        private IEnumerator Dimming(bool enable, float fadeSpeed, bool scaleFromDeltaTime = true, float updateTime = 0)
        {
            float tempAlpha = BlackScreen.color.a;
            while (tempAlpha <= 1 && enable || tempAlpha >= 0 && !enable)
            {
                BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, tempAlpha);
                if (enable)
                    tempAlpha += scaleFromDeltaTime ? Time.deltaTime * fadeSpeed : fadeSpeed;
                else
                    tempAlpha -= scaleFromDeltaTime ? Time.deltaTime * fadeSpeed : fadeSpeed;
                yield return scaleFromDeltaTime ? new WaitForSecondsRealtime(updateTime) : new WaitForSeconds(updateTime);
            }
            BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, enable ? 1f : 0f);
            followCoroutine = null;
        }
    }
}