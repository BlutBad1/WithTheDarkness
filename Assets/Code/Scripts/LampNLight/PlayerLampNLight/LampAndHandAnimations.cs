using LampConstantsNS;
using PlayerScriptsNS;
using System;
using UnityEngine;
using WeaponConstantsNS;

namespace LightNS.Player
{
	public class LampAndHandAnimations : MonoBehaviour
	{
		[SerializeField]
		private PlayerMotor characterController;
		[SerializeField]
		private Animator leftHandAnimator;
		public Action OnLampReloading;
		public Action OnLampAfterReloading;
		void Update()
		{
			float speed = characterController.CurrentScaledByTimeVelocity.magnitude;
			leftHandAnimator.SetFloat(LampConstants.SPEED, speed);
		}
		public void LampReloading() =>
			OnLampReloading?.Invoke();
		public void LampAfterReloading() =>
		  OnLampAfterReloading?.Invoke();
		public void EnableLightUp()
		{
			leftHandAnimator.SetBool(LampConstants.IS_LIGHT_UP, true);
			leftHandAnimator.SetTrigger(LampConstants.LIGHT_UP);
		}
		public void DisableLightUp() =>
			leftHandAnimator.SetBool(LampConstants.IS_LIGHT_UP, false);
	}
}