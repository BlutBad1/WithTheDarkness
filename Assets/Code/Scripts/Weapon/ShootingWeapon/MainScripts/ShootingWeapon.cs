using MyConstants.WeaponConstants;
using MyConstants.WeaponConstants.ShootingWeaponConstants;
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
        static bool firstFramePassed = false;
        public delegate void BulletSpread(GunData gunData);
        public BulletSpread OnShootRaycast;
        private void OnEnable()
        {
            if (animator && !animator.GetBool(MainShootingWeaponConstants.RELOADING))
                gunData.Reloading = false;
            timeSinceLastShot = 0;
            if (firstFramePassed)
            {
                PlayerShoot.shootInput += Shoot;
                PlayerShoot.altFireInput += AltFire;
                PlayerShoot.reloadInput += StartReload;
            }
        }
        private void Awake()
        {
            animator = gun.GetComponent<Animator>();
            gunData.CurrentAmmo = gunData.MagSize;
            gunData.Reloading = false;
            timeSinceLastShot = 2;
        }
        private void Start()
        {
            if (!firstFramePassed)
            {
                PlayerShoot.shootInput += Shoot;
                PlayerShoot.altFireInput += AltFire;
                PlayerShoot.reloadInput += StartReload;
            }
        }
        private void Update()
        {
            if (!firstFramePassed)
                firstFramePassed = true;
            timeSinceLastShot += Time.deltaTime;
        }
        private void OnDisable()
        {
            PlayerShoot.shootInput -= Shoot;
            PlayerShoot.altFireInput -= AltFire;
            PlayerShoot.reloadInput -= StartReload;
        }
        public void StartReload()
        {
            if (gunData.CurrentAmmo != gunData.MagSize && gunData.ReserveAmmo != 0)
                if (!gunData.Reloading && !animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))
                    StartCoroutine(Reload());
        }
        public virtual void ReloadAnim() => animator?.SetTrigger(MainShootingWeaponConstants.RELOADING);
        public virtual void ShootRaycast() => OnShootRaycast?.Invoke(gunData);
        public virtual void Shoot()
        {
            if (CanShoot())
            {
                if (animator && (animator.GetBool(MainShootingWeaponConstants.FIRING) || animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))) //if some anim is triggered => false
                    return;
                if (gunData.CurrentAmmo > 0)
                {
                    animator?.SetTrigger(MainShootingWeaponConstants.FIRING);
                    gunData.CurrentAmmo--;
                    timeSinceLastShot = 0;
                    return;
                }
                else if (gunData.CurrentAmmo == 0)
                {
                    animator?.SetTrigger(MainShootingWeaponConstants.OUT_OF_AMMO);
                    timeSinceLastShot = 0;
                    return;
                }
            }
        }
        public void GetAmmo(int ammo) => gunData.ReserveAmmo += ammo;
        public virtual void AltFire() { return; }
        protected bool CanShoot() => !gunData.Reloading && timeSinceLastShot > (2f / (gunData.FireRate / 60f));
        protected virtual IEnumerator Reload()
        {
            gunData.Reloading = true;
            difference = gunData.ReserveAmmo >= (gunData.MagSize - gunData.CurrentAmmo) ? gunData.MagSize - gunData.CurrentAmmo : gunData.ReserveAmmo;
            gunData.ReserveAmmo -= difference;
            gunData.CurrentAmmo += difference;
            ReloadAnim();
            yield return new WaitForSeconds(gunData.ReloadTime);
            gunData.Reloading = false;
        }
    }
}