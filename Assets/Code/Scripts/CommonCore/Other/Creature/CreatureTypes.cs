using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Creature Types", menuName = "ScriptableObject/CreatureTypes")]
public class CreatureTypes : ScriptableObject
{
    //private void Awake()
    //{
    //    if (Instance == null)
    //        Instance = this;
    //    else if (Instance != this)
    //        DestroyImmediate(this);
    //}
    static CreatureTypes Instance;
    public static CreatureTypes GetInstance()
    {
        if (Instance == null)
            Instance = Resources.Load(MyConstants.DataConstants.CREATURE_TYPES_INSTANCE) as CreatureTypes;
        if (Instance == null)
            ScriptableObject.CreateInstance(typeof(CreatureTypes));
        return Instance;
    }
    public List<string> Names;
}
