using System.Collections;
using UnityEngine;
namespace ForTestingNS
{
    public class CoroutineTest : MonoBehaviour
    {
        Coroutine currentCoroutine;
        private void Start()
        {
            currentCoroutine = (StartCoroutine(TestCoroutine()));
        }
        public void Update()
        {
        }
        IEnumerator TestCoroutine()
        {
            yield return null;
        }
    }
}