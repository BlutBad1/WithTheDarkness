using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HudNS
{


public class Overlay : MonoBehaviour
{
    
    [Header("Overlay")]
    public bool activate;
    public Image overlay;// out DamageOverlay GameObject
    public float duration; // how long the image stays fully opaque
    public float fadeSpeed;// how quickly the image will fade

    private float durationTimer;//time to check against the duration
    void Start()
    {
        //Logic here //health = maxHealth;
        if (activate)
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Logic here //health = Mathf.Clamp(health, 0, health);
        //UpdateHealthUI();
        if (activate)
        {
            if (overlay.color.a > 0)
            {
                durationTimer += Time.deltaTime;
                if (durationTimer > duration)
                {
                    float tempAlpha = overlay.color.a;
                    tempAlpha -= Time.deltaTime * fadeSpeed;

                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
                }
            }
        }

    }
    //Logic here

   //Example
   //public void TakeDamage(float damage)
    //{
    //    health -= damage;
    //    lerpTimer = 0f;
    //    durationTimer = 0;
    //    if (activate)
    //    {
    //        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1); 
    //    }

    //}
    
}
}