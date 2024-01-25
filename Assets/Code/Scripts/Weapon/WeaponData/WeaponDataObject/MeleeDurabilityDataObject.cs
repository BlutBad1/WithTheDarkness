using ScriptableObjectNS.Weapon;
using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace WeaponNS.DataNS
{
    [Serializable]
    public class MeleeDurabilityDataObject : IWeaponDataObject
    {
        [SerializeField, FormerlySerializedAs("MeleeData")]
        private MeleeData meleeData;
        [SerializeField, FormerlySerializedAs("CurrentDurability")]
        private int currentDurability;

        public MeleeData MeleeData { set => meleeData = value; }
        public int CurrentDurability { get => currentDurability; set => currentDurability = value; }

        public void SetData()
        {
            meleeData.CurrentDurability = CurrentDurability;
        }
    }
}