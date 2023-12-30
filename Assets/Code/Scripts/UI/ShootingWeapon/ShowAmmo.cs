using HudNS;
using ScriptableObjectNS.Weapon.Gun;
using System.Linq;
using TMPro;
using UnityEngine;
using WeaponManagement;
using WeaponNS;
using WeaponNS.ShootingWeaponNS;

namespace HudNS.Weapon.ShootingWeapon
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
            if (weaponManager.ActiveWeapon.CurrentSelectedActiveWeapon != null)
                ShowAmmoLeftOfWeapon(weaponManager.ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.WeaponEntity);
        }
        public void ShowAmmoLeftOfWeapon(WeaponEntity weaponType) =>
            ShowAmmoLeftOfWeapon(weaponType, DisapperingSpeed);
        public void ShowAmmoLeftOfWeapon(WeaponEntity weaponEntity, float disapperingSpeed)
        {
            GunData weaponData = weaponManager.Weapons.FirstOrDefault(x => x.WeaponData.WeaponEntity == weaponEntity).WeaponData as GunData;
            if (weaponData)
            {
                // int ammo = shootingWeapon.gunData.currentAmmo + shootingWeapon.gunData.reserveAmmo;
                string currentAmmo = weaponData.CurrentAmmo >= 10 ? weaponData.CurrentAmmo.ToString() : "0" + weaponData.CurrentAmmo.ToString();
                string reserveAmmo = weaponData.ReserveAmmoData.ReserveAmmo >= 10 ? weaponData.ReserveAmmoData.ReserveAmmo.ToString() : "0" + weaponData.ReserveAmmoData.ReserveAmmo.ToString();
                messagePrint.PrintMessage($"{currentAmmo} | {reserveAmmo}", disapperingSpeed, Showcaser);
            }
        }
    }
}
