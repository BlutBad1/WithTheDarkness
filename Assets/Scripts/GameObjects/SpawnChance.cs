using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChance : MonoBehaviour
{
    public float chance;
    private void Start()
    {
        if (new System.Random().Next() % 100 > chance)
        {
            Destroy(gameObject);
        }
    }
}
