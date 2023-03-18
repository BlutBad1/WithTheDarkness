using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLightTimer : MonoBehaviour
{
    [HideInInspector]
    public Light light;
    [HideInInspector]
    public Behaviour halo;
    [HideInInspector]
    public float startingTime=10;
    float currentTime = 0f;
    private float startingIntenstity;
   
    void Start()
    {
        light = GetComponent<Light>();
        halo = GetComponent<Behaviour>();
        startingIntenstity = light.intensity; 
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = GameObject.Find("LightsTimers").GetComponent<LightsTimers>().time;
        startingTime = GameObject.Find("LightsTimers").GetComponent<LightsTimers>().StartingTime;
        light.intensity = light.intensity>startingIntenstity?startingIntenstity:(startingIntenstity * currentTime) / startingTime;
        if (halo !=null)
        {
            halo.enabled = light.intensity < 0.2 ? false : true;
        }
       
    }
}
