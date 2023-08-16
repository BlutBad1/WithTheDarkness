using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS
{

    [CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
    public class GunData : ScriptableObject
    {
        [Header("Info")]
        public new string name;
        [Header("Shooting")]
        public int damage;
        public float maxDistance;
        public float firingTime;
        public float force = 1f;
        [Header("Reloading")]
        public int currentAmmo;
        public int magSize;
        public int reserveAmmo;
        public float fireRate;
        public float reloadTime;

        [HideInInspector]
        public bool reloading;

    }
}
