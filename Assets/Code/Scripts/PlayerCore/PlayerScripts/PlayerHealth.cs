using System.Collections;
using UnityEngine;

namespace PlayerScriptsNS
{
    public class PlayerHealth : Damageable
    {
        public float HealthRegenPerCycle = 1f;
        public float TimeAfterHitToRegen = 3f;
        public float HeatlhRegenCycleTime = 0.1f;
        public float InvincibilityTime = 1f;
        float timeSinceLastHit;
        Coroutine CurrentRegenCoroutine;
        [HideInInspector]
        public bool HasRegenStarted = false;
        private void Update()
        {
            timeSinceLastHit += Time.deltaTime;
            if (Health < OriginalHealth && timeSinceLastHit > TimeAfterHitToRegen && !HasRegenStarted)
                StartRegen();
            if (Health <= 0 && !IsDead)
                IsDead = true;
        }
        public override void TakeDamage(float damage, float force, Vector3 hit)
        {
            if (timeSinceLastHit > InvincibilityTime)
                base.TakeDamage(damage, force, hit);
        }
        public override void TakeDamage(float damage)
        {
            if (timeSinceLastHit > InvincibilityTime)
            {
                StopRegen();
                timeSinceLastHit = 0f;
                base.TakeDamage(damage);
#if UNITY_EDITOR
                if (Health > 0)
                    Debug.Log($"Damage {damage}");
#endif
            }
        }
        public void StopRegen()
        {
            if (CurrentRegenCoroutine != null)
                StopCoroutine(CurrentRegenCoroutine);
            HasRegenStarted = false;
        }
        public void StartRegen()
        {
            HasRegenStarted = true;
            if (CurrentRegenCoroutine != null)
                StopCoroutine(CurrentRegenCoroutine);
            CurrentRegenCoroutine = StartCoroutine(RegenStartCoroutine());
        }
        IEnumerator RegenStartCoroutine()
        {
            while (Health < OriginalHealth)
            {
                Health += HealthRegenPerCycle;
                yield return new WaitForSeconds(HeatlhRegenCycleTime);
            }
            if (Health > OriginalHealth)
                Health = OriginalHealth;
            HasRegenStarted = false;
        }
    }
}