using System.Collections;
using UnityEngine;

namespace PlayerScriptsNS
{
    public class PlayerHealth : Damageable
    {
        public int HealthRegenPerCycle = 1;
        public float TimeAfterHitToRegen = 3f;
        public float HeatlRegenCycleTime = 0.1f;
        float timeSinceLastHit;
        public Coroutine CurrentRegenCoroutine;
        bool HasRegenStarted = false;
        private void Update()
        {
            timeSinceLastHit += Time.deltaTime;
            if (Health < 100 && timeSinceLastHit > TimeAfterHitToRegen && !HasRegenStarted)
                RegenStart();
        }
        public override void TakeDamage(int damage, float force, Vector3 hit)
        {
            TakeDamage(damage);
            OnTakeDamage?.Invoke(damage, force, hit);
        }
        public override void TakeDamage(int damage)
        {
            if (CurrentRegenCoroutine != null)
                StopCoroutine(CurrentRegenCoroutine);
            timeSinceLastHit = 0f;
            HasRegenStarted = false;
            Health -= damage;
            if (Health <= 0)
            {
                Debug.Log($"Damage {damage}. You're Dead!");
                OnDeath?.Invoke();
            }
            else
                Debug.Log($"Damage {damage}");
        }
        public void RegenStart()
        {
            HasRegenStarted = true;
            if (CurrentRegenCoroutine != null)
                StopCoroutine(CurrentRegenCoroutine);
            CurrentRegenCoroutine = StartCoroutine(RegenStartCoroutine());
        }
        IEnumerator RegenStartCoroutine()
        {
            while (Health < 100)
            {
                Health += HealthRegenPerCycle;
                yield return new WaitForSeconds(HeatlRegenCycleTime);
            }
            if (Health > 100)
                Health = 0;
            HasRegenStarted = false;
        }
    }
}