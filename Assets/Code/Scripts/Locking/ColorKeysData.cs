using ScriptableObjectNS.Locking;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class ColorKeyData : KeyData
{
    [ColorUsageAttribute(true, true)]
    public Color KeyHighlightColor;
    public ColorKeyData(bool isGeneric, string genericKeyName, Color keyHighlightColor) : base(isGeneric, genericKeyName)
    {
        KeyHighlightColor = keyHighlightColor;
    }
}
[CreateAssetMenu(fileName = "KeyColorData", menuName = "ScriptableObject/Locking/KeyColorData")]
public class ColorKeysData : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<ColorKeyData> colorKeysData = new List<ColorKeyData>();

    public void OnAfterDeserialize() { }
    public void OnBeforeSerialize()
    {
        CheckAndChangeKeyData();
    }

    public Color GetColorByKey(KeyData keyData) =>
        colorKeysData.Find(x => x.IsGeneric == keyData.IsGeneric && x.KeyName == keyData.KeyName).KeyHighlightColor;
    private void CheckAndChangeKeyData()
    {
        LockingTypeData lockingTypeData = LockingTypeData.Instance;
        foreach (var lockingTypeName in lockingTypeData.LockingTypes)
        {
            int amountOfObjects = colorKeysData.Count(x => x.KeyName == lockingTypeName);
            if (amountOfObjects == 0)
                AddNewObjects(lockingTypeName);
            else if (amountOfObjects == 1)
                AddNewObject(lockingTypeName);
            else if (amountOfObjects > 2)
                DeleteExtras(lockingTypeName, amountOfObjects);
            CheckGenericAmount(lockingTypeName);
        }
    }
    private void AddNewObjects(string keyName)
    {
        colorKeysData.Add(new ColorKeyData(true, keyName, Color.white));
        colorKeysData.Add(new ColorKeyData(false, keyName, Color.white));
    }
    private void AddNewObject(string keyName)
    {
        ColorKeyData colorKeyDataInList = colorKeysData.Find(x => x.KeyName == keyName);
        colorKeysData.Add(new ColorKeyData(!colorKeyDataInList.IsGeneric, keyName, Color.white));
    }
    private void DeleteExtras(string keyName, int amountOfObjects)
    {
        while (amountOfObjects != 2)
        {
            colorKeysData.Remove(colorKeysData.Last(x => x.KeyName == keyName));
            amountOfObjects--;
        }
    }
    private void CheckGenericAmount(string keyName)
    {
        ColorKeyData[] keyGeneric = colorKeysData.FindAll(x => x.KeyName == keyName && x.IsGeneric).ToArray();
        if (keyGeneric.Length == 2)
            keyGeneric[0].IsGeneric = false;
        else if (keyGeneric.Length == 0)
            colorKeysData.Find(x => x.KeyName == keyName && !x.IsGeneric).IsGeneric = true;
    }
}
