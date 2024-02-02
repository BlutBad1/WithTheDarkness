using HudNS;
using HudNS.Weapon.ShootingWeapon;
using MyConstants;
using ScriptableObjectNS.Weapon.Gun;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UtilitiesNS;
using WeaponManagement;

namespace InteractableNS.Pickups
{
    public class ShootingWeaponPickups : WeaponPickups
    {
        [SerializeField, MinMaxSlider(0, 1000), FormerlySerializedAs("MinMaxAmountOfBulletToAdd")]
        private Vector2 minMaxAmountOfBulletToAdd;
        [SerializeField]
        private float disapperingSpeed = 0.8f;
        [SerializeField, FormerlySerializedAs("IfWeaponNotUnlocked")]
        private UnityEvent ifWeaponNotUnlocked;
        [SerializeField, FormerlySerializedAs("IfWeaponAlreadyUnlocked")]
        private UnityEvent ifWeaponAlreadyUnlocked;
        [SerializeField, FormerlySerializedAs("IfWeaponTypeIsOccupiedByOther")]
        private UnityEvent ifWeaponTypeIsOccupiedByOther;

        protected override void Start()
        {
            base.Start();
            actionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlocked;
            actionIfWeaponTypeIsNotOccupied += OnNotUnlocked;
            actionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlockedUnityEvent;
            actionIfWeaponTypeIsOccupiedByOther += OnOccupiedByOther;
        }
        protected void OnNotUnlocked() =>
            ifWeaponNotUnlocked?.Invoke();
        protected void OnAlreadyUnlockedUnityEvent() =>
            ifWeaponAlreadyUnlocked?.Invoke();
        protected void OnOccupiedByOther() =>
            ifWeaponTypeIsOccupiedByOther?.Invoke();
        protected void OnAlreadyUnlocked()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(lastWhoInteracted.gameObject);
            GunData gunData = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponEntity == weaponEntity).WeaponData as GunData;
            if (gunData)
            {
                int amountOfBulletsToAdd = (int)UnityEngine.Random.Range(minMaxAmountOfBulletToAdd.x, minMaxAmountOfBulletToAdd.y);
                gunData.ReserveAmmoData.ReserveAmmo += amountOfBulletsToAdd;
                MessagePrint messagePrint = Utilities.GetComponentFromGameObject<MessagePrint>(lastWhoInteracted.gameObject);
                if (messagePrint)
                    messagePrint.PrintMessage(HUDConstants.
                        AMMO_GET_MESSAGES[UnityEngine.Random.Range(0, HUDConstants.AMMO_GET_MESSAGES.Length)] + $"{amountOfBulletsToAdd} bullets", disapperingSpeed);
                ShowAmmo showAmmo = Utilities.GetComponentFromGameObject<ShowAmmo>(lastWhoInteracted.gameObject);
                if (showAmmo)
                    showAmmo.ShowAmmoLeftOfWeapon(weaponEntity);
            }
        }
    }
}