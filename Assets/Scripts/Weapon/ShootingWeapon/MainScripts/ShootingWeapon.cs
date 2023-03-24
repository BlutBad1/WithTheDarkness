using PoolableObjectsNS;
using System.Collections;
using UnityEngine;


namespace WeaponNS.ShootingWeaponNS
{
    public class ShootingWeapon : MonoBehaviour
    {
        [SerializeField]
        protected GunData gunData;
        [SerializeField]
        protected GameObject gun;
        protected Animator animator;
        float timeSinceLastShot;
        protected int difference;
        protected const string RELOADING = "Reloading";
        protected const string FIRING = "Firing";
        protected const string OUT_OF_AMMO = "OutOfAmmo";
        protected const string IDLE = "Idle";
        public delegate void BulletSpread(GunData gunData);
        public BulletSpread OnBulletSpread;
        private void Start()
        {

            PlayerShoot.shootInput += Shoot;
            PlayerShoot.reloadInput += StartReload;
            animator = gun.GetComponent<Animator>();
            gunData.currentAmmo = gunData.magSize;
            gunData.reloading = false;
        }


        public void StartReload()
        {
            if (gunData.currentAmmo != gunData.magSize && gunData.reserveAmmo != 0)
            {
                if (!gunData.reloading)
                {
                    StartCoroutine(Reload());
                }
            }

        }

        public virtual void ReloadAnim()
        {
            animator?.SetTrigger(RELOADING);

        }




        protected virtual IEnumerator Reload()
        {
            difference = gunData.reserveAmmo >= (gunData.magSize - gunData.currentAmmo) ? gunData.magSize - gunData.currentAmmo : gunData.reserveAmmo;
            gunData.reserveAmmo -= difference;
            gunData.currentAmmo += difference;
            gunData.reloading = true;
            ReloadAnim();

            yield return new WaitForSeconds(gunData.reloadTime);
            gunData.reloading = false;

        }
        private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 2f / (gunData.fireRate / 60f);
     
        public virtual void ShootRaycast()
        {
            OnBulletSpread?.Invoke(gunData);

        }
        public virtual void Shoot()
        {
            if (CanShoot())
            {
                if (gunData.currentAmmo > 0)
                {

                    animator?.SetTrigger(FIRING);
                    gunData.currentAmmo--;
                    timeSinceLastShot = 0;
                    return;

                }
                else if (gunData.currentAmmo == 0)
                {


                    animator?.SetTrigger(OUT_OF_AMMO);
                    timeSinceLastShot = 0;


                }
            }
        }
        public void GetAmmo(int ammo) { gunData.reserveAmmo += ammo; }


        private void Update()
        {

            timeSinceLastShot += Time.deltaTime;
        }
    }
}