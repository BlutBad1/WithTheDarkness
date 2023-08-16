using MyConstants.WeaponConstants.ShootingWeaponConstants;
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
            difference = gunData.reserveAmmo >= (gunData.magSize - gunData.currentAmmo) ? gunData.magSize - gunData.currentAmmo : gunData.reserveAmmo;
            gunData.reserveAmmo -= difference;
            gunData.currentAmmo += difference;
            gunData.reloading = true;
            ReloadAnim();
            yield return new WaitForSeconds(1.40f + (difference * gunData.reloadTime));
        }
        public override void ReloadAnim()
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (gunData.reloading && info.IsName(RevolverConstants.RELOADING_ENDING))
            {
                animator.SetTrigger(RevolverConstants.RELOADING_ENDING);
                lamp.GetComponent<Animator>().SetTrigger(RevolverConstants.RELOADING_ENDING);
                gunData.reloading = false;
            }
            else if (gunData.reloading)
            {
                base.ReloadAnim();
                lamp.GetComponent<Animator>().SetTrigger(MainShootingWeaponConstants.RELOADING);
                animator.SetFloat(RevolverConstants.RELOADING_ANIMATION_SPEED, 1 / (difference * gunData.reloadTime));
                lamp.GetComponent<Animator>().SetFloat(RevolverConstants.RELOADING_ANIMATION_SPEED, 1 / (difference * gunData.reloadTime));
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