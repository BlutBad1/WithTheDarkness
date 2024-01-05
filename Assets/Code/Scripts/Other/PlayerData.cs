using ScriptableObjectNS.Weapon;
using UnityEngine;
using WeaponManagement;

namespace ScriptableObjectNS.Player
{
    [CreateAssetMenu(fileName = "Player Configuration", menuName = "ScriptableObject/Player/Player Configuration")]
    public class PlayerData : ScriptableObject
    {
        [Header("Player Stats")]
        [Min(0)]
        public float Health = 100;
        public float TimeAfterHitToRegen = 3f;
        public float InvincibilityTime = 1f;
        [Min(0)]
        public float PlayerInteracteDistance = 3f;
        [Min(0)]
        public float DefaultSpeed = 5f;
        [Min(0)]
        public float SprintingSpeed = 5f;
        [Min(0)]
        public float SprintingTime = 5f;
        //[Min(0)]
        //public float TimeBeforeStaminaRestore = 5f;
        //[Min(0)]
        //public float StaminaRestoreMultiplier = 1f;
        [Header("WeaponData")]
        public SerializableActiveWeapon ActiveWeapons;
        public ActiveWeapon ActiveWeaponData;
        public CurrentAmmoObject[] GunObjects;
        public ReserveAmmoObject[] AmmoObjects;
        [Header("Light"), Min(0)]
        public float GlowTime = 100f;
        [Min(0)]
        public float MaxGlowTime = 100f;
        public float LightIntensity = 2.5f;
        public float LightRange = 22;
        public float LightUpRange = 20;
        public float LightUpSpotAngle = 60;
        public PlayerData GetCoppiedValues()
        {
            PlayerData newPlayerData = new PlayerData();
            string serializedPlayerData = JsonUtility.ToJson(this);
            JsonUtility.FromJsonOverwrite(serializedPlayerData, newPlayerData);
            return newPlayerData;
        }
    }
}