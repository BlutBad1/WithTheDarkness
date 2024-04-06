using EnemyConstantsNS;
using EntityNS.Base;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EntityNS.Attack
{
    public abstract class EntityStateAttack : EntityAttack
    {
        [SerializeField]
        protected StateHandler stateHandler;
        [SerializeField]
        protected Animator animator;

        private Coroutine lookCoroutine;

        public delegate void AttackEvent(StateInfo stateInfo);
        public AttackEvent OnAttack;

        private void OnEnable()
        {
            stateHandler.OnFollow += TryAttack;
            OnAttack += PerformAnimationAttack;
        }
        private void OnDisable()
        {
            stateHandler.OnFollow -= TryAttack;
            OnAttack -= PerformAnimationAttack;
        }
        public override void StopAttack() =>
            isAttacking = false;
        public override void TryAttack()
        {
            if (CanAttack())
            {
                OnAttack?.Invoke(stateHandler.GenerateStateInfo());
                isAttacking = true;
            }
        }
        protected override bool CanAttack() =>
            Vector3.Distance(stateHandler.GenerateStateInfo().PursuedTarget.transform.position, transform.position) <= AttackDistance;
        protected void PerformAnimationAttack(StateInfo stateInfo)
        {
            if (!isAttacking)
            {
                if (lookCoroutine != null)
                    StopCoroutine(lookCoroutine);
                lookCoroutine = StartCoroutine(LookAt(stateInfo.PursuedTarget.transform));
				animator.SetTrigger(EntityConstants.ATTACK_TRIGGER);
            }
        }
        private IEnumerator LookAt(Transform Target)
        {
            Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
            float time = 0;
            while (time < 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
                time += Time.deltaTime * 2;
                yield return null;
            }
            transform.rotation = lookRotation;
        }
    }
}