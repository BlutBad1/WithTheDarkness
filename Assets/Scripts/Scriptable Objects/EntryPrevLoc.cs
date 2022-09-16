using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPrevLoc : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [HideInInspector]
    public Transform previousGate; // position of the last exit

    private void OnTriggerEnter(Collider other)
    {


        previousGate.position = new Vector3(previousGate.position.x, 0.98f, previousGate.position.z);
        StartCoroutine(Teleport());


    }
    IEnumerator Teleport()
    {
        player.GetComponent<InputManager>().isTeleporting = true;
        yield return new WaitForSeconds(0.05f);
       
        player.transform.position = previousGate.position;
        yield return new WaitForSeconds(0.05f);
        player.GetComponent<InputManager>().isTeleporting = false;

    }
}
