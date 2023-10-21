using EZCameraShake;
using UnityEngine;
namespace PlayerScriptsNS
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField]
        public float magnitude;
        [SerializeField]
        public float roughness;
        [SerializeField]
        public float fadeInTime;
        [SerializeField]
        public float fadeOutTime;
        public void FooCameraShake()
        {
            foreach (var shaker in CameraShaker.AllShakeInstances)
                shaker.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        }
        public void FooCameraShake(float coeffOfShaking)
        {
            foreach (var shaker in CameraShaker.AllShakeInstances)
                shaker.ShakeOnce(magnitude * coeffOfShaking, roughness * coeffOfShaking, fadeInTime, fadeOutTime);
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