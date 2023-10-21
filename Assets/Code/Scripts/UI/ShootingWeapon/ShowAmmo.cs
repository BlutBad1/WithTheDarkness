using HudNS;
using MyConstants;
using System;
using TMPro;
using UnityEngine;
using WeaponManagement;

namespace WeaponNS.ShootingWeaponNS
{
    [RequireComponent(typeof(WeaponManagement.WeaponManager))]
    public class ShowAmmo : MonoBehaviour
    {
        public float DisapperingSpeed = 0.5f;
        [Tooltip("Delay for showing ammo on reloading input.")]
        public float DelayTime = 1f;
        public TextMeshProUGUI Showcaser;
        private float timeSinceLastReloadInput = 0f;
        private WeaponManager weaponManager;
        private MessagePrint messagePrint;
        private void Awake()
        {
            messagePrint = GameObject.FindAnyObjectByType<MessagePrint>();
            weaponManager = GetComponent<WeaponManager>();
            messagePrint.PrintMessage("", 0.5f, Showcaser);
        }
        private void Start()
        {
            weaponManager.OnWeaponChange += ShowCurrentWeaponAmmo;
            PlayerBattleInput.ReloadInputStarted += ShowAmmoOnReloadingInput;
            ShowCurrentWeaponAmmo();
        }
        private void OnDisable()
        {
            weaponManager.OnWeaponChange -= ShowCurrentWeaponAmmo;
            PlayerBattleInput.ReloadInputStarted -= ShowAmmoOnReloadingInput;
        }
        private void Update()
        {
            timeSinceLastReloadInput += Time.deltaTime;
        }
        private void ShowAmmoOnReloadingInput()
        {
            if (timeSinceLastReloadInput >= DelayTime)
            {
                ShowCurrentWeaponAmmo();
                timeSinceLastReloadInput = 0f;
            }
        }
        public void ShowCurrentWeaponAmmo()
        {
            if (weaponManager.currentSelection != -1)
                ShowAmmoLeftOfWeapon(weaponManager.currentSelection);
        }
        public void ShowAmmoLeftOfWeapon(string nameOfWeapon) =>
            ShowAmmoLeftOfWeapon(nameOfWeapon, 0.5f);
        public void ShowAmmoLeftOfWeapon(WeaponType weaponType) =>
            ShowAmmoLeftOfWeapon(weaponType, 0.5f);
        public void ShowAmmoLeftOfWeapon(WeaponType weaponType, float disapperingSpeed) =>
            ShowAmmoLeftOfWeapon(Array.FindIndex(weaponManager.Weapons, x => x.WeaponData.WeaponType == weaponType), disapperingSpeed);
        public void ShowAmmoLeftOfWeapon(string nameOfWeapon, float disapperingSpeed)
        {
            int index = Array.FindIndex(weaponManager.Weapons, x => x.WeaponData.Name == nameOfWeapon);
            if (index != -1)
                ShowAmmoLeftOfWeapon(index, disapperingSpeed);
#if UNITY_EDITOR
            else
                Debug.Log("Weapon is not found!");
#endif
        }
        public void ShowAmmoLeftOfWeapon(int indexOfWeapon) =>
            ShowAmmoLeftOfWeapon(indexOfWeapon, DisapperingSpeed);
        public void ShowAmmoLeftOfWeapon(int indexOfWeapon, float disapperingSpeed)
        {
            if (weaponManager.Weapons[indexOfWeapon].WeaponGameObject.TryGetComponent(out ShootingWeapon shootingWeapon))
            {
                // int ammo = shootingWeapon.gunData.currentAmmo + shootingWeapon.gunData.reserveAmmo;
                string currentAmmo = shootingWeapon.gunData.CurrentAmmo >= 10 ? shootingWeapon.gunData.CurrentAmmo.ToString() : "0" + shootingWeapon.gunData.CurrentAmmo.ToString();
                string reserveAmmo = shootingWeapon.gunData.ReserveAmmo >= 10 ? shootingWeapon.gunData.ReserveAmmo.ToString() : "0" + shootingWeapon.gunData.ReserveAmmo.ToString();
                messagePrint.PrintMessage($"{currentAmmo} | {reserveAmmo}", disapperingSpeed, Showcaser);
            }
        }
    }
}
