using ScriptableObjectNS.Weapon.Gun;
using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace WeaponNS.DataNS
{
    [Serializable]
    public class CurrentAmmoDataObject : IWeaponDataObject
    {
        [SerializeField, FormerlySerializedAs("WeaponData")]
        private GunData weaponData;
        [SerializeField, FormerlySerializedAs("CurrentAmmo")]
        private int currentAmmo;

        public GunData WeaponData { set => weaponData = value; }
        public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }

        public void SetData()
        {
            weaponData.CurrentAmmo = CurrentAmmo > weaponData.MagSize ? weaponData.MagSize : CurrentAmmo;
        }
    }
}