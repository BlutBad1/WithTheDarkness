using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
namespace PlayerScriptsNS
{


public class CameraShake : MonoBehaviour
{
   
    [SerializeField]
    float magnitude;
    [SerializeField]
    float roughness;
    [SerializeField]
    float fadeInTime;
    [SerializeField]
    float fadeOutTime;
    public void FooCameraShake()
    {
        CameraShaker.Instance.ShakeOnce(magnitude , roughness, fadeInTime, fadeOutTime);

    }
    public void FooCameraShake(float coeffOfShaking)
    {
        CameraShaker.Instance.ShakeOnce(magnitude *coeffOfShaking, roughness* coeffOfShaking, fadeInTime, fadeOutTime);

    }
 
    public void FooCameraShake(float magnitude, float roughness, float fadeInTime, float fadeOutTime )
    {
        CameraShaker.Instance.ShakeOnce(magnitude, roughness , fadeInTime, fadeOutTime);

    }
    public void FooCameraShake(float magnitude, float roughness)
    {
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);

    }
}
}