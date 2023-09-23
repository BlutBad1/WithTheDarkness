using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponNS;

namespace ScriptableObjectNS.Weapon.Gun
{
    [CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObject/Weapon/GunData")]
    public class GunData : WeaponData
    {
        [Header("Shooting")]
        public float MaxDistance;
        public float FiringTime;
        public float MaxDeviation;
        [Header("Reloading")]
        public int CurrentAmmo;
        public int MagSize;
        public int ReserveAmmo;
        public float FireRate;
        public float ReloadTime;
        [HideInInspector]
        public bool Reloading;
    }
}
