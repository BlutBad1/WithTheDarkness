using UnityEngine;
namespace OptimizationNS
{
    public class OcclusionPortalWhileAnimation : OcclusionPortalManager
    {
        public Animator Animator;
        public string OpenPortalAnimatorStateName;
        public string ClosePortalAnimatorStateName;
        [Min(0)]
        public int AnimationLayerIndex = 0;
        [Tooltip("True - opens portal only if anim in transition; False - opens only if anim out of transition")]
        public bool OpenPortalOnlyWhileInTransition = false;
        [Tooltip("True - closes portal only if anim in transition; False - closes only if anim out of transition")]
        public bool ClosePortalOnlyWhileInTransition = false;
        public override void ClosePortal()
        {
            if (MatchConditions(ClosePortalOnlyWhileInTransition, OpenPortalAnimatorStateName, ClosePortalAnimatorStateName))
                base.ClosePortal();
        }
        public override void OpenPortal()
        {
            if (MatchConditions(OpenPortalOnlyWhileInTransition, ClosePortalAnimatorStateName, OpenPortalAnimatorStateName))
                base.OpenPortal();
        }
        private bool MatchConditions(bool portalWhileinTransition, string animatorStateFrom, string animatorStateTo)
        {
            if (!portalWhileinTransition && Animator.GetCurrentAnimatorStateInfo(AnimationLayerIndex).IsName(animatorStateTo) && !Animator.IsInTransition(AnimationLayerIndex))
                return true;
            else if (portalWhileinTransition && Animator.GetAnimatorTransitionInfo(AnimationLayerIndex).IsName(animatorStateFrom + " -> " + animatorStateTo))
                return true;
            return false;
        }
    }
}