using UnityEngine;
using WeaponManagement;

namespace InteractableNS.Pickups
{
    public class WeaponPickups : Interactable
    {
        private WeaponManager weaponManager;
        public string WeaponNameToPickup;
        public bool ChangeWeaponAfterPickup = true;
        protected override void Start()
        {
            base.Start();
            weaponManager = GameObject.Find(MyConstants.CommonConstants.WEAPON_HOLDER).GetComponent<WeaponManager>();
            ChangeWeaponAfterPickup = SettingsNS.GameSettings.ChangeWeaponAfterPickup;
            SettingsNS.GameSettings.OnSwitchWeaponOnPickUpChange += CheckChangeWeaponAfterPickupStatus;
        }
        public void CheckChangeWeaponAfterPickupStatus() =>
            ChangeWeaponAfterPickup = SettingsNS.GameSettings.ChangeWeaponAfterPickup;
        protected override void Interact()
        {
            weaponManager.ChangeWeaponLockStatus(WeaponNameToPickup, true);
            if (ChangeWeaponAfterPickup)
                weaponManager.ChangeWeaponSelection(WeaponNameToPickup);
        }
    }
}