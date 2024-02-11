using GameObjectsConstantsNS.PickupsConstantsNS;
using InteractableNS.Pickups.PropNS;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponManagement;
using WeaponNS;

namespace InteractableNS.Pickups
{
	public class WeaponPickups : Interactable
	{
		[SerializeField, FormerlySerializedAs("WeaponEntity")]
		protected WeaponEntity weaponEntity;
		[SerializeField, FormerlySerializedAs("Prop")]
		protected Prop prop;

		protected Action actionIfWeaponTypeIsNotOccupied;
		protected Action actionIfWeaponTypeIsOccupiedBySame;
		protected Action actionIfWeaponTypeIsOccupiedByOther;

		protected virtual new void Start()
		{
			base.Start();
		}
		protected override void Interact()
		{
			WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
			if (weaponManager != null)
			{
				Weapon thisWeapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponEntity == weaponEntity);
				Weapon currentActiveWeapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponType == thisWeapon.WeaponData.WeaponType && weaponManager.IsActiveWeapon(x.WeaponData));
				if (currentActiveWeapon == null)
				{
					weaponManager.ChangeActiveWeapon(thisWeapon.WeaponData.WeaponType, thisWeapon.WeaponData);
					actionIfWeaponTypeIsNotOccupied?.Invoke();
				}
				else if (thisWeapon == currentActiveWeapon)
					actionIfWeaponTypeIsOccupiedBySame?.Invoke();
				else
				{
					prop.PropBody.SetActive(false);
					Action methodToExecute = null;
					methodToExecute = () =>
					{
						SwitchPrefabs(currentActiveWeapon.WeaponData.WeaponPrefab);
						// Unsubscribe after execution
						UnsubscribeFromEvent(methodToExecute, weaponManager);
					};
					weaponManager.OnWeaponChange += methodToExecute;
					weaponManager.ChangeActiveWeapon(thisWeapon.WeaponData.WeaponType, thisWeapon.WeaponData);
				}
			}
		}
		protected virtual GameObject SwitchPrefabs(GameObject prefab)
		{
			Transform newPropRootTran = transform.root.name == PickupsConstants.NEW_PROP_ROOT ?
				transform.root : transform.root.Find(PickupsConstants.NEW_PROP_ROOT);
			GameObject propRoot = newPropRootTran == null ? null : newPropRootTran.gameObject;
			if (!propRoot)
				propRoot = new GameObject(PickupsConstants.NEW_PROP_ROOT);
			propRoot.transform.parent = transform.parent;
			Prop[] props = UtilitiesNS.Utilities.FindAllComponentsInGameObject<Prop>(propRoot, true).ToArray();
			Transform newPropTran = null;
			foreach (var prop in props)
			{
				if (prop.name.Contains(prefab.name) && prop != this.prop)
				{
					prop.PropBody.SetActive(true);
					newPropTran = prop.gameObject.transform;
				}
			}
			GameObject newProp = newPropTran == null ? null : newPropTran.gameObject;
			if (!newProp)
			{
				newProp = GameObject.Instantiate(prefab, transform.position, transform.rotation, transform.root);
				newProp.name = prefab.name;
			}
			newProp.transform.parent = propRoot.transform;
			transform.parent = propRoot.transform;
			actionIfWeaponTypeIsOccupiedByOther?.Invoke();
			newProp.SetActive(true);
			gameObject.SetActive(false);
			return newProp;
		}
		protected void UnsubscribeFromEvent(Action method, WeaponManager weaponManager)
		{
			// Unsubscribe the method from the event
			weaponManager.OnWeaponChange -= method;
		}
	}
}