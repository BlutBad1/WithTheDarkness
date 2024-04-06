using LightNS;
using PlayerScriptsNS;
using ScriptableObjectNS.Player;
using ScriptableObjectNS.Weapon;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponNS.DataNS;

namespace Data.Player
{
    public class PlayerDataOnLevel : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("PlayerData")]
        private PlayerData playerData;
        [Header("Player Stats"), SerializeField, FormerlySerializedAs("Health"), Min(0)]
        private float health = 100;
        [SerializeField, FormerlySerializedAs("TimeAfterHitToRegen")]
        private float timeAfterHitToRegen = 3f;
        [SerializeField, FormerlySerializedAs("InvincibilityTime")]
        private float invincibilityTime = 1f;
        [SerializeField, FormerlySerializedAs("PlayerInteracteDistance"), Min(0)]
        private float playerInteracteDistance = 3f;
        [SerializeField, FormerlySerializedAs("DefaultSpeed"), Min(0)]
        private float defaultSpeed = 5f;
        [SerializeField, FormerlySerializedAs("SprintingSpeed"), Min(0)]
        private float sprintingSpeed = 5f;
        [SerializeField, FormerlySerializedAs("SprintingTime"), Min(0)]
        private float sprintingTime = 5f;
        [Header("WeaponData"), SerializeField, FormerlySerializedAs("ActiveWeapons")]
        private SerializableActiveWeapon activeWeapons;
        [SerializeField, FormerlySerializedAs("ActiveWeaponData")]
        private ActiveWeapon activeWeaponData;
        [SerializeField, FormerlySerializedAs("MeleeDurabilityData")]
        private MeleeDurabilityDataObject[] meleeDurabilityData;
        [SerializeField, FormerlySerializedAs("CurrentAmmoData")]//
        private CurrentAmmoDataObject[] currentAmmoData;
        [SerializeField, FormerlySerializedAs("ReserveAmmoData")]//
        private ReserveAmmoDataObject[] reserveAmmoData;
        [Header("Light"), SerializeField, FormerlySerializedAs("GlowTime"), Min(0)]
        private float glowTime = 100f;
        [Header("Light"), SerializeField, FormerlySerializedAs("MaxGlowTime"), Min(0)]
        private float maxGlowTime = 100f;
        [SerializeField, FormerlySerializedAs("LightIntensity")]
        private float lightIntensity = 2.5f;
        [SerializeField, FormerlySerializedAs("LightRange")]
        private float lightRange = 22;
        [SerializeField, FormerlySerializedAs("LightUpRange")]
        private float lightUpRange = 20;
        [SerializeField, FormerlySerializedAs("LightUpSpotAngle")]
        private float lightUpSpotAngle = 60;

        public PlayerData PlayerData { get => playerData; set => playerData = value; }

        private void Awake()
        {
            GetDataFromScriptableObject();
            InitializeWeapon();
            InitializePlayerStats();
            LightInitazlie();
        }
        private void GetDataFromScriptableObject()
        {
            if (playerData)
            {
                PlayerData copiedPlayerData = playerData.GetCoppiedValues();
                //Player
                health = copiedPlayerData.Health;
                timeAfterHitToRegen = copiedPlayerData.TimeAfterHitToRegen;
                invincibilityTime = copiedPlayerData.InvincibilityTime;
                playerInteracteDistance = copiedPlayerData.PlayerInteracteDistance;
                defaultSpeed = copiedPlayerData.DefaultSpeed;
                sprintingSpeed = copiedPlayerData.SprintingSpeed;
                sprintingTime = copiedPlayerData.SprintingTime;
                //Weapon
                activeWeapons = copiedPlayerData.ActiveWeapons;
                activeWeaponData = copiedPlayerData.ActiveWeaponData;
                meleeDurabilityData = copiedPlayerData.MeleeDurabilityData;
                currentAmmoData = copiedPlayerData.CurrentAmmoData;
                reserveAmmoData = copiedPlayerData.ReserveAmmoData;
                //Light
                glowTime = copiedPlayerData.GlowTime;
                maxGlowTime = copiedPlayerData.MaxGlowTime;
                lightIntensity = copiedPlayerData.LightIntensity;
                lightRange = copiedPlayerData.LightRange;
                lightUpRange = copiedPlayerData.LightUpRange;
                lightUpSpotAngle = copiedPlayerData.LightUpSpotAngle;
            }
        }
        private void InitializePlayerStats()
        {
            ExtendedPlayerHealth[] health = GameObject.FindObjectsOfType<ExtendedPlayerHealth>();
            foreach (var h in health)
            {
                h.Health = this.health;
                h.TimeAfterHitToRegen = timeAfterHitToRegen;
                h.InvincibilityTime = invincibilityTime;
            }
            PlayerInteract[] interacts = GameObject.FindObjectsOfType<PlayerInteract>();
            foreach (var interacte in interacts)
                interacte.InteracteDistance = playerInteracteDistance;
            PlayerMotor[] playerMotors = GameObject.FindObjectsOfType<PlayerMotor>();
            foreach (var motor in playerMotors)
            {
                motor.DefaultSpeed = defaultSpeed;
                motor.SprintingSpeed = sprintingSpeed;
            }
            PlayerSprintLogic[] playerSprintLogic = GameObject.FindObjectsOfType<PlayerSprintLogic>();
            foreach (var sprint in playerSprintLogic)
                sprint.SprintingTime = sprintingTime;
        }
        private void InitializeWeapon()
        {
            activeWeaponData.ActiveWeapons = activeWeapons;
            foreach (var durabilityData in meleeDurabilityData)
                durabilityData.SetData();
            foreach (var weaponData in currentAmmoData)
                weaponData.SetData();
            foreach (var ammoData in reserveAmmoData)
                ammoData.SetData();
        }
        private void LightInitazlie()
        {
            LightGlowTimer[] lightGlowTimers = GameObject.FindObjectsOfType<LightGlowTimer>();

            foreach (var lightGlowTimer in lightGlowTimers)
            {
                lightGlowTimer.CurrentTimeLeftToGlow = glowTime;
                lightGlowTimer.MaxTimeOfGlowing = maxGlowTime;
            }
            PlayerMainLampLight[] playerMainLampLights = GameObject.FindObjectsOfType<PlayerMainLampLight>();
            foreach (var playerMainLampLight in playerMainLampLights)
            {
                playerMainLampLight.SpotLight.range = lightRange;
                playerMainLampLight.SpotLight.intensity = lightIntensity;
            }
            PlayerLightUp[] playerLightUp = GameObject.FindObjectsOfType<PlayerLightUp>();
            foreach (var pLU in playerLightUp)
            {
                pLU.LightUpRange = lightUpRange;
                pLU.LightUpSpotAngle = lightUpSpotAngle;
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
            property = serializedObject.FindProperty("playerData");
            EditorGUILayout.PropertyField(property, new GUIContent("PlayerData"), true);
            if (!script.PlayerData)
            {
                //PlayerStats
                property = serializedObject.FindProperty("health");
                EditorGUILayout.PropertyField(property, new GUIContent("Health"), true);
                property = serializedObject.FindProperty("timeAfterHitToRegen");
                EditorGUILayout.PropertyField(property, new GUIContent("TimeAfterHitToRegen"), true);
                property = serializedObject.FindProperty("invincibilityTime");
                EditorGUILayout.PropertyField(property, new GUIContent("InvincibilityTime"), true);
                property = serializedObject.FindProperty("playerInteracteDistance");
                EditorGUILayout.PropertyField(property, new GUIContent("PlayerInteracteDistance"), true);
                property = serializedObject.FindProperty("defaultSpeed");
                EditorGUILayout.PropertyField(property, new GUIContent("DefaultSpeed"), true);
                property = serializedObject.FindProperty("sprintingSpeed");
                EditorGUILayout.PropertyField(property, new GUIContent("SprintingSpeed"), true);
                property = serializedObject.FindProperty("sprintingTime");
                EditorGUILayout.PropertyField(property, new GUIContent("SprintingTime"), true);
                //WeaponData
                property = serializedObject.FindProperty("activeWeapons");
                EditorGUILayout.PropertyField(property, new GUIContent("ActiveWeapons"), true);
                property = serializedObject.FindProperty("activeWeaponData");
                EditorGUILayout.PropertyField(property, new GUIContent("ActiveWeaponData"), true);
                property = serializedObject.FindProperty("meleeDurabilityData");
                EditorGUILayout.PropertyField(property, new GUIContent("MeleeDurabilityData"), true);
                property = serializedObject.FindProperty("currentAmmoData");
                EditorGUILayout.PropertyField(property, new GUIContent("CurrentAmmoData"), true);
                property = serializedObject.FindProperty("reserveAmmoData");
                EditorGUILayout.PropertyField(property, new GUIContent("ReserveAmmoData"), true);
                //LightData
                property = serializedObject.FindProperty("glowTime");
                EditorGUILayout.PropertyField(property, new GUIContent("GlowTime"), true);
                property = serializedObject.FindProperty("maxGlowTime");
                EditorGUILayout.PropertyField(property, new GUIContent("MaxGlowTime"), true);
                property = serializedObject.FindProperty("lightIntensity");
                EditorGUILayout.PropertyField(property, new GUIContent("LightIntensity"), true);
                property = serializedObject.FindProperty("lightRange");
                EditorGUILayout.PropertyField(property, new GUIContent("LightRange"), true);
                property = serializedObject.FindProperty("lightUpRange");
                EditorGUILayout.PropertyField(property, new GUIContent("LightUpRange"), true);
                property = serializedObject.FindProperty("lightUpSpotAngle");
                EditorGUILayout.PropertyField(property, new GUIContent("LightUpSpotAngle"), true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
