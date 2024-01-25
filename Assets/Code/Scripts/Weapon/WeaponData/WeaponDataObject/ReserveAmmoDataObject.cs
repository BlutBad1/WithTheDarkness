using ScriptableObjectNS.Weapon.Gun.ReserveAmmo;
using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace WeaponNS.DataNS
{
    [Serializable]
    public class ReserveAmmoDataObject : IWeaponDataObject
    {
        [SerializeField, FormerlySerializedAs("ReserveAmmoData")]
        private ReserveAmmoData reserveAmmoData;
        [SerializeField, FormerlySerializedAs("ReserveAmmo")]
        private int reserveAmmo;

        public ReserveAmmoData ReserveAmmoData { set => reserveAmmoData = value; }
        public int ReserveAmmo { get => reserveAmmo; set => reserveAmmo = value; }

        public void SetData()
        {
            reserveAmmoData.ReserveAmmo = ReserveAmmo;
        }
    }
}