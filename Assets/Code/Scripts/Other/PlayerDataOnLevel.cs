using LightNS;
using PlayerScriptsNS;
using ScriptableObjectNS.Weapon;
using ScriptableObjectNS.Weapon.Gun;
using UnityEngine;
using WeaponManagement;

namespace Data.Player
{
    public class PlayerDataOnLevel : MonoBehaviour
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
        public float LightUpRange = 20;
        public float LightUpSpotAngle = 60;
        private void Awake()
        {
            InitializeWeapon();
            InitializePlayerStats();
            LightInitazlie();
        }
        public void InitializePlayerStats()
        {
            PlayerHealth[] health = GameObject.FindObjectsOfType<PlayerHealth>();
            if (health != null)
            {
                foreach (var h in health)
                {
                    h.Health = Health;
                    h.TimeAfterHitToRegen = TimeAfterHitToRegen;
                    h.InvincibilityTime = InvincibilityTime;
                }
            }
            PlayerInteract[] interacts = GameObject.FindObjectsOfType<PlayerInteract>();
            if (interacts != null)
            {
                foreach (var interacte in interacts)
                    interacte.InteracteDistance = PlayerInteracteDistance;
            }
            PlayerMotor[] playerMotors = GameObject.FindObjectsOfType<PlayerMotor>();
            if (playerMotors != null)
            {
                foreach (var motor in playerMotors)
                {
                    motor.DefaultSpeed = DefaultSpeed;
                    motor.SprintingSpeed = SprintingSpeed;
                }
            }
            PlayerSprintLogic[] playerSprintLogic = GameObject.FindObjectsOfType<PlayerSprintLogic>();
            if (playerSprintLogic != null)
            {
                foreach (var sprint in playerSprintLogic)
                {
                    sprint.SprintingTime = SprintingTime;
                    //sprint.TimeBeforeRestore = TimeBeforeStaminaRestore;
                    //sprint.StaminaRestoreMultiplier = StaminaRestoreMultiplier;
                }
            }
        }
        public void InitializeWeapon()
        {
            ActiveWeaponData.ActiveWeapons = ActiveWeapons;
            foreach (var weaponData in GunObjects)
            {
                GunData gunData = (GunData)weaponData.WeaponData;
                if (gunData != null)
                    gunData.CurrentAmmo = weaponData.CurrentAmmo > gunData.MagSize ? gunData.MagSize : weaponData.CurrentAmmo;
            }
            foreach (var ammoData in AmmoObjects)
                ammoData.ReserveAmmoData.ReserveAmmo = ammoData.ReserveAmmo;
        }
        public void LightInitazlie()
        {
            LightGlowTimer[] lightGlowTimers = GameObject.FindObjectsOfType<LightGlowTimer>();
            if (lightGlowTimers != null)
            {
                foreach (var lightGlowTimer in lightGlowTimers)
                {
                    lightGlowTimer.CurrentTimeLeftToGlow = GlowTime;
                    lightGlowTimer.MaxTimeOfGlowing = MaxGlowTime;
                }
            }
            PlayerLightUp[] playerLightUp = GameObject.FindObjectsOfType<PlayerLightUp>();
            if (playerLightUp != null)
            {
                foreach (var pLU in playerLightUp)
                {
                    pLU.LightUpRange = LightUpRange;
                    pLU.LightUpSpotAngle = LightUpSpotAngle;
                }
            }
        }
    }
}
