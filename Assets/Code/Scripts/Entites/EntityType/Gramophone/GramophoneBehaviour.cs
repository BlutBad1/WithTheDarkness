using AYellowpaper;
using CreatureNS;
using EnemyConstantsNS.Gramophone;
using EntityNS.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace EntityNS.Type.Gramophone
{
    public class GramophoneBehaviour : EntityBehaviour
    {
        [SerializeField, RequireInterface(typeof(ICreature))]
        private MonoBehaviour creature;
        [SerializeField]
        private StateHandler stateHandler;
        [SerializeField, FormerlySerializedAs("GramophoneRenders")]
        private Renderer[] gramophoneRenders;
        [SerializeField]
        private Animator animator;

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
        private void EnableGramophone(EntityState oldState, EntityState newState)
        {
            if (!gramophonEnabled && oldState == Movement.DefaultState)
            {
                gramophonEnabled = true;
                animator.SetTrigger(GramophoneConstants.PLAY_TRIGGER);
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