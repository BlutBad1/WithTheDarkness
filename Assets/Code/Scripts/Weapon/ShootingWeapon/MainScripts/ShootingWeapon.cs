using ScriptableObjectNS.Weapon.Gun;
using WeaponConstantsNS.ShootingWeaponConstantsNS;
using System.Collections;
using UnityEngine;
using WeaponConstantsNS;

namespace WeaponNS.ShootingWeaponNS
{
	public class ShootingWeapon : MonoBehaviour
	{
		private static bool hasBeenInitialized = false;

		[SerializeField]
		private GunData gunData;
		[SerializeField]
		protected Animator animator;

		protected float timeSinceLastShot;
		protected int difference;

		public delegate void BulletSpread(GunData gunData);
		public BulletSpread OnShootRaycast;

		public GunData GunData { get => gunData; set => gunData = value; }

		private void OnEnable()
		{
			if (animator && !animator.GetBool(ShootingWeaponConstants.RELOADING))
				GunData.Reloading = false;
			timeSinceLastShot = 0;
			if (hasBeenInitialized)
				AttachActions();
		}
		private void Awake()
		{
			if (!animator)
				animator = GetComponent<Animator>();
			GunData.Reloading = false;
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
			if (GunData.CurrentAmmo != GunData.MagSize && GunData.ReserveAmmoData.ReserveAmmo != 0)
				if (!GunData.Reloading && !animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))
					StartCoroutine(Reload());
		}
		public virtual void ReloadAnim() => animator?.SetTrigger(ShootingWeaponConstants.RELOADING);
		public virtual void ShootRaycast() => OnShootRaycast?.Invoke(GunData);
		public virtual void Shoot()
		{
			if (CanShoot())
			{
				if (animator && (animator.GetBool(ShootingWeaponConstants.FIRING) || animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))) //if some anim is triggered => false
					return;
				if (GunData.CurrentAmmo > 0)
				{
					animator?.SetTrigger(ShootingWeaponConstants.FIRING);
					GunData.CurrentAmmo--;
					timeSinceLastShot = 0;
					return;
				}
				else if (GunData.CurrentAmmo == 0)
				{
					animator?.SetTrigger(ShootingWeaponConstants.OUT_OF_AMMO);
					timeSinceLastShot = 0;
					return;
				}
			}
		}
		public void GetAmmo(int ammo) => GunData.ReserveAmmoData.ReserveAmmo += ammo;
		public virtual void AltFire() { return; }
		protected bool CanShoot() => !GunData.Reloading && timeSinceLastShot > (2f / (GunData.FireRate / 60f));
		protected virtual IEnumerator Reload()
		{
			GunData.Reloading = true;
			difference = GunData.ReserveAmmoData.ReserveAmmo >= (GunData.MagSize - GunData.CurrentAmmo) ? GunData.MagSize - GunData.CurrentAmmo : GunData.ReserveAmmoData.ReserveAmmo;
			GunData.ReserveAmmoData.ReserveAmmo -= difference;
			GunData.CurrentAmmo += difference;
			ReloadAnim();
			yield return new WaitForSeconds(GunData.ReloadTime);
			GunData.Reloading = false;
		}
	}
}