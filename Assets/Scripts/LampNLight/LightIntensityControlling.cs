using UnityEngine;

namespace LightNS
{
    [RequireComponent(typeof(Light))]
    public class LightIntensityControlling : MonoBehaviour
    {
        private Light light;
        private Behaviour halo;
        private float startedTimeLeft = 10;
        private float currentTimeLeft = 0f;
        private float startingIntenstity;

        void Start()
        {
            light = GetComponent<Light>();
            halo = GetComponent<Behaviour>();
            startingIntenstity = light.intensity;
        }

        // Update is called once per frame
        void Update()
        {
            currentTimeLeft = LightGlowTimer.CurrentTimeLeft;
            startedTimeLeft = LightGlowTimer.StartedTimeLeft;
            light.intensity = light.intensity > startingIntenstity ? startingIntenstity : (startingIntenstity * currentTimeLeft) / startedTimeLeft;
            if (halo != null)
                halo.enabled = light.intensity < 0.2 ? false : true;
        }
    }
}