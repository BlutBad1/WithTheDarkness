using ScriptableObjectNS.Weapon;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using WeaponManagement;

namespace InteractableNS.Pickups
{
    public class MeleeWeaponPickups : WeaponPickups
    {
        [SerializeField, HideInInspector, FormerlySerializedAs("RandomDurability")]
        private bool randomDurability = false;
        [SerializeField, HideInInspector, FormerlySerializedAs("Durability")]
        private int durability = 100;
        [SerializeField, HideInInspector, MinMaxSlider(0, 100)]
        private Vector2 minMaxDurability;
        [SerializeField, HideInInspector, FormerlySerializedAs("IfWeaponNotUnlocked")]
        private UnityEvent ifWeaponNotUnlocked;
        //public UnityEvent IfWeaponAlreadyUnlocked;
        [SerializeField, HideInInspector, FormerlySerializedAs("IfWeaponTypeIsOccupiedByOther")]
        private UnityEvent ifWeaponTypeIsOccupiedByOther;

        private bool isChanged = false;
        private float curentDurability;

        public bool RandomDurability { get => randomDurability; set => randomDurability = value; }

        protected override void Start()
        {
            base.Start();
            actionIfWeaponTypeIsNotOccupied += OnNotUnlocked;
            //ActionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlocked;
            actionIfWeaponTypeIsOccupiedByOther += OnOccupiedByOther;
        }
        protected override void Interact()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
            if (weaponManager != null)
            {
                Weapon thisWeapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponEntity == weaponEntity);
                Weapon currentActiveWeapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponType == thisWeapon.WeaponData.WeaponType && weaponManager.IsActiveWeapon(x.WeaponData));
                MeleeData meleeData = thisWeapon.WeaponData as MeleeData;
                curentDurability = isChanged ? curentDurability : randomDurability ? UnityEngine.Random.Range(minMaxDurability.x, Math.Clamp(minMaxDurability.y, 0, meleeData.MaxDurability)) : durability;
                int durBuffer = meleeData.CurrentDurability;
                meleeData.CurrentDurability = (int)curentDurability;
                curentDurability = durBuffer;
                if (currentActiveWeapon == null)
                {
                    weaponManager.ChangeActiveWeapon(thisWeapon.WeaponData.WeaponType, thisWeapon.WeaponData);
                    actionIfWeaponTypeIsNotOccupied?.Invoke();
                }
                else
                {
                    prop.PropBody.SetActive(false);
                    Action methodToExecute = null;
                    methodToExecute = () =>
                    {

                        MeleeWeaponPickups meleeWeaponPickups = UtilitiesNS.Utilities.GetComponentFromGameObject<MeleeWeaponPickups>(SwitchPrefabs(currentActiveWeapon.WeaponData.WeaponPrefab));
                        meleeWeaponPickups.isChanged = true;
                        meleeWeaponPickups.curentDurability = curentDurability;
                        // Unsubscribe after execution
                        UnsubscribeFromEvent(methodToExecute, weaponManager);
                    };
                    weaponManager.OnWeaponChange += methodToExecute;
                    weaponManager.ChangeActiveWeapon(thisWeapon.WeaponData.WeaponType, thisWeapon.WeaponData);
                }
                isChanged = true;
            }
        }
        public void OnNotUnlocked() =>
            ifWeaponNotUnlocked?.Invoke();
        //public void OnAlreadyUnlocked() =>
        //    IfWeaponAlreadyUnlocked?.Invoke();
        public void OnOccupiedByOther() =>
            ifWeaponTypeIsOccupiedByOther?.Invoke();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MeleeWeaponPickups))]
    public class MeleeWeaponPickups_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            MeleeWeaponPickups script = (MeleeWeaponPickups)target;
            SerializedProperty property;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Durability", EditorStyles.boldLabel);
            // draw checkbox for the bool
            property = serializedObject.FindProperty("randomDurability");
            EditorGUILayout.PropertyField(property, new GUIContent("RandomDurability"), true);
            if (script.RandomDurability) // if bool is true, show other fields
            {
                property = serializedObject.FindProperty("minMaxDurability");
                EditorGUILayout.PropertyField(property, new GUIContent("Min Max Durability"), true);
            }
            else
            {
                property = serializedObject.FindProperty("durability");
                EditorGUILayout.PropertyField(property, new GUIContent("Durability"), true);
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            property = serializedObject.FindProperty("ifWeaponNotUnlocked");
            EditorGUILayout.PropertyField(property, new GUIContent("IfWeaponNotUnlocked"), true);
            property = serializedObject.FindProperty("ifWeaponTypeIsOccupiedByOther");
            EditorGUILayout.PropertyField(property, new GUIContent("IfWeaponTypeIsOccupiedByOther"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}