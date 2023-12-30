using ScriptableObjectNS.Weapon;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using WeaponManagement;

namespace InteractableNS.Pickups
{
    public class MeleeWeaponPickups : WeaponPickups
    {
        [HideInInspector]
        public bool RandomDurability = false;
        [HideInInspector]
        public int Durability = 100;
        [HideInInspector]
        public int MinDurability = 20;
        [HideInInspector]
        public int MaxDurability = 100;
        [HideInInspector]
        public UnityEvent IfWeaponNotUnlocked;
        //public UnityEvent IfWeaponAlreadyUnlocked;
        [HideInInspector]
        public UnityEvent IfWeaponTypeIsOccupiedByOther;
        private bool isChanged = false;
        private int curentDurability;
        protected override void Start()
        {
            base.Start();
            ActionIfWeaponTypeIsNotOccupied += OnNotUnlocked;
            //ActionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlocked;
            ActionIfWeaponTypeIsOccupiedByOther += OnOccupiedByOther;
        }
        protected override void Interact()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
            if (weaponManager != null)
            {
                Weapon thisWeapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponEntity == WeaponEntity);
                Weapon currentActiveWeapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponType == thisWeapon.WeaponData.WeaponType && weaponManager.IsActiveWeapon(x.WeaponData));
                MeleeData meleeData = thisWeapon.WeaponData as MeleeData;
                curentDurability = isChanged ? curentDurability : RandomDurability ? UnityEngine.Random.Range(MinDurability, Math.Clamp(MaxDurability, 0, meleeData.MaxDurability)) : Durability;
                int durBuffer = meleeData.CurrentDurability;
                meleeData.CurrentDurability = curentDurability;
                curentDurability = durBuffer;
                if (currentActiveWeapon == null)
                {
                    weaponManager.ChangeActiveWeapon(thisWeapon.WeaponData.WeaponType, thisWeapon.WeaponData);
                    ActionIfWeaponTypeIsNotOccupied?.Invoke();
                }
                else
                {
                    Prop.PropBody.SetActive(false);
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
            IfWeaponNotUnlocked?.Invoke();
        //public void OnAlreadyUnlocked() =>
        //    IfWeaponAlreadyUnlocked?.Invoke();
        public void OnOccupiedByOther() =>
            IfWeaponTypeIsOccupiedByOther?.Invoke();
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
            script.RandomDurability = EditorGUILayout.Toggle("Random Durability", script.RandomDurability);
            if (script.RandomDurability) // if bool is true, show other fields
            {
                property = serializedObject.FindProperty("MinDurability");
                EditorGUILayout.PropertyField(property, new GUIContent("MinDurability"), true);
                script.MinDurability = script.MinDurability > script.MaxDurability ? script.MaxDurability : script.MinDurability;
                property = serializedObject.FindProperty("MaxDurability");
                EditorGUILayout.PropertyField(property, new GUIContent("MaxDurability"), true);
            }
            else
            {
                property = serializedObject.FindProperty("Durability");
                EditorGUILayout.PropertyField(property, new GUIContent("Durability"), true);
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            property = serializedObject.FindProperty("IfWeaponNotUnlocked");
            EditorGUILayout.PropertyField(property, new GUIContent("IfWeaponNotUnlocked"), true);
            property = serializedObject.FindProperty("IfWeaponTypeIsOccupiedByOther");
            EditorGUILayout.PropertyField(property, new GUIContent("IfWeaponTypeIsOccupiedByOther"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}