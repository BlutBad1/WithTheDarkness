using HudNS;
using MyConstants;
using ScriptableObjectNS.Weapon.Gun;
using System;
using UnityEngine;
using UtilitiesNS;
using WeaponManagement;
using WeaponNS.ShootingWeaponNS;

namespace InteractableNS.Pickups
{
    public class ShootingWeaponPickups : WeaponPickups
    {
        [MinMaxSlider(0, 1000)]
        public Vector2 MinMaxAmountOfBulletToAdd;
        [SerializeField]
        private float disapperingSpeed = 0.8f;
        protected override void Start()
        {
            base.Start();
            ActionIfWeaponUnlocked += OnAlreadyUnlocked;
        }
        protected void OnAlreadyUnlocked()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
            GunData gunData = (GunData)Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponType == WeaponType).WeaponData;
            if (gunData)
            {
                int amountOfBulletsToAdd = (int)UnityEngine.Random.Range(MinMaxAmountOfBulletToAdd.x, MinMaxAmountOfBulletToAdd.y);
                gunData.ReserveAmmo += amountOfBulletsToAdd;
                MessagePrint messagePrint = Utilities.GetComponentFromGameObject<MessagePrint>(LastWhoInteracted.gameObject);
                if (messagePrint)
                    messagePrint.PrintMessage(HUDConstants.
                        AMMO_GET_MESSAGES[UnityEngine.Random.Range(0, HUDConstants.AMMO_GET_MESSAGES.Length)] + $"{amountOfBulletsToAdd} bullets", disapperingSpeed);
                ShowAmmo showAmmo = Utilities.GetComponentFromGameObject<ShowAmmo>(LastWhoInteracted.gameObject);
                if (showAmmo)
                    showAmmo.ShowAmmoLeftOfWeapon(WeaponType);
            }
        }
    }
}