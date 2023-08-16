using HudNS;
using MyConstants;
using System;
using UnityEngine;
using WeaponManagement;

namespace WeaponNS.ShootingWeaponNS
{
    [RequireComponent(typeof(WeaponManagement.WeaponManager))]
    public class ShowAmmo : MonoBehaviour
    {
        WeaponManager weaponManager;
        MessagePrint messagePrint;
        private void Awake()
        {
            messagePrint = GameObject.FindAnyObjectByType<MessagePrint>();
            weaponManager = GetComponent<WeaponManager>();
            messagePrint.PrintMessage("", 0.5f, HUDConstants.AMMO_LEFT);
        }
        private void Start()
        {
            weaponManager.OnWeaponChange += ShowCurrentWeaponAmmo;
            //PlayerShoot.reloadInput += ShowAmmoLeft;
            ShowCurrentWeaponAmmo();
        }
        private void OnDisable()
        {
            weaponManager.OnWeaponChange -= ShowCurrentWeaponAmmo;
            //PlayerShoot.reloadInput -= ShowAmmoLeft;
        }
        public void ShowCurrentWeaponAmmo()
        {
            if (weaponManager.currentSelection != -1)
                ShowAmmoLeftOfWeapon(weaponManager.currentSelection);
        }
        public void ShowAmmoLeftOfWeapon(string nameOfWeapon) =>
            ShowAmmoLeftOfWeapon(nameOfWeapon, 0.5f);
        public void ShowAmmoLeftOfWeapon(string nameOfWeapon, float disapperingSpeed)
        {
            int index = Array.FindIndex(weaponManager.Weapons, x => x.Name == nameOfWeapon);
            if (index != -1)
                ShowAmmoLeftOfWeapon(index, disapperingSpeed);
#if UNITY_EDITOR
            else
                Debug.Log("Weapon is not found!");
#endif
        }
        public void ShowAmmoLeftOfWeapon(int indexOfWeapon) =>
            ShowAmmoLeftOfWeapon(indexOfWeapon, 0.5f);
        public void ShowAmmoLeftOfWeapon(int indexOfWeapon, float disapperingSpeed)
        {
            if (weaponManager.Weapons[indexOfWeapon].WeaponGameObject.TryGetComponent(out ShootingWeapon shootingWeapon))
            {
                // int ammo = shootingWeapon.gunData.currentAmmo + shootingWeapon.gunData.reserveAmmo;
                string currentAmmo = shootingWeapon.gunData.currentAmmo >= 10 ? shootingWeapon.gunData.currentAmmo.ToString() : "0" + shootingWeapon.gunData.currentAmmo.ToString();
                string reserveAmmo = shootingWeapon.gunData.reserveAmmo >= 10 ? shootingWeapon.gunData.reserveAmmo.ToString() : "0" + shootingWeapon.gunData.reserveAmmo.ToString();
                messagePrint.PrintMessage($"{currentAmmo} | {reserveAmmo}", disapperingSpeed, HUDConstants.AMMO_LEFT);
            }
        }
    }
}
