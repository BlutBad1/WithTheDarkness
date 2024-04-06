using UnityEngine;
using UnityEngine.Serialization;
using static DamageableNS.IDamageable;

namespace DamageableNS
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        [SerializeField, FormerlySerializedAs("Health")]
        private float health = 100;
        [SerializeField, FormerlySerializedAs("InvokeOnTakeDamageAfterDead")]
        private bool invokeOnTakeDamageAfterDead = true;

        private float healthOnStart;
        private bool isDead = false;

        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; if (isDead) OnDead?.Invoke(); }
        }
        public float Health { get => health; set => health = value; }
        public float HealthOnStart { get => healthOnStart; }

        public event DamageWIthData OnTakeDamageWithDamageData;
        public event DamageWithoutData OnTakeDamageWithoutDamageData;
        public event DamageWithoutData OnDead;

        protected void Start()
        {
            healthOnStart = Health;
        }
        public virtual GameObject GetGameObject() =>
            gameObject;
        public virtual void TakeDamage(TakeDamageData takeDamageData)
        {
            TakeDamage(takeDamageData.Damage);
            OnTakeDamageWithDamageData?.Invoke(takeDamageData);
        }
        public virtual void TakeDamage(float damage)
        {
            Health -= damage;
            OnTakeDamageWithoutDamageData?.Invoke();
            if (Health <= 0)
            {
                IsDead = true;
                if (!invokeOnTakeDamageAfterDead)
                {
                    OnTakeDamageWithDamageData = null;
                    OnTakeDamageWithoutDamageData = null;
                }
                OnDead = null;
            }
        }
    }
}