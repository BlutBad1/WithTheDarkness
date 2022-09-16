using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLight : MonoBehaviour
{
    public Light light;
    public Behaviour halo;
    public float startingTime=300f;
    float currentTime = 0f;
    private float startingIntenstity;
   
    void Start()
    {
        currentTime = startingTime;
        startingIntenstity = light.intensity; 
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        light.intensity = light.intensity>startingIntenstity?startingIntenstity:(startingIntenstity * currentTime) / startingTime;
        if (halo !=null)
        {
            halo.enabled = light.intensity < 0.2 ? false : true;
        }
        currentTime = currentTime < 0 ? 0 : currentTime;
        

       
    }
}
