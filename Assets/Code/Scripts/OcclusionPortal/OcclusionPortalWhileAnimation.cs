using UnityEngine;
using UnityEngine.Serialization;

namespace OptimizationNS
{
	public class OcclusionPortalWhileAnimation : OcclusionPortalManager
	{
		[SerializeField, FormerlySerializedAs("Animator")]
		public Animator animator;
		[SerializeField, FormerlySerializedAs("OpenPortalAnimatorStateName")]
		public string openPortalAnimatorStateName;
		[SerializeField, FormerlySerializedAs("ClosePortalAnimatorStateName")]
		public string closePortalAnimatorStateName;
		[SerializeField, FormerlySerializedAs("AnimationLayerIndex"), Min(0)]
		public int animationLayerIndex = 0;
		[Tooltip("True - opens portal only if anim in transition; False - opens only if anim out of transition")]
		[SerializeField, FormerlySerializedAs("OpenPortalOnlyWhileInTransition")]
		public bool openPortalOnlyWhileInTransition = false;
		[Tooltip("True - closes portal only if anim in transition; False - closes only if anim out of transition")]
		[SerializeField, FormerlySerializedAs("ClosePortalOnlyWhileInTransition")]
		public bool closePortalOnlyWhileInTransition = false;

		public override void ClosePortal()
		{
			if (MatchConditions(closePortalOnlyWhileInTransition, openPortalAnimatorStateName, closePortalAnimatorStateName))
				base.ClosePortal();
		}
		public override void OpenPortal()
		{
			if (MatchConditions(openPortalOnlyWhileInTransition, closePortalAnimatorStateName, openPortalAnimatorStateName))
				base.OpenPortal();
		}
		private bool MatchConditions(bool portalWhileinTransition, string animatorStateFrom, string animatorStateTo)
		{
			if (!portalWhileinTransition && animator.GetCurrentAnimatorStateInfo(animationLayerIndex).IsName(animatorStateTo) && !animator.IsInTransition(animationLayerIndex))
				return true;
			else if (portalWhileinTransition && animator.GetAnimatorTransitionInfo(animationLayerIndex).IsName(animatorStateFrom + " -> " + animatorStateTo))
				return true;
			return false;
		}
	}
}