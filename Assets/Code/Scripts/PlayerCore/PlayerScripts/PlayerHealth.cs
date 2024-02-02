using DamageableNS;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
    public class PlayerHealth : Damageable
    {
        [SerializeField, FormerlySerializedAs("HealthRegenPerCycle")]
        private float healthRegenPerCycle = 1f;
        [SerializeField, FormerlySerializedAs("TimeAfterHitToRegen")]
        private float timeAfterHitToRegen = 3f;
        [SerializeField, FormerlySerializedAs("HeatlhRegenCycleTime")]
        private float heatlhRegenCycleTime = 0.1f;
        [SerializeField, FormerlySerializedAs("InvincibilityTime")]
        private float invincibilityTime = 1f;

        private bool hasRegenStarted = false;
        private float timeSinceLastHit;
        private Coroutine CurrentRegenCoroutine;

        public float PercentResistance { get; set; }
        public float InvincibilityTime { get => invincibilityTime; set => invincibilityTime = value; }
        public float TimeAfterHitToRegen { get => timeAfterHitToRegen; set => timeAfterHitToRegen = value; }

        private void Update()
        {
            timeSinceLastHit += Time.deltaTime;
            if (Health < HealthOnStart && timeSinceLastHit > TimeAfterHitToRegen && !hasRegenStarted)
                StartRegen();
            if (Health <= 0 && !IsDead)
                IsDead = true;
        }
        public override void TakeDamage(TakeDamageData takeDamageData)
        {
            if (timeSinceLastHit > InvincibilityTime)
            {
                takeDamageData.Damage = CalculateIncomingDamage(takeDamageData.Damage);
                base.TakeDamage(takeDamageData);
            }
        }
        public override void TakeDamage(float damage)
        {
            if (timeSinceLastHit > InvincibilityTime)
            {
                StopRegen();
                timeSinceLastHit = 0f;
                base.TakeDamage(CalculateIncomingDamage(damage));
#if UNITY_EDITOR
                if (Health > 0)
                    Debug.Log($"Damage {damage}");
#endif
            }
        }
        private void StopRegen()
        {
            if (CurrentRegenCoroutine != null)
                StopCoroutine(CurrentRegenCoroutine);
            hasRegenStarted = false;
        }
        private void StartRegen()
        {
            hasRegenStarted = true;
            if (CurrentRegenCoroutine != null)
                StopCoroutine(CurrentRegenCoroutine);
            CurrentRegenCoroutine = StartCoroutine(RegenStartCoroutine());
        }
        private float CalculateIncomingDamage(float damage) =>
             damage - (damage * PercentResistance / 100);
        private IEnumerator RegenStartCoroutine()
        {
            while (Health < HealthOnStart)
            {
                Health += healthRegenPerCycle;
                yield return new WaitForSeconds(heatlhRegenCycleTime);
            }
            if (Health > HealthOnStart)
                Health = HealthOnStart;
            hasRegenStarted = false;
        }
    }
}