using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
   public void DestroyAfter (int timeToDestroy)
    {
        
        StartCoroutine(DestroyAfter_(timeToDestroy));
    }
    IEnumerator DestroyAfter_(int timeToDestroy)
    {

        float timeElapsed = 0f;

        while (timeElapsed < timeToDestroy)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
