using CreatureNS;
using ScriptableObjectNS.Creature;
using System.Collections;
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
        public PlayerMotor PlayerMotor;
        public PostProcessVolume PostProcessVolumeMotionBlur;
        private Coroutine teleportCoroutine;
        private MotionBlur motionBlur;
        void Start()
        {
            PostProcessVolumeMotionBlur.profile.TryGetSettings(out motionBlur);
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
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            if (teleportCoroutine != null)
                StopCoroutine(teleportCoroutine);
            teleportCoroutine = StartCoroutine(Teleport(position, rotation));
        }
        public void SetPositionAndRotationWithoutBlur(Vector3 position, Quaternion rotation)
        {
            if (teleportCoroutine != null)
                StopCoroutine(teleportCoroutine);
            teleportCoroutine = StartCoroutine(Teleport(position, rotation, false));
        }
        private IEnumerator Teleport(Vector3 position, Quaternion rotation, bool enableBlurAfterTeleport = true)
        {
            SetMotionBlurState(false);
            PlayerMotor.GetCharacterController().enabled = false;
            transform.position = position;
            transform.rotation = rotation;
            yield return new WaitForSeconds(0.005f);
            PlayerMotor.GetCharacterController().enabled = true;
            SetMotionBlurState(enableBlurAfterTeleport);
            teleportCoroutine = null;
        }
        public void SetMotionBlurState(bool state)
        {
            motionBlur.active = state;
        }
        public void BlockMovement()
        {
            PlayerMotor.GetCharacterController().enabled = false;
        }
        public void UnBlockMovement()
        {
            PlayerMotor.GetCharacterController().enabled = true;
        }
    }
}