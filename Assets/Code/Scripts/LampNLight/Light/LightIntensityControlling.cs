using UnityEngine;
using UtilitiesNS;

namespace LightNS
{
    [RequireComponent(typeof(Light))]
    public class LightIntensityControlling : MonoBehaviour
    {
        public LightGlowTimer LightGlowTimer;
        private Light light;
        private Behaviour halo;
        private float startingIntenstity;
        private void OnEnable()
        {
            if (!LightGlowTimer)
                LightGlowTimer = Utilities.GetComponentFromGameObject<LightGlowTimer>(gameObject) != null ?
                    Utilities.GetComponentFromGameObject<LightGlowTimer>(gameObject) : GameObject.FindAnyObjectByType<LightGlowTimer>();
            light = GetComponent<Light>();
            halo = GetComponent<Behaviour>();
            startingIntenstity = light.intensity;
            SetIntensity();
        }
        private void Update()
        {
            SetIntensity();
        }
        private void SetIntensity()
        {
            light.intensity = light.intensity > startingIntenstity ? startingIntenstity : (startingIntenstity * LightGlowTimer.GetGlowingLeftTimeInPercantage()) / 100;
            if (halo != null)
                halo.enabled = light.intensity < 0.2 ? false : true;
        }
    }
}