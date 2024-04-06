using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
    public class FPSPlayerCreature : PlayerCreature
    {
        [SerializeField, FormerlySerializedAs("PlayerMotor")]
        protected PlayerMotor playerMotor;

        public override void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            BlockMovement();
            transform.position = position;
            transform.rotation = rotation;
            Task.Delay(5).GetAwaiter();
            UnblockMovement();
        }
        public override void BlockMovement() =>
            playerMotor.Character.enabled = false;
        public override void UnblockMovement() =>
            playerMotor.Character.enabled = true;
        protected override void SetCurrentSpeed() =>
            playerMotor.SpeedCoef = CurrentSpeedCoefficient;
    }
}