using LightNS;
using PlayerScriptsNS;
using ScriptableObjectNS.Player;
using ScriptableObjectNS.Weapon;
using ScriptableObjectNS.Weapon.Gun;
using UnityEditor;
using UnityEngine;
using WeaponManagement;

namespace Data.Player
{
    public class PlayerDataOnLevel : MonoBehaviour
    {
        public PlayerData PlayerData;
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
        private void Awake()
        {
            GetDataFromScriptableObject();
            InitializeWeapon();
            InitializePlayerStats();
            LightInitazlie();
        }
        public void GetDataFromScriptableObject()
        {
            if (PlayerData)
            {
                PlayerData copiedPlayerData = PlayerData.GetCoppiedValues();
                //Player
                Health = copiedPlayerData.Health;
                TimeAfterHitToRegen = copiedPlayerData.TimeAfterHitToRegen;
                InvincibilityTime = copiedPlayerData.InvincibilityTime;
                PlayerInteracteDistance = copiedPlayerData.PlayerInteracteDistance;
                DefaultSpeed = copiedPlayerData.DefaultSpeed;
                SprintingSpeed = copiedPlayerData.SprintingSpeed;
                SprintingTime = copiedPlayerData.SprintingTime;
                //Weapon
                ActiveWeapons = copiedPlayerData.ActiveWeapons;
                ActiveWeaponData = copiedPlayerData.ActiveWeaponData;
                GunObjects = copiedPlayerData.GunObjects;
                AmmoObjects = copiedPlayerData.AmmoObjects;
                //Light
                GlowTime = copiedPlayerData.GlowTime;
                MaxGlowTime = copiedPlayerData.MaxGlowTime;
                LightIntensity = copiedPlayerData.LightIntensity;
                LightRange = copiedPlayerData.LightRange;
                LightUpRange = copiedPlayerData.LightUpRange;
                LightUpSpotAngle = copiedPlayerData.LightUpSpotAngle;
            }
        }
        public void InitializePlayerStats()
        {
            PlayerHealth[] health = GameObject.FindObjectsOfType<PlayerHealth>();
            foreach (var h in health)
            {
                h.Health = Health;
                h.TimeAfterHitToRegen = TimeAfterHitToRegen;
                h.InvincibilityTime = InvincibilityTime;
            }
            PlayerInteract[] interacts = GameObject.FindObjectsOfType<PlayerInteract>();
            foreach (var interacte in interacts)
                interacte.InteracteDistance = PlayerInteracteDistance;
            PlayerMotor[] playerMotors = GameObject.FindObjectsOfType<PlayerMotor>();
            foreach (var motor in playerMotors)
            {
                motor.DefaultSpeed = DefaultSpeed;
                motor.SprintingSpeed = SprintingSpeed;
            }
            PlayerSprintLogic[] playerSprintLogic = GameObject.FindObjectsOfType<PlayerSprintLogic>();
            foreach (var sprint in playerSprintLogic)
            {
                sprint.SprintingTime = SprintingTime;
                //sprint.TimeBeforeRestore = TimeBeforeStaminaRestore;
                //sprint.StaminaRestoreMultiplier = StaminaRestoreMultiplier;
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

            foreach (var lightGlowTimer in lightGlowTimers)
            {
                lightGlowTimer.CurrentTimeLeftToGlow = GlowTime;
                lightGlowTimer.MaxTimeOfGlowing = MaxGlowTime;
            }
            PlayerMainLampLight[] playerMainLampLights = GameObject.FindObjectsOfType<PlayerMainLampLight>();
            foreach (var playerMainLampLight in playerMainLampLights)
            {
                playerMainLampLight.SpotLight.range = LightRange;
                playerMainLampLight.SpotLight.intensity = LightIntensity;
            }
            PlayerLightUp[] playerLightUp = GameObject.FindObjectsOfType<PlayerLightUp>();
            foreach (var pLU in playerLightUp)
            {
                pLU.LightUpRange = LightUpRange;
                pLU.LightUpSpotAngle = LightUpSpotAngle;
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerDataOnLevel))]
    public class PlayerDataOnLevel_CustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector(); // for other non-HideInInspector fields
            PlayerDataOnLevel script = (PlayerDataOnLevel)target;
            SerializedProperty property;
            property = serializedObject.FindProperty("PlayerData");
            EditorGUILayout.PropertyField(property, new GUIContent("PlayerData"), true);
            if (!script.PlayerData)
            {
                //PlayerStats
                property = serializedObject.FindProperty("Health");
                EditorGUILayout.PropertyField(property, new GUIContent("Health"), true);
                property = serializedObject.FindProperty("TimeAfterHitToRegen");
                EditorGUILayout.PropertyField(property, new GUIContent("TimeAfterHitToRegen"), true);
                property = serializedObject.FindProperty("InvincibilityTime");
                EditorGUILayout.PropertyField(property, new GUIContent("InvincibilityTime"), true);
                property = serializedObject.FindProperty("PlayerInteracteDistance");
                EditorGUILayout.PropertyField(property, new GUIContent("PlayerInteracteDistance"), true);
                property = serializedObject.FindProperty("DefaultSpeed");
                EditorGUILayout.PropertyField(property, new GUIContent("DefaultSpeed"), true);
                property = serializedObject.FindProperty("SprintingSpeed");
                EditorGUILayout.PropertyField(property, new GUIContent("SprintingSpeed"), true);
                property = serializedObject.FindProperty("SprintingTime");
                EditorGUILayout.PropertyField(property, new GUIContent("SprintingTime"), true);
                //WeaponData
                property = serializedObject.FindProperty("ActiveWeapons");
                EditorGUILayout.PropertyField(property, new GUIContent("ActiveWeapons"), true);
                property = serializedObject.FindProperty("ActiveWeaponData");
                EditorGUILayout.PropertyField(property, new GUIContent("ActiveWeaponData"), true);
                property = serializedObject.FindProperty("GunObjects");
                EditorGUILayout.PropertyField(property, new GUIContent("GunObjects"), true);
                property = serializedObject.FindProperty("AmmoObjects");
                EditorGUILayout.PropertyField(property, new GUIContent("AmmoObjects"), true);
                //LightData
                property = serializedObject.FindProperty("GlowTime");
                EditorGUILayout.PropertyField(property, new GUIContent("GlowTime"), true);
                property = serializedObject.FindProperty("MaxGlowTime");
                EditorGUILayout.PropertyField(property, new GUIContent("MaxGlowTime"), true);
                property = serializedObject.FindProperty("LightIntensity");
                EditorGUILayout.PropertyField(property, new GUIContent("LightIntensity"), true);
                property = serializedObject.FindProperty("LightRange");
                EditorGUILayout.PropertyField(property, new GUIContent("LightRange"), true);
                property = serializedObject.FindProperty("LightUpRange");
                EditorGUILayout.PropertyField(property, new GUIContent("LightUpRange"), true);
                property = serializedObject.FindProperty("LightUpSpotAngle");
                EditorGUILayout.PropertyField(property, new GUIContent("LightUpSpotAngle"), true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
