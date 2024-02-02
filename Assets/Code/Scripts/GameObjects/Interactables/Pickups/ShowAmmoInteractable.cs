using HudNS.Weapon.ShootingWeapon;
using InteractableNS.Common;
using MyConstants;
using ScriptableObjectNS.Weapon.Gun;
using UnityEngine;
using UtilitiesNS;

namespace InteractableNS.Pickups
{
    public class ShowAmmoInteractable : InfoTextWriting
    {
        public GunData gun;
        [MinMaxSlider(0, 100)]
        public Vector2 AmountOfBullets;
        protected override void Interact()
        {
            int amountOfBulletsToAdd = (int)Random.Range(AmountOfBullets.x, AmountOfBullets.y);
            message = HUDConstants.AMMO_GET_MESSAGES[Random.Range(0, HUDConstants.AMMO_GET_MESSAGES.Length)] + $"{amountOfBulletsToAdd} bullets";
            base.Interact();
            gun.ReserveAmmoData.ReserveAmmo += amountOfBulletsToAdd;
            ShowAmmo showAmmo = Utilities.GetComponentFromGameObject<ShowAmmo>(lastWhoInteracted.gameObject);
            if (showAmmo)
                showAmmo.ShowAmmoLeftOfWeapon(gun.WeaponEntity);
        }
    }
}
