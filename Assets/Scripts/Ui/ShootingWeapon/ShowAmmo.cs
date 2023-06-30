using HudNS;
using MyConstants;
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
            weaponManager.OnWeaponChange += ShowAmmoLeft;
            //PlayerShoot.reloadInput += ShowAmmoLeft;
            ShowAmmoLeft();
        }
        private void OnDisable()
        {
            weaponManager.OnWeaponChange -= ShowAmmoLeft;
            //PlayerShoot.reloadInput -= ShowAmmoLeft;
        }
        public void ShowAmmoLeft()
        {
            if (weaponManager.currentSelection != -1 && weaponManager.Weapons[weaponManager.currentSelection].WeaponGameObject.TryGetComponent(out ShootingWeapon shootingWeapon))
            {
                // int ammo = shootingWeapon.gunData.currentAmmo + shootingWeapon.gunData.reserveAmmo;
                string currentAmmo = shootingWeapon.gunData.currentAmmo >= 10 ? shootingWeapon.gunData.currentAmmo.ToString() : "0" + shootingWeapon.gunData.currentAmmo.ToString();
                string reserveAmmo = shootingWeapon.gunData.reserveAmmo >= 10 ? shootingWeapon.gunData.reserveAmmo.ToString() : "0" + shootingWeapon.gunData.reserveAmmo.ToString();
                messagePrint.PrintMessage($"{currentAmmo} | {reserveAmmo}", 0.5f, HUDConstants.AMMO_LEFT);
            }
        }
    }
}
