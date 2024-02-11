using HUDConstantsNS;
using HudNS;
using HudNS.Weapon.ShootingWeapon;
using ScriptableObjectNS.Weapon.Gun;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UtilitiesNS;
using WeaponManagement;

namespace InteractableNS.Pickups
{
	public class ShootingWeaponPickups : WeaponPickups
	{
		[SerializeField, MinMaxSlider(0, 1000), FormerlySerializedAs("MinMaxAmountOfBulletToAdd")]
		private Vector2 minMaxAmountOfBulletToAdd;
		[SerializeField]
		private float disapperingSpeed = 0.8f;
		[SerializeField, FormerlySerializedAs("IfWeaponNotUnlocked")]
		private UnityEvent ifWeaponNotUnlocked;
		[SerializeField, FormerlySerializedAs("IfWeaponAlreadyUnlocked")]
		private UnityEvent ifWeaponAlreadyUnlocked;
		[SerializeField, FormerlySerializedAs("IfWeaponTypeIsOccupiedByOther")]
		private UnityEvent ifWeaponTypeIsOccupiedByOther;

		protected override void Start()
		{
			base.Start();
			actionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlocked;
			actionIfWeaponTypeIsNotOccupied += OnNotUnlocked;
			actionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlockedUnityEvent;
			actionIfWeaponTypeIsOccupiedByOther += OnOccupiedByOther;
		}
		protected void OnNotUnlocked() =>
			ifWeaponNotUnlocked?.Invoke();
		protected void OnAlreadyUnlockedUnityEvent() =>
			ifWeaponAlreadyUnlocked?.Invoke();
		protected void OnOccupiedByOther() =>
			ifWeaponTypeIsOccupiedByOther?.Invoke();
		protected void OnAlreadyUnlocked()
		{
			WeaponManager weaponManager = Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
			GunData gunData = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponEntity == weaponEntity).WeaponData as GunData;
			if (gunData)
			{
				int amountOfBulletsToAdd = (int)UnityEngine.Random.Range(minMaxAmountOfBulletToAdd.x, minMaxAmountOfBulletToAdd.y);
				gunData.ReserveAmmoData.ReserveAmmo += amountOfBulletsToAdd;
				HUDConstants.AmmoGetMessagePrinter.PrintMessage(amountOfBulletsToAdd.ToString(), gameObject);
				HUDConstants.AmmoPrinter.PrintMessage(weaponEntity, gameObject);
			}
		}
	}
}