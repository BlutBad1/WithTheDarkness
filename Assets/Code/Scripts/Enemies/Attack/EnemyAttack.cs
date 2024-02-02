using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Attack
{
    public abstract class EnemyAttack : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("Damage")]
        private int damage = 10;
        [SerializeField, FormerlySerializedAs("AttackForce")]
        private float attackForce = 5f;
        [SerializeField, FormerlySerializedAs("AttackDelay")]
        private float attackDelay = 0.5f;
        [SerializeField, FormerlySerializedAs("AttackDistance")]
        private float attackDistance = 2f;
        [SerializeField, FormerlySerializedAs("attackRadius")]
        private float attackRadius = 2f;

        protected bool isAttacking = false;

        public int Damage { get => damage; set => damage = value; }
        public float AttackForce { get => attackForce; set => attackForce = value; }
        public float AttackDelay { get => attackDelay; set => attackDelay = value; }
        public float AttackDistance { get => attackDistance; set => attackDistance = value; }
        public float AttackRadius { get => attackRadius; set => attackRadius = value; }

        public abstract void StopAttack();
        public abstract void TryAttack();
        protected abstract bool CanAttack();
    }
}