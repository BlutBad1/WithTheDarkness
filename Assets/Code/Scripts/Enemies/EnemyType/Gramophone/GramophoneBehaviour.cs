using DamageableNS;
using EnemyNS.Base;
using UnityEngine;

namespace EnemyNS.Type.Gramophone
{
    public class GramophoneBehaviour : Enemy
    {
        public Renderer[] GramophoneRenders;
        protected sealed override void Start()
        {
            base.Start();
            Movement.OnFollow += IsRenderVisible;
            Movement.OnIdle += IsRenderVisible;
            Movement.OnDoPriority += IsRenderVisible;
            Movement.OnPatrol += IsRenderVisible;
            OnTakeDamageWithDamageData += GramophoneOnTakeDamageMovementBehaviour;
        }
        public void KillGramophone() =>
            TakeDamage(Health);
        public void GramophoneOnTakeDamageMovementBehaviour(TakeDamageData takeDamageData) =>
            Movement.HandleGainCreatureInSight(takeDamageData.FromGameObject);
        public void IsRenderVisible()
        {
            bool isNotVisible = true;
            foreach (var render in GramophoneRenders)
            {
                if (render.isVisible)
                {
                    isNotVisible = false;
                    break;
                }
            }
            Movement.Agent.enabled = isNotVisible;
        }
        protected override void OnAttack(IDamageable Target)
        {
        }
    }
}