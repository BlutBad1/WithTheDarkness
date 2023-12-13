using ScriptableObjectNS.Weapon;
using ScriptableObjectNS.Weapon.Gun;
using System;
using UnityEngine;
using WeaponNS;
using WeaponNS.ShootingWeaponNS;

namespace WeaponManagement
{
    [Serializable]
    public class WeaponObject
    {
        public WeaponData WeaponData;
        public bool IsUnlocked;
    }
    [Serializable]
    public class GunObject : WeaponObject
    {
        public int CurrentAmmo;
        public int ReserveAmmo;
    }
    public class WeaponInitiallyStats : MonoBehaviour
    {
        [Header("WeaponData")]
        public WeaponObject[] WeaponObjects;

        [Header("GunData")]
        public GunObject[] GunObjects;
        private void Start()
        {
            foreach (var weaponData in WeaponObjects)
                weaponData.WeaponData.IsUnlocked = weaponData.IsUnlocked;
            foreach (var weaponData in GunObjects)
            {
                GunData gunData = (GunData)weaponData.WeaponData;
                if (gunData != null)
                {
                    gunData.IsUnlocked = weaponData.IsUnlocked;
                    gunData.CurrentAmmo = weaponData.CurrentAmmo > gunData.MagSize ? gunData.MagSize : weaponData.CurrentAmmo;
                    gunData.ReserveAmmo = weaponData.ReserveAmmo;
                }
            }
        }
    }
}
