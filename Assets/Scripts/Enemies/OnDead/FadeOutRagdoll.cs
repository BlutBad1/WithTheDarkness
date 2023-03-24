using EnemyBaseNS;
using System.Collections;
using UnityEngine;

namespace EnemyOnDeadNS
{
    public class FadeOutRagdoll : MonoBehaviour
    {
        [SerializeField]
        private RagdollEnabler ragdollEnabler;
        [Tooltip("Time while object in ragdoll.")]
        public float RagdollTime = 1f;
        [Tooltip("Delay to fade out.")]
        public float FadeOutDelay = 1f;
        [Tooltip("Fade out speed.")]
        public float FadeOutSpeed = 0.05f;
        private Enemy enemy;
        private Coroutine lookCoroutine;
        void Start()
        {
            if (ragdollEnabler == null)
            {
                TryGetComponent(out ragdollEnabler);
            }
            enemy = GetComponent<Enemy>();
            enemy.OnDeath += OnDead;
        }
        void OnDead()
        {
            //if (lookCoroutine==null) //uncomment if ragdoll should be instant without physics 
            //{
            enemy.Movement.State = EnemyState.Dead;
            enemy.Agent.enabled = false;
            enemy.Movement.enabled = false;
            ragdollEnabler.EnableRagdoll();
            lookCoroutine = StartCoroutine(FadeOutCoroutine());


            //}

        }
        // Update is called once per frame
        private IEnumerator FadeOutCoroutine()
        {
            yield return new WaitForSeconds(RagdollTime);
            if (ragdollEnabler != null)
            {
                ragdollEnabler.DisableAllRigidbodies();
            }
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
