using MyConstants.WeaponConstants.ShootingWeaponConstants;
using System.Collections;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS.ShotgunNS
{
    [RequireComponent(typeof(Animator))]
    public class Shotgun : ShootingWeapon
    {
        public override void ReloadAnim()
        {
            animator?.SetInteger(MainShootingWeaponConstants.DIFFERENCE, difference);
            animator?.SetTrigger(MainShootingWeaponConstants.RELOADING);
        }
        protected override IEnumerator Reload()
        {
            gunData.Reloading = true;
            difference = gunData.ReserveAmmo >= (gunData.MagSize - gunData.CurrentAmmo) ? gunData.MagSize - gunData.CurrentAmmo : gunData.ReserveAmmo;
            gunData.ReserveAmmo -= difference;
            gunData.CurrentAmmo += difference;
            ReloadAnim();
            yield return new WaitForSeconds(0.1f);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(MainShootingWeaponConstants.IDLE) || animator.GetBool(MainShootingWeaponConstants.RELOADING))
                yield return new WaitForSeconds(0.05f);
            yield return new WaitForSeconds(0.2f);
            gunData.Reloading = false;
        }
        public override void AltFire()
        {
            if (CanShoot())
            {
                if (gunData.CurrentAmmo >= 2)
                {
                    animator?.SetTrigger(MainShootingWeaponConstants.ALT_FIRING);
                    gunData.CurrentAmmo -= 2;
                    timeSinceLastShot = 0;
                    return;
                }
                else
                    Shoot();
            }
        }
        public void ShootAltFireRaycast()
        {
            OnShootRaycast?.Invoke(gunData);
            OnShootRaycast?.Invoke(gunData);
        }
    }
}