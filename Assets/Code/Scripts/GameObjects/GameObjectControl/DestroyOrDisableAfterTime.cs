using System.Collections;
using UnityEngine;

namespace GameObjectsControllingNS
{
    public class DestroyOrDisableAfterTime : MonoBehaviour
    {
        public void DestroyAfter(float timeToDestroy) =>
              StartCoroutine(ChangeGameObjectStatus(timeToDestroy, true));
        public void DisableAfter(float timeToDestroy) =>
            StartCoroutine(ChangeGameObjectStatus(timeToDestroy, false));
        IEnumerator ChangeGameObjectStatus(float timeToDestroy, bool isDestroy)
        {
            float timeElapsed = 0f;
            while (timeElapsed < timeToDestroy)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            if (isDestroy)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
    }
}