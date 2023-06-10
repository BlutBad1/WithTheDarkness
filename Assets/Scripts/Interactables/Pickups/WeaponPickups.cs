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
            ChangeWeaponAfterPickup = SettingsNS.ChangeWeapoAfterPickupSettings.ChangeWeaponAfterPickup;
            SettingsNS.ChangeWeapoAfterPickupSettings.WeaponChangeAfterPickupStatusChangeEvent += CheckChangeWeaponAfterPickupStatus;
        }
        public void CheckChangeWeaponAfterPickupStatus() =>
            ChangeWeaponAfterPickup = SettingsNS.ChangeWeapoAfterPickupSettings.ChangeWeaponAfterPickup;
        protected override void Interact()
        {
            weaponManager.ChangeWeaponLockStatus(WeaponNameToPickup, true);
            if (ChangeWeaponAfterPickup)
                weaponManager.ChangeWeaponSelection(WeaponNameToPickup);
        }
    }
}