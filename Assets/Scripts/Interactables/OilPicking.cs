using HudNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractableNS
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
    [SerializeField]
    LightsTimers lightTimer;
    private void Start()
    {
     


        lightTimer = GameObject.Find("LightsTimers").GetComponent<LightsTimers>();
    }
    protected override void Interact()
    {
        System.Random rand = new System.Random();
        int addingTime = rand.Next(minTime, maxTime);
        lightTimer.AddTime(addingTime);
        message += $"{addingTime} seconds";
        GameObject.Find("InfoText").GetComponent<MessagePrint>().PrintMessage(message, disapperingSpeed);

    }

}
}