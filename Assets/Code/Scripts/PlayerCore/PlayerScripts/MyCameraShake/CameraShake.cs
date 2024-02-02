using EZCameraShake;
using UnityEngine;
namespace PlayerScriptsNS
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField]
        private float magnitude;
        [SerializeField]
        private float roughness;
        [SerializeField]
        private float fadeInTime;
        [SerializeField]
        private float fadeOutTime;

        public float Magnitude { get => magnitude; }
        public float Roughness { get => roughness; }

        public void FooCameraShake()
        {
            foreach (var shaker in CameraShaker.AllShakeInstances)
                shaker.ShakeOnce(Magnitude, Roughness, fadeInTime, fadeOutTime);
        }
        public void FooCameraShake(float coeffOfShaking)
        {
            foreach (var shaker in CameraShaker.AllShakeInstances)
                shaker.ShakeOnce(Magnitude * coeffOfShaking, Roughness * coeffOfShaking, fadeInTime, fadeOutTime);
        }
        public void FooCameraShake(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
        {
            foreach (var shaker in CameraShaker.AllShakeInstances)
                shaker.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        }
        public void FooCameraShake(float magnitude, float roughness)
        {
            foreach (var shaker in CameraShaker.AllShakeInstances)
                shaker.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        }
    }
}