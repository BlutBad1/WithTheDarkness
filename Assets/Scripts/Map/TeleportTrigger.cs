
using System;
using System.Collections;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
  
    GameObject player;
    [SerializeField]
    public Transform teleportPointToHere;
    [HideInInspector]
    public Transform teleportPoint; // position of the next spawn point of a next location 
    [SerializeField]
    float automaticallySpawnTimer = 2;
    float timeElapsed = 0;

    GameObject dimming ;
    GameObject audioManager;
    bool isActivated = false;
    //private void OnTriggerStay(Collider other)
    //{
    //    timeElapsed += Time.deltaTime;
    //    if (timeElapsed >= automaticallySpawnTimer)
    //    {
    //        StartCoroutine(Teleport());
    //        timeElapsed = 0;
    //    }
    //}
    private void Start()
    {
        dimming = GameObject.Find("BlackScreenDimming");
        dimming.GetComponent<BlackScreenDimming>().fadeSpeed = 0.5f;
        audioManager = GameObject.Find("MainAudioManager");
        player = GameObject.Find("Player");
    }
    private void Update()
    {
        if (isActivated)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= automaticallySpawnTimer)
            {
                StartCoroutine(Teleport());
                dimming.GetComponent<BlackScreenDimming>().DimmingDisable();
                timeElapsed = 0;
                isActivated = false;
                
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isActivated =true;
        dimming.GetComponent<BlackScreenDimming>().DimmingEnable();
        audioManager.GetComponent<AudioManager>().PlayWithoutRep("transitionSound");

    }
    //private void OnTriggerExit(Collider other)
    //{
      
    //    timeElapsed = 0;
    //    //isScreenDimmed = false;
    //    StartCoroutine(Teleport());
    //    dimmingOff.Invoke();

    //}
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
