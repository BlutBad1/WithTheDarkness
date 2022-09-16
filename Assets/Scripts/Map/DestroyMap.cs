using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMap : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    float destroyDistance;
    private void FixedUpdate()
    {

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist >= destroyDistance)
        {
            Destroy(gameObject);
        }


    }
}
