using UnityEngine;

namespace WeaponNS
{
    public class WeaponData : ScriptableObject
    {
        [Header("Info")]
        public string Name;
        public WeaponType WeaponType;
        public bool IsUnlocked;
        public bool IsTwoHanded;
        [Header("DamageInfliction")]
        public int Damage;
        public float Force = 1f;
    }
}
