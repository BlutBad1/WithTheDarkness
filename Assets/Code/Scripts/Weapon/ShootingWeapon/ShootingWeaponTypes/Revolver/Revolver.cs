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
            difference = gunData.ReserveAmmo >= (gunData.MagSize - gunData.CurrentAmmo) ? gunData.MagSize - gunData.CurrentAmmo : gunData.ReserveAmmo;
            gunData.ReserveAmmo -= difference;
            gunData.CurrentAmmo += difference;
            gunData.Reloading = true;
            ReloadAnim();
            yield return new WaitForSeconds(1.40f + (difference * gunData.ReloadTime));
        }
        public override void ReloadAnim()
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (gunData.Reloading && info.IsName(RevolverConstants.RELOADING_ENDING))
            {
                animator.SetTrigger(RevolverConstants.RELOADING_ENDING);
                lamp.GetComponent<Animator>().SetTrigger(RevolverConstants.RELOADING_ENDING);
                gunData.Reloading = false;
            }
            else if (gunData.Reloading)
            {
                base.ReloadAnim();
                lamp.GetComponent<Animator>().SetTrigger(MainShootingWeaponConstants.RELOADING);
                animator.SetFloat(RevolverConstants.RELOADING_ANIMATION_SPEED, 1 / (difference * gunData.ReloadTime));
                lamp.GetComponent<Animator>().SetFloat(RevolverConstants.RELOADING_ANIMATION_SPEED, 1 / (difference * gunData.ReloadTime));
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