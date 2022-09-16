using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;
    [SerializeField] GameObject bulletHole;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject lamp;
    [SerializeField]
    private AudioManager audioManager;
    float timeSinceLastShot;
    int difference;
    [SerializeField]
   
  

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }
    public void StartReload()
    {
        if (gunData.currentAmmo!=gunData.magSize&&gunData.reserveAmmo!=0)
        {
            if (!gunData.reloading)
            {
                StartCoroutine(Reload());
            }
        }
       
    }
    public void ReloadingSound(){audioManager.PlayAFewTimes("RevolverReloading","RevolverCylinder",difference);
        
    }
    public void ReloadAnim(float difference)
    {
        gun.GetComponent<Animator>().SetBool("Reloading", !gun.GetComponent<Animator>().GetBool("Reloading"));
        lamp.GetComponent<Animator>().SetBool("Reloading", !lamp.GetComponent<Animator>().GetBool("Reloading"));
        gun.GetComponent<Animator>().SetFloat("AnimSpeed", 1 / (difference * gunData.reloadTime));
        lamp.GetComponent<Animator>().SetFloat("AnimSpeed", 1 / (difference * gunData.reloadTime));
       
    }


    public void Timeconsububle()
    {
        lamp.GetComponent<Animator>().SetBool("Timeconsububle", !lamp.GetComponent<Animator>().GetBool("Timeconsububle"));
        gun.GetComponent<Animator>().SetBool("Timeconsububle", !gun.GetComponent<Animator>().GetBool("Timeconsububle")); 
    }
    
    private IEnumerator Reload()
    {
         difference = gunData.reserveAmmo >= (gunData.magSize - gunData.currentAmmo) ? gunData.magSize - gunData.currentAmmo : gunData.reserveAmmo;
        gunData.reserveAmmo -= difference;
        gunData.currentAmmo += difference;
        gunData.reloading = true;
        ReloadAnim(difference);
        //1.4 = ReloadAnimOn+ReloadAnimOff
        yield return new WaitForSeconds(1.40f+(difference*gunData.reloadTime));
        gun.GetComponent<Animator>().SetInteger("Ammo", gunData.currentAmmo);
        //ChangeWeaponReloadStatus disables reloading (gunData.reloading = false)
    }
    private bool CanShoot() => !gunData.reloading && timeSinceLastShot >2f / (gunData.fireRate / 60f);
    public void ShootRaycast()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, gunData.maxDistance,~(1<<20)))
        {

            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            damageable?.TakeDamage(gunData.damage);
            GameObject t_newHole = Instantiate(bulletHole, hitInfo.point + hitInfo.normal * 0.0001f, Quaternion.LookRotation(hitInfo.normal));



        }

    }
    public void Shoot()
    {
     
        if (gunData.currentAmmo>0)
        {
            
            if (CanShoot())
            {
              
               
                gun.GetComponent<Animator>().SetBool("Firing", true);


                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                return;
                
                
            }
        }
      
        if (gunData.currentAmmo == 0)
        {        
               gun.GetComponent<Animator>().SetInteger("Ammo", gunData.currentAmmo);
               gun.GetComponent<Animator>().SetBool("Firing", true); 
          
        }
    }
    public void GetAmmo(int ammo){gunData.reserveAmmo += ammo;}
    private void FixedUpdate()
    {
       AnimatorStateInfo info = gun.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
       if (info.IsName("Firing")|| info.IsName("OutOfAmmo")) { gun.GetComponent<Animator>().SetBool("Firing", false); }

    }
  
    private void Update()
    {

        timeSinceLastShot += Time.deltaTime;   
    }
  
}
