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
        public void FooCameraShake() =>
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        public void FooCameraShake(float coeffOfShaking) =>
            CameraShaker.Instance.ShakeOnce(magnitude * coeffOfShaking, roughness * coeffOfShaking, fadeInTime, fadeOutTime);
        public void FooCameraShake(float magnitude, float roughness, float fadeInTime, float fadeOutTime) =>
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        public void FooCameraShake(float magnitude, float roughness) =>
           CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
    }
}