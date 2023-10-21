using CreatureNS;
using ScriptableObjectNS.Creature;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerScriptsNS
{
    public class PlayerCreature : MonoBehaviour, ICreature, ISerializationCallbackReceiver
    {
        [HideInInspector]
        public static List<string> CreatureNames;
        [ListToPopup(typeof(PlayerCreature), "CreatureNames")]
        public string CreatureType;
        public InputManager PlayerInputManager;
        public string GetCreatureName() =>
               CreatureType;
        public GameObject GetCreatureGameObject() =>
            gameObject;
        public void OnBeforeSerialize() =>
            CreatureNames = CreatureTypes.Instance.Names;
        public void OnAfterDeserialize()
        {
        }
        public void BlockMovement() =>
            PlayerInputManager.SetMovingLock(true);
        public void UnBlockMovement() =>
            PlayerInputManager.SetMovingLock(false);
    }
}