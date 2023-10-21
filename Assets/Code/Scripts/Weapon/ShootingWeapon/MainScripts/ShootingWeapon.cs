using MyConstants.WeaponConstants;
using MyConstants.WeaponConstants.ShootingWeaponConstants;
using ScriptableObjectNS.Weapon.Gun;
using System.Collections;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS
{
    public class ShootingWeapon : MonoBehaviour
    {
        [SerializeField]
        public GunData gunData;
        [SerializeField]
        protected Animator animator;
        protected float timeSinceLastShot;
        protected int difference;
        public delegate void BulletSpread(GunData gunData);
        public BulletSpread OnShootRaycast;
        private static bool hasBeenInitialized = false;
        private void OnEnable()
        {
            if (animator && !animator.GetBool(MainShootingWeaponConstants.RELOADING))
                gunData.Reloading = false;
            timeSinceLastShot = 0;
            if (hasBeenInitialized)
                AttachActions();
        }
        private void Awake()
        {
            if (!animator)
                animator = GetComponent<Animator>();
            gunData.CurrentAmmo = gunData.MagSize;
            gunData.Reloading = false;
            timeSinceLastShot = 2;
        }
        private void Start()
        {
            if (!hasBeenInitialized)
                AttachActions();
            hasBeenInitialized = true;
        }
        private void OnDisable() =>
            DettachActions();
        private void Update() =>
            timeSinceLastShot += Time.deltaTime;
        public void AttachActions()
        {
            PlayerBattleInput.AttackInputStarted += Shoot;
            PlayerBattleInput.AltAttackInputStarted += AltFire;
            PlayerBattleInput.ReloadInputStarted += StartReload;
        }
        public void DettachActions()
        {
            PlayerBattleInput.AttackInputStarted -= Shoot;
            PlayerBattleInput.AltAttackInputStarted -= AltFire;
            PlayerBattleInput.ReloadInputStarted -= StartReload;
        }
        public void StartReload()
        {
            if (gunData.CurrentAmmo != gunData.MagSize && gunData.ReserveAmmo != 0)
                if (!gunData.Reloading && !animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN))
                    StartCoroutine(Reload());
        }
        public virtual void ReloadAnim() => animator?.SetTrigger(MainShootingWeaponConstants.RELOADING);
        public virtual void ShootRaycast() => OnShootRaycast?.Invoke(gunData);
        public virtual void Shoot()
        {
            if (CanShoot())
            {
                if (animator && (animator.GetBool(MainShootingWeaponConstants.FIRING) || animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN))) //if some anim is triggered => false
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