using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HudNS
{


public class MessagePrint : MonoBehaviour
{
  
    private float disapperingSpeed;
    private TextMeshProUGUI infoMessage;
    float tempAlpha;

    void Start()
    {
        infoMessage = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
    }
    public void PrintMessage(string message, float disapperingSpeed)
    {
        this.disapperingSpeed = disapperingSpeed;
        StopAllCoroutines();
      
        infoMessage.text = message;
        infoMessage.alpha = 1;
        StartCoroutine(MessageDisappering());

    }
    // Update is called once per frame
  
    IEnumerator MessageDisappering()
    {

        tempAlpha = infoMessage.alpha;
        while (tempAlpha > 0)
        {
            infoMessage.color = new Color(infoMessage.color.r, infoMessage.color.g, infoMessage.color.b, tempAlpha);
            if (tempAlpha>=0.1)
                tempAlpha -= Time.deltaTime * disapperingSpeed;

            else
                tempAlpha = 0;
         
            yield return null;

        }
        infoMessage.text = "";
    }
}
}