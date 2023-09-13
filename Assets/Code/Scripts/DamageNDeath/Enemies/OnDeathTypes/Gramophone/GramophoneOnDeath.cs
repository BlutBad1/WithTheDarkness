using InteractableNS.Common;
using UnityEngine;

namespace EnemyNS.Death
{
    public class GramophoneOnDeath : EnemyDeadEvent
    {
        public GameObject[] GramophoneInnerGameObjects;
        private void OnDisable() =>
           gameObject.SetActive(false);
        public override void OnDead()
        {
            enemy.Movement.Animator.SetTrigger(MyConstants.CreatureConstants.EnemyConstants.GramophoneConstants.STOP_PLAYING_TRIGGER);
            base.OnDead();
            ItemImpact itemImpact;
            foreach (var item in GramophoneInnerGameObjects)
            {
                item.AddComponent<Rigidbody>();
                itemImpact = item.AddComponent<ItemImpact>();
                itemImpact.ImpactForce = 2f;
            }
        }
    }
}
