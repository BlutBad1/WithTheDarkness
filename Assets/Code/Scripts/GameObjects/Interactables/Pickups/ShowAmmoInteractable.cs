using InteractableNS.Common;
using MyConstants;
using UnityEngine;
using WeaponNS.ShootingWeaponNS;

namespace InteractableNS.Pickups
{
    public class ShowAmmoInteractable : InfoTextWriting
    {
        public string WeaponName;
        ShowAmmo showAmmo;
        protected new void Start()
        {
            base.Start();
            showAmmo = GameObject.Find(CommonConstants.WEAPON_HOLDER).GetComponent<ShowAmmo>();
        }
        protected override void Interact()
        {
            base.Interact();
            showAmmo.ShowAmmoLeftOfWeapon(WeaponName);
        }
    }
}
