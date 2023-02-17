using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : ShotingWeapon
{
    protected const string RELOADING_DELAY = "RelodingDelay";
    protected const string RELOADING_ANIMATION_SPEED = "ReloadingSpeed";
    protected const string RELOADING_ENDING = "ReloadingEnding";
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
        Debug.Log(gunData.reloading);
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0); 
        if (gunData.reloading && info.IsName(RELOADING_ENDING))
        {
          
            animator.SetTrigger(RELOADING_ENDING);
            lamp.GetComponent<Animator>().SetTrigger(RELOADING_ENDING);
            gunData.reloading = false;
        }
        else if (gunData.reloading)
        {
            base.ReloadAnim();
            lamp.GetComponent<Animator>().SetTrigger(RELOADING);
            animator.SetFloat(RELOADING_ANIMATION_SPEED, 1 / (difference * gunData.reloadTime));
            lamp.GetComponent<Animator>().SetFloat(RELOADING_ANIMATION_SPEED, 1 / (difference * gunData.reloadTime));

        }

   
       



    }

    public void ReloadingSound()
    {
       
        GetComponent<AudioManager>().PlayAFewTimes(new string[2] { "RevolverReloading", "RevolverCylinder" }, difference);
      
    }
    public void RelodingDelay()
    {
        animator.SetBool(RELOADING_DELAY, !animator.GetBool(RELOADING_DELAY));
        lamp.GetComponent<Animator>().SetBool(RELOADING_DELAY, !lamp.GetComponent<Animator>().GetBool(RELOADING_DELAY));

    }
}
