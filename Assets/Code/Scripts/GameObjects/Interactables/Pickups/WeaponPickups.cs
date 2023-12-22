using SettingsNS;
using System;
using UnityEngine;
using WeaponManagement;
using WeaponNS;

namespace InteractableNS.Pickups
{
    public class WeaponPickups : Interactable
    {
        public WeaponEntity WeaponEntity;
        public Action ActionIfWeaponTypeIsNotOccupied;
        public Action ActionIfWeaponTypeIsOccupiedBySame;
        public Action ActionIfWeaponTypeIsOccupiedByOther;
        protected virtual new void Start()
        {
            base.Start();
        }
        protected override void Interact()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
            if (weaponManager != null)
            {
                Weapon thisWeapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponEntity == WeaponEntity);
                Weapon currentActiveWeapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponType == thisWeapon.WeaponData.WeaponType && weaponManager.IsActiveWeapon(x.WeaponData));
                if (currentActiveWeapon == null)
                {
                    weaponManager.ChangeActiveWeapon(thisWeapon.WeaponData.WeaponType, thisWeapon.WeaponData);
                    if (GameSettings.ChangeWeaponAfterPickup)
                        weaponManager.ChangeWeaponSelection(thisWeapon.WeaponData.WeaponType);
                    ActionIfWeaponTypeIsNotOccupied?.Invoke();
                }
                else if (thisWeapon == currentActiveWeapon)
                    ActionIfWeaponTypeIsOccupiedBySame?.Invoke();
                else
                {
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
        protected GameObject SwitchPrefabs(GameObject prefab)
        {
            Transform newPropRootTran = transform.root.name == MyConstants.EnironmentConstants.ItemConstants.PropConstants.NEW_PROP_ROOT ?
                transform.root : transform.root.Find(MyConstants.EnironmentConstants.ItemConstants.PropConstants.NEW_PROP_ROOT);
            GameObject propRoot = newPropRootTran == null ? null : newPropRootTran.gameObject;
            if (!propRoot)
                propRoot = new GameObject(MyConstants.EnironmentConstants.ItemConstants.PropConstants.NEW_PROP_ROOT);
            propRoot.transform.parent = transform.parent;
            Transform newPropTran = propRoot.transform.Find(prefab.name);
            GameObject newProp = newPropTran == null ? null : newPropTran.gameObject;
            if (!newProp)
            {
                newProp = GameObject.Instantiate(prefab, transform.position, prefab.transform.rotation, transform.root);
                newProp.name = prefab.name;
            }
            newProp.transform.parent = propRoot.transform;
            gameObject.transform.parent = propRoot.transform;
            ActionIfWeaponTypeIsOccupiedByOther?.Invoke();
            gameObject.SetActive(false);
            newProp.SetActive(true);
            return newProp;
        }
        protected void UnsubscribeFromEvent(Action method, WeaponManager weaponManager)
        {
            // Unsubscribe the method from the event
            weaponManager.OnWeaponChange -= method;
        }
    }
}