using UnityEngine;

namespace ScriptableObjectNS.Weapon
{
    [CreateAssetMenu(fileName = "MeleeData", menuName = "ScriptableObject/Weapon/MeleeData")]
    public class MeleeData : WeaponData
    {
        [Header("Melee Params")]
        public float AttackDistance = 3f;
        public float AttackRadius = 0.25f;
        public float AttackTime = 1f;
        public float AnimTransitionTime = 0.3f;
        [Header("Durability")]
        [HideInInspector, Min(0)]
        public int CurrentDurability = 100;
        [Min(0)]
        public int MaxDurability = 100;
        public int MoveDurabilityCost = 15;
    }
}