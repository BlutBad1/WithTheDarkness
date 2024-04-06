using EntityNS.Attack;
using EntityNS.Base;
using EntityNS.Base.StateBehaviourNS;
using EntityNS.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace EntityNS.Death
{
    public class EntityDeadEvent : MonoBehaviour
    {
        [SerializeField]
        protected EntityBehaviour entityBehaviour;
        [SerializeField]
        protected EntityMovement entityMovement;
        [SerializeField]
        protected CreatureInSightChecker creatureInSightChecker;
        [SerializeField]
        protected EntityAttack entityAttack;
        [SerializeField]
        protected EntityUseSkills entityUseSkills;
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected StateBehaviour[] stateBehaviours;

        protected virtual void OnEnable()
        {
            entityBehaviour.OnDead += OnDeadEvent;
        }
        protected virtual void OnDisable()
        {
            entityBehaviour.OnDead -= OnDeadEvent;
        }
        protected virtual void OnDeadEvent()
        {
            entityUseSkills?.StopSkills();
            creatureInSightChecker.enabled = false;
            entityMovement.CurrentState = EntityState.Dead;
            entityMovement.Agent.enabled = false;
            entityAttack.StopAttack();
            entityAttack.enabled = false;
            entityMovement.enabled = false;
            entityBehaviour.enabled = false;
            animator.enabled = false;
            foreach (var behaviour in stateBehaviours)
                behaviour.enabled = false;
        }
    }
}