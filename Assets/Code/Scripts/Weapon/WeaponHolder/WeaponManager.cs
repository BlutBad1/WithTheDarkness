using ScriptableObjectNS.Weapon;
using SettingsNS;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponConstantsNS;
using WeaponNS;

namespace WeaponManagement
{
	[System.Serializable]
	public class Weapon
	{
		public WeaponData WeaponData;
		public GameObject WeaponGameObject;
	}
	public class WeaponManager : MonoBehaviour
	{
		[Header("Lamp")]
		[SerializeField, FormerlySerializedAs("MainSpotLight")]
		private GameObject mainSpotLight;
		[SerializeField, FormerlySerializedAs("Lamp")]
		private GameObject lamp;
		[Header("All Weapon")]
		[SerializeField, FormerlySerializedAs("Weapons")]
		private Weapon[] weapons;
		[Header("Active Weapon")]
		[SerializeField, FormerlySerializedAs("ActiveWeapon")]
		private ActiveWeapon activeWeapon;//null is default state without any active weapon to use
		[SerializeField, FormerlySerializedAs("UseLampAsDefault")]
		private bool useLampAsDefault = true;

		public Action OnWeaponChange;

		public GameObject MainSpotLight { get => mainSpotLight; }
		public GameObject Lamp { get => lamp; }
		public ActiveWeapon ActiveWeapon { get => activeWeapon; }
		public Weapon[] Weapons { get => weapons; private set => weapons = value; }

		private void Awake()
		{
			if (Weapons == null)
				Weapons = new Weapon[0];
			DefineSelection();
		}
		public Weapon GetCurrentSelectedWeapon() =>
		   ActiveWeapon.CurrentSelectedActiveWeapon;
		public void ChangeActiveWeapon(WeaponType weaponType, WeaponData weaponData)
		{
			Weapon weapon = Weapons.First(x => x.WeaponData == weaponData);
			if (weapon != null)
			{
				switch (weaponType)
				{
					case WeaponType.Melee:
						ActiveWeapon.ActiveWeapons.ActiveMeleeWeapon = weapon.WeaponData;
						break;
					case WeaponType.OneHandedGun:
						ActiveWeapon.ActiveWeapons.ActiveOneHandedGun = weapon.WeaponData;
						break;
					case WeaponType.TwoHandedGun:
						ActiveWeapon.ActiveWeapons.ActiveTwoHandedGun = weapon.WeaponData;
						break;
					default:
						break;
				}
				if (ActiveWeapon.CurrentSelectedActiveWeapon == null || ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.WeaponType == weaponType || GameSettings.ChangeWeaponAfterPickup)
					ChangeWeaponSelection(weapon);
			}
#if UNITY_EDITOR
			else
				Debug.LogWarning("Weapon is not found!");
#endif
		}
		public bool IsActiveWeapon(WeaponData weaponData) =>
			weaponData == ActiveWeapon.ActiveWeapons.ActiveMeleeWeapon || weaponData == ActiveWeapon.ActiveWeapons.ActiveOneHandedGun || weaponData == ActiveWeapon.ActiveWeapons.ActiveTwoHandedGun;
		//For player button weapon change 
		public void ChangeActiveWeaponSelection(WeaponType weaponType)
		{
			Weapon newWeapon = Weapons.FirstOrDefault(x => x.WeaponData.WeaponType == weaponType && IsActiveWeapon(x.WeaponData));
			if (newWeapon != ActiveWeapon.CurrentSelectedActiveWeapon)
				ChangeWeaponSelection(newWeapon);
		}
		private void ChangeWeaponSelection(Weapon newWeapon)
		{
			if (newWeapon != null)
				StartCoroutine(ChangeWeaponCoroutine(newWeapon));
		}
		private void DefineSelection()
		{
			foreach (var item in Array.FindAll(Weapons, weapon => weapon.WeaponGameObject.activeInHierarchy && !IsActiveWeapon(weapon.WeaponData)))
				item.WeaponGameObject.SetActive(false);
			Weapon[] activeWeapon = Array.FindAll(Weapons, weapon => IsActiveWeapon(weapon.WeaponData));
			if (activeWeapon != null && activeWeapon.Length > 0)
			{
				this.ActiveWeapon.CurrentSelectedActiveWeapon = Array.Find(Weapons, weapon => this.ActiveWeapon.CurrentSelectedActiveWeapon == activeWeapon[0]) == null
					? Array.Find(Weapons, weapon => weapon == activeWeapon[0])
					: this.ActiveWeapon.CurrentSelectedActiveWeapon;
				if (activeWeapon?.Length > 1)
				{
					for (int i = 0; i < activeWeapon.Length; i++)
					{
						if (activeWeapon[i] != this.ActiveWeapon.CurrentSelectedActiveWeapon)
							activeWeapon[i].WeaponGameObject.SetActive(false);
					}
				}
			}
			else
				this.ActiveWeapon.CurrentSelectedActiveWeapon = null;
			if (this.ActiveWeapon.CurrentSelectedActiveWeapon != null)
			{
				this.ActiveWeapon.CurrentSelectedActiveWeapon.WeaponGameObject.SetActive(true);
				if (this.ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.IsTwoHanded)
					Lamp.SetActive(false);
				else
					Lamp.SetActive(true);
			}
			else
				Lamp.SetActive(useLampAsDefault);
		}
		private IEnumerator ChangeWeaponCoroutine(Weapon newWeapon)
		{
			Animator lampAnimator = Lamp.GetComponent<Animator>(), currentWeaponAnimator = ActiveWeapon.CurrentSelectedActiveWeapon?.WeaponGameObject.GetComponent<Animator>();
			if (ActiveWeapon.CurrentSelectedActiveWeapon != null)
				currentWeaponAnimator.SetTrigger(WeaponConstants.PUTTING_DOWN);
			if (newWeapon.WeaponData.IsTwoHanded && Lamp.activeInHierarchy)
			{
				lampAnimator.SetTrigger(WeaponConstants.PUTTING_DOWN);
				while (!lampAnimator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))
					yield return null;
			}
			while (ActiveWeapon.CurrentSelectedActiveWeapon != null && ActiveWeapon.CurrentSelectedActiveWeapon.WeaponGameObject.activeInHierarchy && !currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))
				yield return null;
			while ((lampAnimator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) && lampAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
				|| (ActiveWeapon.CurrentSelectedActiveWeapon != null && currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) && currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
			{
				yield return new WaitForSeconds(0.1f);
			}
			if (newWeapon.WeaponData.IsTwoHanded)
				Lamp.SetActive(false);
			else
				Lamp.SetActive(true);
			if (ActiveWeapon.CurrentSelectedActiveWeapon != null)
				ActiveWeapon.CurrentSelectedActiveWeapon.WeaponGameObject.SetActive(false);
			ActiveWeapon.CurrentSelectedActiveWeapon = newWeapon;
			newWeapon.WeaponGameObject.SetActive(true);
			OnWeaponChange?.Invoke();
		}
	}
}