using SettingsNS;
using System;
using WeaponManagement;
using WeaponNS;

namespace InteractableNS.Pickups
{
    public class WeaponPickups : Interactable
    {
        public WeaponType WeaponType;
        public Action ActionIfWeaponNotUnlocked;
        public Action ActionIfWeaponUnlocked;
        protected virtual new void Start()
        {
            base.Start();
        }
        protected override void Interact()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
            if (weaponManager && !Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponType == WeaponType).WeaponData.IsUnlocked)
            {
                weaponManager.ChangeWeaponLockStatus(WeaponType, true);
                if (GameSettings.ChangeWeaponAfterPickup)
                    weaponManager.ChangeWeaponSelection(WeaponType);
                ActionIfWeaponNotUnlocked?.Invoke();
            }
            else if (weaponManager)
                ActionIfWeaponUnlocked?.Invoke();
        }
    }
}