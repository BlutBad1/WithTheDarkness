using ScriptableObjectNS.Weapon;
using ScriptableObjectNS.Weapon.Gun;
using ScriptableObjectNS.Weapon.Gun.ReserveAmmo;
using System;
using UnityEngine;

namespace WeaponManagement
{
    [Serializable]
    public class CurrentAmmoObject
    {
        public WeaponData WeaponData;
        public int CurrentAmmo;
    }
    [Serializable]
    public class ReserveAmmoObject
    {
        public ReserveAmmoData ReserveAmmoData;
        public int ReserveAmmo;
    }
    public class WeaponInitiallyStats : MonoBehaviour
    {
        [Header("ActiveWeapon")]
        public SerializableActiveWeapon ActiveWeapons;
        public ActiveWeapon ActiveWeaponData;
        [Header("CurrentAmmoData")]
        public CurrentAmmoObject[] GunObjects;
        [Header("ReserveAmmoData")]
        public ReserveAmmoObject[] AmmoObjects;
        private void Start()
        {
            InitializeWeaponStats();
        }
        public void InitializeWeaponStats()
        {
            ActiveWeaponData.ActiveWeapons = ActiveWeapons;
            foreach (var weaponData in GunObjects)
            {
                GunData gunData = (GunData)weaponData.WeaponData;
                if (gunData != null)
                    gunData.CurrentAmmo = weaponData.CurrentAmmo > gunData.MagSize ? gunData.MagSize : weaponData.CurrentAmmo;
            }
            foreach (var ammoData in AmmoObjects)
                ammoData.ReserveAmmoData.ReserveAmmo = ammoData.ReserveAmmo;
        }
    }
}
