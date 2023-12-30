using System.Collections;
using UnityEngine;

namespace LightNS
{
    public class LightGlowing : MonoBehaviour
    {
        [SerializeField]
        protected Light light;
        public float StartingIntensity = 1f;
        public float GlowingIntensity = 0.7f;
        [Range(-1, 1)]
        public float MaxRandomPercentVariance = 0.1f;
        public float GlowingSpeed = 0.01f;
        public float UpdateDelay = 0.05f;
        private float toIntensity;
        private Coroutine currentCoroutine;
        private void Start()
        {
            if (!light)
                light = GetComponent<Light>();
        }
        private void Update()
        {
            if (light.intensity >= StartingIntensity)
                toIntensity = GlowingIntensity + Random.Range(0, MaxRandomPercentVariance);
            if (light.intensity > toIntensity && currentCoroutine == null)
                currentCoroutine = StartCoroutine(ChangeIntesity(light, toIntensity, GlowingSpeed, UpdateDelay));
            if (light.intensity <= toIntensity && currentCoroutine == null)
                currentCoroutine = StartCoroutine(ChangeIntesity(light, StartingIntensity, GlowingSpeed, UpdateDelay));
        }
        private IEnumerator ChangeIntesity(Light light, float targetIntensity, float speed, float delay)
        {
            while (light.intensity != targetIntensity)
            {
                if (light.intensity > targetIntensity)
                    light.intensity = light.intensity - speed < targetIntensity ? targetIntensity : light.intensity - speed;
                else if (light.intensity < targetIntensity)
                    light.intensity = light.intensity + speed > targetIntensity ? targetIntensity : light.intensity + speed;
                yield return new WaitForSeconds(delay);
            }
            currentCoroutine = null;
        }
    }
}