using CreatureNS;
using ScriptableObjectNS.Creature;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScriptsNS
{
    public class PlayerHealth : Damageable, ICreature, ISerializationCallbackReceiver
    {
        [HideInInspector]
        public static List<string> CreatureNames;
        [ListToPopup(typeof(PlayerHealth), "CreatureNames")]
        public string CreatureType;
        public float HealthRegenPerCycle = 1f;
        public float TimeAfterHitToRegen = 3f;
        public float HeatlhRegenCycleTime = 0.1f;
        public float InvincibilityTime = 1f;
        [HideInInspector]
        public bool HasRegenStarted = false;
        float timeSinceLastHit;
        Coroutine CurrentRegenCoroutine;
        private void Update()
        {
            timeSinceLastHit += Time.deltaTime;
            if (Health < OriginalHealth && timeSinceLastHit > TimeAfterHitToRegen && !HasRegenStarted)
                StartRegen();
            if (Health <= 0 && !IsDead)
                IsDead = true;
        }
        public override void TakeDamage(TakeDamageData takeDamageData)
        {
            if (timeSinceLastHit > InvincibilityTime)
                base.TakeDamage(takeDamageData);
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
        public string GetCreatureName() =>
            CreatureType;

        public GameObject GetCreatureGameObject() =>
            gameObject;
        public void OnBeforeSerialize()
        {
            CreatureNames = CreatureTypes.Instance.Names;
        }
        public void OnAfterDeserialize()
        {
        }
        private IEnumerator RegenStartCoroutine()
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