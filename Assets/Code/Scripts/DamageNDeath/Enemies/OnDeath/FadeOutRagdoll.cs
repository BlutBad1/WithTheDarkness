using System.Collections;
using UnityEngine;

namespace EnemyNS.Death
{
    public class FadeOutRagdoll : EnemyDeadEvent
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
        private bool isEnabled = false;
        override protected void Start()
        {
            base.Start();
            if (!ragdollEnabler)
                TryGetComponent(out ragdollEnabler);
        }
        private void OnDisable()
        {
            if ((fadeOutCoroutine != null || isEnabled) && gameObject.scene.IsValid())
                gameObject.SetActive(false);
        }
        override public void OnDead()
        {
            //if (lookCoroutine==null) //uncomment if ragdoll should be instant without physics 
            //{
            base.OnDead();
            if (ragdollEnabler)
                ragdollEnabler.EnableRagdoll();
            fadeOutCoroutine = StartCoroutine(FadeOutCoroutine());
            isEnabled = true;
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
            gameObject.SetActive(false);
        }
    }

}
