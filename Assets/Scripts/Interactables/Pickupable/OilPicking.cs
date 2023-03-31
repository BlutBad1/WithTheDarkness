using HudNS;
using MyConstants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractableNS.Pickupable
{ 

public class OilPicking : Interactable
{
    [SerializeField]
    private string message;
    [SerializeField]
    private float disapperingSpeed;
    [SerializeField]
    private int minTime;
    [SerializeField]
    private int maxTime;
 
    private void Start()
    {
     


      
    }
    protected override void Interact()
    {
        System.Random rand = new System.Random();
        int addingTime = rand.Next(minTime, maxTime);
        LightGlowTimer.AddTime(addingTime);
        message += $"{addingTime} seconds";
        GameObject.Find(CommonConstants.TEXTSHOWER).GetComponent<MessagePrint>().PrintMessage(message, disapperingSpeed);

    }

}
}