using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace VictorNS
{
    public class VictorFly : Damageable
    {
        public float Duration = 250;
        private void Awake() =>
            Health = 25;
        public override void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                GetComponent<AudioSource>().Play();
                StartCoroutine(Fly());
            }
        }
        IEnumerator Fly()
        {
            transform.DOLocalRotate(new Vector3(0, transform.position.y + 360, 0), Duration).SetLoops(-1, LoopType.Incremental);
            yield return new WaitForSeconds(1.5f);
            transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z), 3000).SetUpdate(UpdateType.Normal, true);
            yield return null;
        }
    }
}