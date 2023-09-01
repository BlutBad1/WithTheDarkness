using UnityEngine;
namespace LightNS
{
    [RequireComponent(typeof(ParticleSystem))]
    public class FireParticleSystemIntensityControlling : MonoBehaviour
    {
        private ParticleSystem particleSystem;
        private float startedTimeLeft = 10;
        private float currentTimeLeft = 0f;
        private float startingAlpha;
        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            startingAlpha = particleSystem.main.startColor.color.a;
        }
        void Update()
        {
            currentTimeLeft = LightGlowTimer.CurrentTimeLeft;
            startedTimeLeft = LightGlowTimer.StartedTimeLeft;
            float newAlpha;
            var main = particleSystem.main;
            newAlpha = main.startColor.color.a > startingAlpha ? startingAlpha : (startingAlpha * currentTimeLeft) / startedTimeLeft;
            main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, newAlpha);
        }
    }
}