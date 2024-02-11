using AYellowpaper;
using ScriptableObjectNS.Weapon.Gun;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityMethodsNS;
using WeaponManagement;
using WeaponNS;
using WeaponNS.ShootingWeaponNS;

namespace HudNS.Weapon.ShootingWeapon
{
	[RequireComponent(typeof(WeaponManagement.WeaponManager))]
	public class ShowAmmo : OnEnableMethodAfterStart
	{
		[Tooltip("Delay for showing ammo on reloading input.")]
		[SerializeField, FormerlySerializedAs("DelayTime")]
		private float delayTime = 1f;
		[SerializeField, RequireInterface(typeof(IMessagePrinter))]
		private MonoBehaviour messagePrinter;

		private float timeSinceLastReloadInput = 0f;
		private WeaponManager weaponManager;

		protected IMessagePrinter MessagePrinter { get => (IMessagePrinter)messagePrinter; }

		private void Awake()
		{
			weaponManager = GetComponent<WeaponManager>();
			MessagePrinter.PrintMessage("", gameObject);
		}
		protected override void OnEnableAfterStart()
		{
			weaponManager.OnWeaponChange += ShowCurrentWeaponAmmo;
			PlayerBattleInput.ReloadInputStarted += ShowAmmoOnReloadingInput;
			ShowCurrentWeaponAmmo();
		}
		private void OnDisable()
		{
			weaponManager.OnWeaponChange -= ShowCurrentWeaponAmmo;
			PlayerBattleInput.ReloadInputStarted -= ShowAmmoOnReloadingInput;
		}
		private void Update()
		{
			timeSinceLastReloadInput += Time.deltaTime;
		}
		private void ShowAmmoOnReloadingInput()
		{
			if (timeSinceLastReloadInput >= delayTime)
			{
				ShowCurrentWeaponAmmo();
				timeSinceLastReloadInput = 0f;
			}
		}
		public void ShowCurrentWeaponAmmo()
		{
			if (weaponManager.ActiveWeapon.CurrentSelectedActiveWeapon != null)
				ShowAmmoLeftOfWeapon(weaponManager.ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.WeaponEntity);
		}
		public void ShowAmmoLeftOfWeapon(WeaponEntity weaponEntity)
		{
			GunData weaponData = weaponManager.Weapons.FirstOrDefault(x => x.WeaponData.WeaponEntity == weaponEntity).WeaponData as GunData;
			if (weaponData)
			{
				string currentAmmo = weaponData.CurrentAmmo >= 10 ? weaponData.CurrentAmmo.ToString() : "0" + weaponData.CurrentAmmo.ToString();
				string reserveAmmo = weaponData.ReserveAmmoData.ReserveAmmo >= 10 ? weaponData.ReserveAmmoData.ReserveAmmo.ToString() : "0" + weaponData.ReserveAmmoData.ReserveAmmo.ToString();
				MessagePrinter.PrintMessage($"{currentAmmo} | {reserveAmmo}", gameObject);
			}
		}
	}
}
