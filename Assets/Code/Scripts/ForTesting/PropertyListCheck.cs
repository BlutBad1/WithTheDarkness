using System.Collections.Generic;
using UnityEngine;

public class PropertyListCheck : MonoBehaviour, ISerializationCallbackReceiver
{
    [HideInInspector]
    public static List<string> creatureNames;
    [ListToPopup(typeof(PropertyListCheck), "creatureNames")]
    public string selectedOption;
    [ListToMultiplePopup(typeof(PropertyListCheck), "creatureNames")]
    public int choice;
    public void OnAfterDeserialize()
    {
    }
    public void OnBeforeSerialize()
    {
        SetCreatureNames();
    }

    public void SetCreatureNames()
    {
        creatureNames = CreatureTypes.GetInstance().Names;
    }
    [ContextMenu("PrintStringVariable")]
    public void PrintStringVariable()
    {
        Debug.Log(selectedOption);
    }
    [ContextMenu("PrintIntVariable")]
    public void PrintIntVariable()
    {
        for (int i = 0; i < creatureNames.Count; i++)
        {
            if ((choice & (1 << i)) != 0)
                Debug.Log(creatureNames[i]);
        }
    }
}
