using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject victor;
    [SerializeField]
    GameObject player;
    private void OnTriggerStay(Collider other)
    {
        victor.transform.LookAt(player.transform);
    }
    
}
