using UnityEngine;

namespace LightNS
{
    [RequireComponent(typeof(Light))]
    public class LightIntensityControlling : MonoBehaviour
    {
        public LightGlowTimer LightGlowTimer;
        private Light light;
        private Behaviour halo;
        private float startedTimeLeft = 10;
        private float currentTimeLeft = 0f;
        private float startingIntenstity;
        private void Start()
        {
            if (!LightGlowTimer)
                LightGlowTimer = UtilitiesNS.Utilities.GetComponentFromGameObject<LightGlowTimer>(gameObject);
            light = GetComponent<Light>();
            halo = GetComponent<Behaviour>();
            startingIntenstity = light.intensity;
        }
        private void Update()
        {
            currentTimeLeft = LightGlowTimer.CurrentTimeLeft;
            startedTimeLeft = LightGlowTimer.MaxTimeLeft;
            light.intensity = light.intensity > startingIntenstity ? startingIntenstity : (startingIntenstity * currentTimeLeft) / startedTimeLeft;
            if (halo != null)
                halo.enabled = light.intensity < 0.2 ? false : true;
        }
    }
}