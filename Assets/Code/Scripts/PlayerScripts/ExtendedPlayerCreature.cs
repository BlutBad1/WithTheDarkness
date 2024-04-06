using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
	public class ExtendedPlayerCreature : FPSPlayerCreature
	{
		[SerializeField, FormerlySerializedAs("PostProcessVolumeMotionBlur")]
		private PostProcessVolume postProcessVolumeMotionBlur;

		private MotionBlur motionBlur;

		private void Start()
		{
			postProcessVolumeMotionBlur.profile.TryGetSettings(out motionBlur);
		}
		public override void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			SetMotionBlurState(false);
			base.SetPositionAndRotation(position, rotation);
			SetMotionBlurState(true);
		}
		private void SetMotionBlurState(bool state) =>
		  motionBlur.active = state;
	}
}