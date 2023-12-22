using UnityEngine;

namespace ScriptableObjectNS.Weapon
{
    [CreateAssetMenu(fileName = "MeleeData", menuName = "ScriptableObject/Weapon/MeleeData")]
    public class MeleeData : WeaponData
    {
        [Header("Melee Params")]
        public float AttackDistance = 3f;
        public float AttackRadius= 0.25f;
        public float AttackTime = 1f;
        public float AnimTransitionTime = 0.3f;
    }
}