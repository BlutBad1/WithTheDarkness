using HudNS;
using MyConstants;
using ScriptableObjectNS.Weapon.Gun;
using System;
using UnityEngine;
using UnityEngine.Events;
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
        public UnityEvent IfWeaponNotUnlocked;
        public UnityEvent IfWeaponAlreadyUnlocked;
        public UnityEvent IfWeaponTypeIsOccupiedByOther;
        protected override void Start()
        {
            base.Start();
            ActionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlocked;
            ActionIfWeaponTypeIsNotOccupied += OnNotUnlocked;
            ActionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlockedUnityEvent;
            ActionIfWeaponTypeIsOccupiedByOther += OnOccupiedByOther;
        }
        public void OnNotUnlocked() =>
            IfWeaponNotUnlocked?.Invoke();
        public void OnAlreadyUnlockedUnityEvent() =>
            IfWeaponAlreadyUnlocked?.Invoke();
        public void OnOccupiedByOther() =>
            IfWeaponTypeIsOccupiedByOther?.Invoke();
        protected void OnAlreadyUnlocked()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
            GunData gunData = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponEntity == WeaponEntity).WeaponData as GunData;
            if (gunData)
            {
                int amountOfBulletsToAdd = (int)UnityEngine.Random.Range(MinMaxAmountOfBulletToAdd.x, MinMaxAmountOfBulletToAdd.y);
                gunData.ReserveAmmoData.ReserveAmmo += amountOfBulletsToAdd;
                MessagePrint messagePrint = Utilities.GetComponentFromGameObject<MessagePrint>(LastWhoInteracted.gameObject);
                if (messagePrint)
                    messagePrint.PrintMessage(HUDConstants.
                        AMMO_GET_MESSAGES[UnityEngine.Random.Range(0, HUDConstants.AMMO_GET_MESSAGES.Length)] + $"{amountOfBulletsToAdd} bullets", disapperingSpeed);
                ShowAmmo showAmmo = Utilities.GetComponentFromGameObject<ShowAmmo>(LastWhoInteracted.gameObject);
                if (showAmmo)
                    showAmmo.ShowAmmoLeftOfWeapon(WeaponEntity);
            }
        }
    }
}