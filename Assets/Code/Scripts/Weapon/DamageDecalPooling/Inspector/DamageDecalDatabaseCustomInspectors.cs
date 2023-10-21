using PoolableObjectsNS;
using System;
using UnityEditor;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS.Inspector
{
    public class DamageDecalDatabaseCustomInspectors
    {
    }
    class DamageDecalObjectPopupField : PropertyAttribute
    {
    }
    class DamageDecalObjectTagPopupField : PropertyAttribute
    {
    }
#if UNITY_EDITOR
    // Custom property inspector of type 'DamageDecalObjectPopupField'
    [CustomPropertyDrawer(typeof(DamageDecalObjectPopupField))]
    public class DamageDecalObjectPopupFieldDrawer : PropertyDrawer
    {
        DamageDecalPoolsManager damageDecalHolesPool = null;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!damageDecalHolesPool)
            {
                damageDecalHolesPool = GameObject.FindObjectOfType<DamageDecalPoolsManager>();
                return;
            }
            int selectedIndex = 0;
            if (property.stringValue != null || property.stringValue != "")
                selectedIndex = Array.FindIndex(damageDecalHolesPool.pools, x => x.Name == property.stringValue);
            selectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            position.y += 1f;
            selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex, damageDecalHolesPool.GetAllPoolNames());
            property.stringValue = damageDecalHolesPool.pools[selectedIndex].Name;
        }
    }
    // Custom property inspector of type ' DamageDecalObjectTagPopupField'
    [CustomPropertyDrawer(typeof(DamageDecalObjectTagPopupField))]
    public class DamageDecalObjectTagPopupFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int selectedIndex = 0;
            if (property.stringValue != null || property.stringValue != "")
                selectedIndex = Array.FindIndex(UnityEditorInternal.InternalEditorUtility.tags, x => x == property.stringValue);
            selectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex, UnityEditorInternal.InternalEditorUtility.tags);
            property.stringValue = UnityEditorInternal.InternalEditorUtility.tags[selectedIndex];
        }
    }
#endif
}