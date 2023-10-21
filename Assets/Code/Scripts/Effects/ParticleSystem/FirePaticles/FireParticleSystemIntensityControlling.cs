using UnityEngine;
namespace LightNS
{
    [RequireComponent(typeof(ParticleSystem))]
    public class FireParticleSystemIntensityControlling : MonoBehaviour
    {
        public LightGlowTimer LightGlowTimer;
        private ParticleSystem particleSystem;
        private float startedTimeLeft = 10;
        private float currentTimeLeft = 0f;
        private float startingAlpha;
        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            startingAlpha = particleSystem.main.startColor.color.a;
            if (!LightGlowTimer)
                LightGlowTimer = UtilitiesNS.Utilities.GetComponentFromGameObject<LightGlowTimer>(gameObject);
        }
        void Update()
        {
            currentTimeLeft = LightGlowTimer.CurrentTimeLeft;
            startedTimeLeft = LightGlowTimer.MaxTimeLeft;
            float newAlpha;
            var main = particleSystem.main;
            newAlpha = main.startColor.color.a > startingAlpha ? startingAlpha : (startingAlpha * currentTimeLeft) / startedTimeLeft;
            main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, newAlpha);
        }
    }
}