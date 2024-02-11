using AYellowpaper;
using CreatureNS;
using EnemyConstantsNS.Gramophone;
using EnemyNS.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Type.Gramophone
{
    public class GramophoneBehaviour : Enemy
    {
        [SerializeField, RequireInterface(typeof(ICreature))]
        private MonoBehaviour creature;
        [SerializeField]
        private StateHandler stateHandler;
        [SerializeField, FormerlySerializedAs("GramophoneRenders")]
        private Renderer[] gramophoneRenders;

        private bool gramophonEnabled = false;

        private ICreature Creature { get => (ICreature)creature; }

        protected override void OnEnable()
        {
            base.OnEnable();
            stateHandler.OnStateChange += EnableGramophone;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            stateHandler.OnStateChange -= EnableGramophone;
        }
        private void Update()
        {
            if (gramophonEnabled)
                IsRenderVisible();
        }
        public void KillGramophone() =>
            TakeDamage(Health);
        private void EnableGramophone(EnemyState oldState, EnemyState newState)
        {
            if (!gramophonEnabled && oldState == movement.DefaultState)
            {
                gramophonEnabled = true;
                movement.Animator.SetTrigger(GramophoneConstants.PLAY_TRIGGER);
            }
        }
        private void IsRenderVisible()
        {
            bool isVisible = UtilitiesNS.RendererNS.CheckRenderVisibility.IsSomeRendererVisible(gramophoneRenders);
            if (isVisible)
                Creature.BlockMovement();
            else
                Creature.UnblockMovement();
        }
    }
}