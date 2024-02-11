using WeaponConstantsNS.ShootingWeaponConstantsNS;
using SoundNS;
using System.Collections;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS.RevolverNS
{
	public class Revolver : ShootingWeapon
	{
		[SerializeField]
		private GameObject lamp;
		protected override IEnumerator Reload()
		{
			difference = GunData.ReserveAmmoData.ReserveAmmo >= (GunData.MagSize - GunData.CurrentAmmo) ? GunData.MagSize - GunData.CurrentAmmo : GunData.ReserveAmmoData.ReserveAmmo;
			GunData.ReserveAmmoData.ReserveAmmo -= difference;
			GunData.CurrentAmmo += difference;
			GunData.Reloading = true;
			ReloadAnim();
			yield return new WaitForSeconds(1.40f + (difference * GunData.ReloadTime));
		}
		public override void ReloadAnim()
		{
			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
			if (GunData.Reloading && info.IsName(RevolverConstants.RELOADING_ENDING))
			{
				animator.SetTrigger(RevolverConstants.RELOADING_ENDING);
				lamp.GetComponent<Animator>().SetTrigger(RevolverConstants.RELOADING_ENDING);
				GunData.Reloading = false;
			}
			else if (GunData.Reloading)
			{
				base.ReloadAnim();
				lamp.GetComponent<Animator>().SetTrigger(ShootingWeaponConstants.RELOADING);
				animator.SetFloat(RevolverConstants.RELOADING_ANIMATION_SPEED, 1 / (difference * GunData.ReloadTime));
				lamp.GetComponent<Animator>().SetFloat(RevolverConstants.RELOADING_ANIMATION_SPEED, 1 / (difference * GunData.ReloadTime));
			}
		}
		public void ReloadingSound()
		{
			GetComponent<AudioManager>().PlayAFewTimes(new string[2] { RevolverConstants.REVOLVER_RELOADING_SOUND, RevolverConstants.REVOLVER_RELOADING_CYLINDER_SOUND }, difference);
		}
		public void RelodingDelay()
		{
			animator.SetBool(RevolverConstants.RELOADING_DELAY, !animator.GetBool(RevolverConstants.RELOADING_DELAY));
			lamp.GetComponent<Animator>().SetBool(RevolverConstants.RELOADING_DELAY, !lamp.GetComponent<Animator>().GetBool(RevolverConstants.RELOADING_DELAY));
		}
	}
}