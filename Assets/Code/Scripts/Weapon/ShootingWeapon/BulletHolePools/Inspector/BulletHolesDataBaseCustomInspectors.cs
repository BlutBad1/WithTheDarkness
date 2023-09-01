using PoolableObjectsNS;
using System;
using UnityEditor;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS.Inspector
{
    public class BulletHolesDataBaseCustomInspectors
    {
    }
    class BulletObjectTypePopupField : PropertyAttribute
    {
    }
    class BulletObjectTagPopupField : PropertyAttribute
    {
    }
#if UNITY_EDITOR
    // Custom property inspector of type 'BulletObjectTypePopupField'
    [CustomPropertyDrawer(typeof(BulletObjectTypePopupField))]
    public class BulletObjectPopupFieldDrawer : PropertyDrawer
    {
        BulletHolesPoolsManager bulletHolesPool = null;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (!bulletHolesPool)
            {
                bulletHolesPool = GameObject.FindObjectOfType<BulletHolesPoolsManager>();
                return;
            }
            int selectedIndex = 0;
            if (property.stringValue != null || property.stringValue != "")
            {
                selectedIndex = Array.FindIndex(bulletHolesPool.pools, x => x.Name == property.stringValue);
            }
            selectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            position.y += 1f;

            selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex, bulletHolesPool.GetAllPoolNames());

            property.stringValue = bulletHolesPool.pools[selectedIndex].Name;
        }
    }
    // Custom property inspector of type 'BulletObjectTagPopupField'
    [CustomPropertyDrawer(typeof(BulletObjectTagPopupField))]
    public class BulletObjectTagPopupFieldDrawer : PropertyDrawer
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