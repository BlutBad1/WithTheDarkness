using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HudNS
{


public class PlayerHealthBar : MonoBehaviour
{
  
    private float health;
    private float lerpTimer;
    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    private HpCount hpCount;
    [Header("Damage Overlay")]
    public bool activate; // active overlay or not 
    public float healthBelow; // health where the overlay does not disappear
    public Image overlay;// out DamageOverlay GameObject
    public float duration; // how long the image stays fully opaque
    public float fadeSpeed;// how quickly the image will fade
    
    private float durationTimer;//time to check against the duration
    void Start()
    {
        health = maxHealth;
        if (activate)
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        }
        hpCount = GetComponent<HpCount>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (health > maxHealth)
        health = maxHealth; 
        health = Mathf.Clamp(health, 0, health);
        UpdateHealthUI();
        if (activate)
        {
            if (overlay.color.a > 0)
            {
                if (health<= healthBelow)
                    return;
                
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
    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        
        hpCount.UpdateText($"{health}/{maxHealth}");
        if (fillB>hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF <hFraction)
        {
           backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        if (activate)
        { 
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1); 
        }
           
    }
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
}