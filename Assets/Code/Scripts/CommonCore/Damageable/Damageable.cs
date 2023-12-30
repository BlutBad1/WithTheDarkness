using UnityEngine;
using static DamageableNS.IDamageable;

namespace DamageableNS
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        public event DamageWIthData OnTakeDamageWithDamageData;
        public event DamageWithoutData OnTakeDamageWithoutDamageData;
        public event DamageWithoutData OnDead;
        /// <summary>
        /// Current Health
        /// </summary>
        public float Health = 100;
        public bool InvokeOnTakeDamageAfterDead = true;
        [HideInInspector]
        public float HealthOnStart;
        [HideInInspector]
        private bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; if (isDead) OnDead?.Invoke(); }
        }
        /// <summary>
        /// Health on Start
        /// </summary>
        protected void Start()
        {
            HealthOnStart = Health;
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
                if (!InvokeOnTakeDamageAfterDead)
                {
                    OnTakeDamageWithDamageData = null;
                    OnTakeDamageWithoutDamageData = null;
                }
                OnDead = null;
            }
        }
    }
}