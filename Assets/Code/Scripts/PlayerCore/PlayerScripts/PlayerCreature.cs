using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
    public class PlayerCreature : Creature
    {
        [SerializeField, FormerlySerializedAs("PlayerMotor")]
        private PlayerMotor playerMotor;
        [SerializeField, FormerlySerializedAs("PostProcessVolumeMotionBlur")]
        private PostProcessVolume postProcessVolumeMotionBlur;

        private Coroutine teleportCoroutine;
        private MotionBlur motionBlur;

        private void Start()
        {
            postProcessVolumeMotionBlur.profile.TryGetSettings(out motionBlur);
        }
        public override void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            if (teleportCoroutine != null)
                StopCoroutine(teleportCoroutine);
            teleportCoroutine = StartCoroutine(Teleport(position, rotation));
        }
        public override void BlockMovement() =>
            playerMotor.GetCharacterController().enabled = false;
        public override void UnblockMovement() =>
            playerMotor.GetCharacterController().enabled = true;
        public override void SetSpeedCoef(float speedCoef) =>
            playerMotor.SetSpeedCoef(speedCoef);
        private void SetMotionBlurState(bool state) =>
          motionBlur.active = state;
        private IEnumerator Teleport(Vector3 position, Quaternion rotation, bool enableBlurAfterTeleport = true)
        {
            SetMotionBlurState(false);
            playerMotor.GetCharacterController().enabled = false;
            transform.position = position;
            transform.rotation = rotation;
            yield return new WaitForSeconds(0.005f);
            playerMotor.GetCharacterController().enabled = true;
            SetMotionBlurState(enableBlurAfterTeleport);
            teleportCoroutine = null;
        }
    }
}