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
        public bool UseConstantMessage = true;
        private string originalMessage;
        protected new void Start()
        {
            base.Start();
            originalMessage = message;
        }
        protected override void Interact()
        {
            int amountOfBulletsToAdd = (int)Random.Range(AmountOfBullets.x, AmountOfBullets.y);
            if (UseConstantMessage)
                message = HUDConstants.AMMO_GET_MESSAGES[Random.Range(0, HUDConstants.AMMO_GET_MESSAGES.Length)] + $"{amountOfBulletsToAdd} bullets";
            else
                message = originalMessage;
            base.Interact();
            gun.ReserveAmmoData.ReserveAmmo += amountOfBulletsToAdd;
            ShowAmmo showAmmo = Utilities.GetComponentFromGameObject<ShowAmmo>(LastWhoInteracted.gameObject);
            if (showAmmo)
                showAmmo.ShowAmmoLeftOfWeapon(gun.WeaponEntity);
        }
    }
}
