using System.Collections;
using UnityEngine;
namespace EnemyNS.Death
{
    public class FadeOutRagdollSpawn : EnemyDeadEvent
    {
        public GameObject MainGameObjectBody;
        public GameObject FadeOutRagdollBody;
        [Tooltip("Delay to fade out.")]
        public float FadeOutDelay = 1f;
        [Tooltip("Fade out speed.")]
        public float FadeOutSpeed = 0.05f;
        private Coroutine fadeOutCoroutine;
        private bool isEnabled = false;
        private void OnDisable()
        {
            if ((fadeOutCoroutine != null || isEnabled) && gameObject.scene.IsValid())
                gameObject.SetActive(false);
        }
        override public void OnDead()
        {
            base.OnDead();
            MainGameObjectBody.SetActive(false);
            FadeOutRagdollBody.SetActive(true);
            fadeOutCoroutine = StartCoroutine(FadeOutCoroutine());
            isEnabled = true;
        }
        private IEnumerator FadeOutCoroutine()
        {
            yield return new WaitForSeconds(FadeOutDelay);
            float time = 0;
            while (time < 1)
            {
                transform.position += (Vector3.down * Time.deltaTime) * FadeOutSpeed;
                time += Time.deltaTime * FadeOutSpeed;
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}