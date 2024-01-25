using ScriptableObjectNS.Weapon;
using UnityEngine;
using WeaponNS.DataNS;

namespace WeaponManagement
{
    public class WeaponInitiallyStats : MonoBehaviour
    {
        [Header("ActiveWeapon")]
        public SerializableActiveWeapon ActiveWeapons;
        public ActiveWeapon ActiveWeaponData;
        [Header("CurrentMeleeDurabilityData")]
        public MeleeDurabilityDataObject[] MeleeObjects;
        [Header("CurrentAmmoData")]
        public CurrentAmmoDataObject[] GunObjects;
        [Header("ReserveAmmoData")]
        public ReserveAmmoDataObject[] AmmoObjects;
        private void Start()
        {
            InitializeWeaponStats();
        }
        public void InitializeWeaponStats()
        {
            ActiveWeaponData.ActiveWeapons = ActiveWeapons;
            foreach (var durabilityData in MeleeObjects)
                durabilityData.SetData();
            foreach (var gunData in GunObjects)
                gunData.SetData();
            foreach (var ammoData in AmmoObjects)
                ammoData.SetData();
        }
    }
}
