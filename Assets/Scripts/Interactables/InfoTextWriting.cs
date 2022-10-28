using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoTextWriting : Interactable
{
    [SerializeField]
    private string message;
    [SerializeField]
    private float disapperingSpeed;
 

    protected override void Interact()
    {

        GameObject.Find("InfoText").GetComponent<MessagePrint>().PrintMessage(message, disapperingSpeed);

    }
  
}
