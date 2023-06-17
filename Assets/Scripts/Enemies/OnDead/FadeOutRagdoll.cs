using System.Collections;
using UnityEngine;

namespace EnemyOnDeadNS
{
    public class FadeOutRagdoll : DeadEvent
    {
        [SerializeField]
        private RagdollEnabler ragdollEnabler;
        [Tooltip("Time while object in ragdoll.")]
        public float RagdollTime = 1f;
        [Tooltip("Delay to fade out.")]
        public float FadeOutDelay = 1f;
        [Tooltip("Fade out speed.")]
        public float FadeOutSpeed = 0.05f;
        private Coroutine fadeOutCoroutine;
        override protected void Start()
        {
            base.Start();
            if (!ragdollEnabler)
                TryGetComponent(out ragdollEnabler);
        }
        private void OnDisable()
        {
            if (fadeOutCoroutine != null)
                Destroy(gameObject);
        }
        override public void OnDead()
        {
            //if (lookCoroutine==null) //uncomment if ragdoll should be instant without physics 
            //{
            base.OnDead();
            if (ragdollEnabler)
                ragdollEnabler.EnableRagdoll();
            fadeOutCoroutine = StartCoroutine(FadeOutCoroutine());
            //}
        }
        private IEnumerator FadeOutCoroutine()
        {
            yield return new WaitForSeconds(RagdollTime);
            if (ragdollEnabler)
                ragdollEnabler.DisableAllRigidbodies();
            yield return new WaitForSeconds(FadeOutDelay);
            float time = 0;
            while (time < 1)
            {
                transform.position += (Vector3.down * Time.deltaTime) * FadeOutSpeed;
                time += Time.deltaTime * FadeOutSpeed;
                yield return null;
            }
            Destroy(gameObject);
        }
    }

}
