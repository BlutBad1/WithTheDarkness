using AYellowpaper;
using CreatureNS;
using DamageableNS;
using DamageableNS.OnTakeDamage;
using EnemyNS.Base;
using System.Collections;
using UnityEngine;

namespace EnemyNS.OnTakeDamage
{
    public class EnemyKnockoutEffect : PushDamageable
    {
        [SerializeField, RequireInterface(typeof(ICreature))]
        private MonoBehaviour creature;
        [SerializeField]
        private bool knockoutEnable = false;
        [SerializeField]
        private float inKnockoutTime = 0.3f;

        private bool isInKnockout = false;
        private Vector3 agentBeforeDestination;
        private Coroutine knockoutCoroutine;

        public bool KnockoutEnable { get => knockoutEnable; set => knockoutEnable = value; }
        private ICreature Creature { get => (ICreature)creature; }

        protected override void OnTakeDamage(TakeDamageData takeDamageData)
        {
            Enemy enemy = (Enemy)Damageable;
            if (KnockoutEnable && enemy.Health > 0 && takeDamageData.HitData != null)
            {
                if (!isInKnockout)
                {
                    Creature.BlockMovement();
                    DamageableRigidbody.isKinematic = false;
                    isInKnockout = true;
                    PushRigidbody(DamageableRigidbody, takeDamageData);
                    if (knockoutCoroutine == null)
                        knockoutCoroutine = StartCoroutine(InKnockout(DamageableRigidbody, enemy));
                }
            }
        }
        private IEnumerator InKnockout(Rigidbody hittedRigidbody, Enemy enemy)
        {
            yield return new WaitForSeconds(inKnockoutTime);
            if (enemy.Health > 0)
            {
                Creature.UnblockMovement();
                if (hittedRigidbody != null)
                    hittedRigidbody.isKinematic = true;
            }
            isInKnockout = false;
            knockoutCoroutine = null;
        }
    }
}