using MyConstants.ShootingWeaponConstants;
using MyConstants.WeaponConstants;
using System.Collections;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS
{
    public class ShootingWeapon : MonoBehaviour
    {
        [SerializeField]
        public GunData gunData;
        [SerializeField]
        public GameObject gun;
        protected Animator animator;
        protected float timeSinceLastShot;
        protected int difference;
        public delegate void BulletSpread(GunData gunData);
        public BulletSpread OnShootRaycast;
        static bool firstFramePassed = false;
        private void Start()
        {
            animator = gun.GetComponent<Animator>();
            gunData.currentAmmo = gunData.magSize;
            gunData.reloading = false;
            timeSinceLastShot = 2;
            if (!firstFramePassed)
            {
                PlayerShoot.shootInput += Shoot;
                PlayerShoot.altFireInput += AltFire;
                PlayerShoot.reloadInput += StartReload;
            }
        }
        private void OnDisable()
        {
            PlayerShoot.shootInput -= Shoot;
            PlayerShoot.altFireInput -= AltFire;
            PlayerShoot.reloadInput -= StartReload;
        }
        private void OnEnable()
        {
            if (animator && !animator.GetBool(MainShootingWeaponConstants.RELOADING))
                gunData.reloading = false;
            timeSinceLastShot = 0;
            PlayerShoot.shootInput += Shoot;
            PlayerShoot.altFireInput += AltFire;
            PlayerShoot.reloadInput += StartReload;
        }
        public void StartReload()
        {
            if (gunData.currentAmmo != gunData.magSize && gunData.reserveAmmo != 0)
                if (!gunData.reloading && !animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))
                    StartCoroutine(Reload());
        }
        public virtual void ReloadAnim() => animator?.SetTrigger(MainShootingWeaponConstants.RELOADING);
        protected virtual IEnumerator Reload()
        {
            gunData.reloading = true;
            difference = gunData.reserveAmmo >= (gunData.magSize - gunData.currentAmmo) ? gunData.magSize - gunData.currentAmmo : gunData.reserveAmmo;
            gunData.reserveAmmo -= difference;
            gunData.currentAmmo += difference;
            ReloadAnim();
            yield return new WaitForSeconds(gunData.reloadTime);
            gunData.reloading = false;
        }
        protected bool CanShoot() => !gunData.reloading && timeSinceLastShot > (2f / (gunData.fireRate / 60f));
        public virtual void ShootRaycast() => OnShootRaycast?.Invoke(gunData);
        public virtual void Shoot()
        {
            if (CanShoot())
            {
                if (animator && (animator.GetBool(MainShootingWeaponConstants.FIRING) || animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))) //if some anim is triggered => false
                    return;
                if (gunData.currentAmmo > 0)
                {
                    animator?.SetTrigger(MainShootingWeaponConstants.FIRING);
                    gunData.currentAmmo--;
                    timeSinceLastShot = 0;
                    return;
                }
                else if (gunData.currentAmmo == 0)
                {
                    animator?.SetTrigger(MainShootingWeaponConstants.OUT_OF_AMMO);
                    timeSinceLastShot = 0;
                    return;
                }
            }
        }
        public virtual void AltFire() { return; }
        public void GetAmmo(int ammo) => gunData.reserveAmmo += ammo;
        private void Update()
        {
            if (!firstFramePassed)
                firstFramePassed = true;
            timeSinceLastShot += Time.deltaTime;
        }
    }
}