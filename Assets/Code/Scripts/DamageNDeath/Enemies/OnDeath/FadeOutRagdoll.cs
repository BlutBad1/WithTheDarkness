using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Death
{
    public class FadeOutRagdoll : EnemyDeadEvent
    {
        [SerializeField]
        private RagdollEnabler ragdollEnabler;
        [SerializeField, FormerlySerializedAs("RagdollTime"), Tooltip("Time while object in ragdoll.")]
        private float ragdollTime = 1f;
        [SerializeField, FormerlySerializedAs("FadeOutDelay"), Tooltip("Delay to fade out.")]
        private float fadeOutDelay = 1f;
        [SerializeField, FormerlySerializedAs("FadeOutSpeed"), Tooltip("Fade out speed.")]
        private float fadeOutSpeed = 0.05f;

        protected Coroutine fadeOutCoroutine;
        protected bool isEnabled = false;

        protected override void OnDisable()
        {
            base.OnDisable();
            if ((fadeOutCoroutine != null || isEnabled) && gameObject.scene.IsValid())
                gameObject.SetActive(false);
        }
        protected override void OnDeadEvent()
        {
            //if (lookCoroutine==null) //uncomment if ragdoll should be instant without physics 
            //{
            base.OnDeadEvent();
            if (ragdollEnabler)
                ragdollEnabler.EnableRagdoll();
            fadeOutCoroutine = StartCoroutine(FadeOutCoroutine());
            isEnabled = true;
            //}
        }
        private IEnumerator FadeOutCoroutine()
        {
            yield return new WaitForSeconds(ragdollTime);
            if (ragdollEnabler)
                ragdollEnabler.DisableAllRigidbodies();
            yield return new WaitForSeconds(fadeOutDelay);
            float time = 0;
            while (time < 1)
            {
                transform.position += (Vector3.down * Time.deltaTime) * fadeOutSpeed;
                time += Time.deltaTime * fadeOutSpeed;
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}
