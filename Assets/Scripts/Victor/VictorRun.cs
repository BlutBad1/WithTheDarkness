using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace VictorNS
{
    public class VictorRun : MonoBehaviour
    {
        public float Dutation = 3000;
        void Start()
        {
            StartCoroutine(Fly());
        }

        IEnumerator Fly()
        {
            yield return new WaitForSeconds(1.5f);
            transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1000), Dutation).SetUpdate(UpdateType.Normal, false).SetLoops(-1, LoopType.Incremental);
            yield return null;
        }
    }
}