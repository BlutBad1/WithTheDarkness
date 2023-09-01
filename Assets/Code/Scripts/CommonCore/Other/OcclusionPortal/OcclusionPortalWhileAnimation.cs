using UnityEngine;
namespace OptimizationNS
{
    public class OcclusionPortalWhileAnimation : OcclusionPortalManager
    {
        public Animator Animator;
        public AnimationClip OpenPortalAnimation;
        public AnimationClip ClosePortalAnimation;
        [Min(0)]
        public int AnimationLayerIndex = 0;
        public bool OpenPortalWhileInTransition = false;
        public bool ClosePortalWhileInTransition = false;
        public override void ClosePortal()
        {
            if (!ClosePortalWhileInTransition && Animator.GetCurrentAnimatorStateInfo(AnimationLayerIndex).IsName(ClosePortalAnimation.name) && !Animator.IsInTransition(AnimationLayerIndex))
                base.ClosePortal();
            else if (ClosePortalWhileInTransition && Animator.GetAnimatorTransitionInfo(AnimationLayerIndex).IsName(OpenPortalAnimation.name + " -> " + ClosePortalAnimation.name))
                base.ClosePortal();
        }
        public override void OpenPortal()
        {
            if (!OpenPortalWhileInTransition && Animator.GetCurrentAnimatorStateInfo(AnimationLayerIndex).IsName(OpenPortalAnimation.name) && !Animator.IsInTransition(AnimationLayerIndex))
                base.OpenPortal();
            else if (OpenPortalWhileInTransition && Animator.GetAnimatorTransitionInfo(AnimationLayerIndex).IsName(ClosePortalAnimation.name + " -> " + OpenPortalAnimation.name))
                base.OpenPortal();
        }
    }
}