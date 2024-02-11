using WeaponConstantsNS.ShootingWeaponConstantsNS;
using System.Collections;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS.ShotgunNS
{
	[RequireComponent(typeof(Animator))]
	public class Shotgun : ShootingWeapon
	{
		public override void ReloadAnim()
		{
			animator?.SetInteger(ShootingWeaponConstants.DIFFERENCE, difference);
			animator?.SetTrigger(ShootingWeaponConstants.RELOADING);
		}
		protected override IEnumerator Reload()
		{
			GunData.Reloading = true;
			difference = GunData.ReserveAmmoData.ReserveAmmo >= (GunData.MagSize - GunData.CurrentAmmo) ? GunData.MagSize - GunData.CurrentAmmo : GunData.ReserveAmmoData.ReserveAmmo;
			GunData.ReserveAmmoData.ReserveAmmo -= difference;
			GunData.CurrentAmmo += difference;
			ReloadAnim();
			yield return new WaitForSeconds(0.1f);
			while (!animator.GetCurrentAnimatorStateInfo(0).IsName(ShootingWeaponConstants.IDLE) || animator.GetBool(ShootingWeaponConstants.RELOADING))
				yield return new WaitForSeconds(0.05f);
			yield return new WaitForSeconds(0.2f);
			GunData.Reloading = false;
		}
		public override void AltFire()
		{
			if (CanShoot())
			{
				if (GunData.CurrentAmmo >= 2)
				{
					animator?.SetTrigger(ShootingWeaponConstants.ALT_FIRING);
					GunData.CurrentAmmo -= 2;
					timeSinceLastShot = 0;
					return;
				}
				else
					Shoot();
			}
		}
		public void ShootAltFireRaycast()
		{
			OnShootRaycast?.Invoke(GunData);
			OnShootRaycast?.Invoke(GunData);
		}
	}
}