using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ListToMultiplePopup : PropertyAttribute
{
    public Type MyType;
    public string ListSerializeName;
    public ListToMultiplePopup(Type myType, string listSerializeName)
    {
        MyType = myType;
        ListSerializeName = listSerializeName;
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ListToMultiplePopup))]
public class ListToMultipleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Selection.objects.Length <= 1)
        {
            ListToMultiplePopup atb = attribute as ListToMultiplePopup;
            List<string> options = null;
            int selectedIndex = property.intValue;
            if (atb.MyType.GetField(atb.ListSerializeName) != null)
                options = atb.MyType.GetField(atb.ListSerializeName).GetValue(atb.MyType) as List<string>;
            if (options != null && options.Count != 0)
            {
                selectedIndex = EditorGUI.MaskField(position, label, selectedIndex, options.ToArray());
                property.intValue = selectedIndex;
            }
            else
                EditorGUI.PropertyField(position, property, label);
        }
        else
            EditorGUI.PropertyField(position, property, label);
    }
}
#endif
