using UnityEngine;

namespace LightNS
{
    public class LightIntensityControlling : MonoBehaviour
    {
        public LightGlowTimer LightGlowTimer;
        [SerializeField]
        private Light light;
        [SerializeField]
        private Behaviour halo;
        private float startingIntenstity;
        private void Awake()
        {
            if (!LightGlowTimer)
            {
                //LightGlowTimer = Utilities.GetComponentFromGameObject<LightGlowTimer>(gameObject);
                LightGlowTimer =/* LightGlowTimer ? LightGlowTimer :*/ GameObject.FindAnyObjectByType<LightGlowTimer>();
            }
            if (!light)
                light = GetComponent<Light>();
            if (!halo)
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