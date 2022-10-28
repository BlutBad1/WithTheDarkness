using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenDimming : MonoBehaviour
{
    public float DimmingTime=2f;
    public float fadeSpeed=0.2f;
    public Image blackScreen;


    public void DimmingEnable()
    {
        StartCoroutine(DimmingOn());
      
    }
    public void DimmingDisable()
    {
        StartCoroutine(DimmingOff());
    }
    IEnumerator DimmingOn()
    {
        float timeElapsed = 0f;

        float tempAlpha = blackScreen.color.a;
      
        while (timeElapsed < DimmingTime)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, tempAlpha);   
            timeElapsed += Time.deltaTime;
            tempAlpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

    }
    IEnumerator DimmingOff()
    {
        float timeElapsed = 0f;

        float tempAlpha = blackScreen.color.a;

        while (timeElapsed < DimmingTime)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, tempAlpha);
            timeElapsed += Time.deltaTime;
            tempAlpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

    }
}
