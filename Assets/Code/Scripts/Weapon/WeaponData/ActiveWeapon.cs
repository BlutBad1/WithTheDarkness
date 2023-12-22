using UnityEditor;
using UnityEngine;
using WeaponNS;

namespace ScriptableObjectNS.Weapon
{
    [System.Serializable]
    public class SerializableActiveWeapon
    {
        public WeaponData ActiveMeleeWeapon;
        public WeaponData ActiveOneHandedGun;
        public WeaponData ActiveTwoHandedGun;
    }
    [CreateAssetMenu(fileName = "ActiveWeaponData", menuName = "ScriptableObject/Weapon/ActiveWeaponData")]
    public class ActiveWeapon : ScriptableObject
    {
        [HideInInspector]
        public WeaponManagement.Weapon CurrentSelectedActiveWeapon;
        [Header("Active Weapon")]
        public SerializableActiveWeapon ActiveWeapons;
    }
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(SerializableActiveWeapon))]
    public class SerializableActiveWeaponDrawer : PropertyDrawer
    {
        private int fieldAmount = 3;
        private float fieldSize = 28;
        private float padding = 4.2f;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty activeMeleeWeapon = property.FindPropertyRelative("ActiveMeleeWeapon");
            SerializedProperty activeOneHandedGun = property.FindPropertyRelative("ActiveOneHandedGun");
            SerializedProperty activeTwoHandedGun = property.FindPropertyRelative("ActiveTwoHandedGun");
            DrawWeaponTypeSection(position, activeMeleeWeapon, WeaponType.Melee);
            position.y += EditorGUIUtility.singleLineHeight + padding;
            DrawWeaponTypeSection(position, activeOneHandedGun, WeaponType.OneHandedGun);
            position.y += EditorGUIUtility.singleLineHeight + padding;
            DrawWeaponTypeSection(position, activeTwoHandedGun, WeaponType.TwoHandedGun);
            EditorGUI.EndProperty();
        }
        private void DrawWeaponTypeSection(Rect position, SerializedProperty weaponProperty, WeaponType weaponType)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight + 2f), weaponProperty);
            WeaponType propertyWeaponType = GetWeaponType(weaponProperty);
            if (propertyWeaponType != weaponType && weaponProperty.objectReferenceValue != null)
            {
                weaponProperty.objectReferenceValue = null; // Set to null to clear the assigned object
                Debug.LogError($"Must be of {weaponType.ToString()} type.");
            }
        }
        private WeaponType GetWeaponType(SerializedProperty weaponProperty)
        {
            WeaponData weaponData = weaponProperty.objectReferenceValue as WeaponData;
            return weaponData != null ? weaponData.WeaponType : WeaponType.Melee;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            (EditorGUIUtility.singleLineHeight * fieldAmount) + fieldSize - 7;
    }
#endif
}