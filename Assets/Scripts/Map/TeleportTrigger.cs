using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    public Transform teleportPointToHere;
    [HideInInspector]
    public Transform teleportPoint; // position of the next spawn point of a next location 
    [SerializeField]
    float automaticallySpawnTimer = 2;
    float timeElapsed = 0;


    private void OnTriggerStay(Collider other)
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= automaticallySpawnTimer)
        {
            StartCoroutine(Teleport());
            timeElapsed = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
      
        timeElapsed = 0;
        StartCoroutine(Teleport());


    }
    IEnumerator Teleport()
    {
        player.GetComponent<InputManager>().isTeleporting = true;
        yield return new WaitForSeconds(0.05f);
        player.transform.position = teleportPoint.position;
        player.transform.localRotation = teleportPoint.rotation;
        yield return new WaitForSeconds(0.05f);
        player.GetComponent<InputManager>().isTeleporting = false;

    }
   
}
