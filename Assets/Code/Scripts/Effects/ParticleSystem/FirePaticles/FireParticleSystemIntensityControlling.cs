using UnityEngine;
using UtilitiesNS;

namespace LightNS
{
    [RequireComponent(typeof(ParticleSystem))]
    public class FireParticleSystemIntensityControlling : MonoBehaviour
    {
        public LightGlowTimer LightGlowTimer;
        private ParticleSystem particleSystem;
        private float startingAlpha;
        private void OnEnable()
        {
            particleSystem = GetComponent<ParticleSystem>();
            startingAlpha = particleSystem.main.startColor.color.a;
            if (!LightGlowTimer)
            {
                //LightGlowTimer = Utilities.GetComponentFromGameObject<LightGlowTimer>(gameObject);
                LightGlowTimer =/* LightGlowTimer ? LightGlowTimer :*/ GameObject.FindAnyObjectByType<LightGlowTimer>();
            }
            SetIntensity();
        }
        private void Update()
        {
            SetIntensity();
        }
        public void SetIntensity()
        {
            float newAlpha;
            var main = particleSystem.main;
            newAlpha = main.startColor.color.a > startingAlpha ? startingAlpha : (startingAlpha * LightGlowTimer.GetGlowingLeftTimeInPercantage()) / 100;
            main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, newAlpha);
        }
    }
}