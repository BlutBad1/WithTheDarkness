using CreatureNS;
using ScriptableObjectNS.Creature;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Creature : MonoBehaviour, ICreature, ISerializationCallbackReceiver
{
    public static List<string> CreatureNames;

    [SerializeField, FormerlySerializedAs("CreatureType"), ListToPopup(typeof(Creature), "CreatureNames")]
    private string creatureType;
    public void OnAfterDeserialize() { }
    public void OnBeforeSerialize() =>
         CreatureNames = CreatureTypes.Instance.Names;
    public GameObject GetCreatureGameObject() =>
         gameObject;
    public string GetCreatureName() =>
         creatureType;
    public abstract void BlockMovement();
    public abstract void UnblockMovement();
    public abstract void SetPositionAndRotation(Vector3 position, Quaternion rotation);
    public abstract void SetSpeedCoef(float speedCoef);
}
