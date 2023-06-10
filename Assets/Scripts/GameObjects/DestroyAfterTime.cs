using System.Collections;
using UnityEngine;

namespace GameObjectsControllingNS
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public void DestroyAfter(float timeToDestroy) =>
              StartCoroutine(DestroyAfter_(timeToDestroy));
        public void DestroyAfter(int timeToDestroy) =>
            StartCoroutine(DestroyAfter_(timeToDestroy));

        IEnumerator DestroyAfter_(float timeToDestroy)
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
}