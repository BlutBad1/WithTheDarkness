using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

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
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
    
    }

  
}
