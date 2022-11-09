using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenDimming : MonoBehaviour
{
    enum DimmingStatus
    {
        enable = 1, disable = 2
    }
    public float DimmingTime=2f;
    public float fadeSpeed=0.2f;
    public Image blackScreen;


    public void DimmingEnable()
    {
        StartCoroutine(Dimming((int)DimmingStatus.enable));

    }
    public void DimmingDisable()
    {
        StartCoroutine(Dimming((int)DimmingStatus.disable));
    }
    IEnumerator Dimming(int dimmingType)
    {
        float timeElapsed = 0f;

        float tempAlpha = blackScreen.color.a;

        while (timeElapsed < DimmingTime)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, tempAlpha);
            timeElapsed += Time.deltaTime;
            switch (dimmingType)
            {
                case (int)DimmingStatus.enable:
                    tempAlpha += Time.deltaTime * fadeSpeed;
                    break;
                case (int)DimmingStatus.disable:
                    tempAlpha -= Time.deltaTime * fadeSpeed;
                    break;
                default:
                    break;
            }
            
            yield return null;
        }

    }
}
