using CreatureNS;
using ScriptableObjectNS.Creature;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PlayerScriptsNS
{
    public class PlayerCreature : MonoBehaviour, ICreature, ISerializationCallbackReceiver
    {
        [HideInInspector]
        public static List<string> CreatureNames;
        [ListToPopup(typeof(PlayerCreature), "CreatureNames")]
        public string CreatureType;
        public InputManager PlayerInputManager;
        public PostProcessProfile PostProcessProfile;
        private MotionBlur motionBlurEffect;
        private void Start()
        {
            if (PostProcessProfile.TryGetSettings(out motionBlurEffect))
                motionBlurEffect.active = false;
        }
        public string GetCreatureName() =>
               CreatureType;
        public GameObject GetCreatureGameObject() =>
            gameObject;
        public void OnBeforeSerialize() =>
            CreatureNames = CreatureTypes.Instance.Names;
        public void OnAfterDeserialize()
        {
        }
        public void BlockMovement()
        {
            if (motionBlurEffect)
                motionBlurEffect.active = false;
            PlayerInputManager.SetMovingLock(true);
        }
        public void UnBlockMovement()
        {
            if (motionBlurEffect)
                motionBlurEffect.active = true;
            PlayerInputManager.SetMovingLock(false);
        }
    }
}