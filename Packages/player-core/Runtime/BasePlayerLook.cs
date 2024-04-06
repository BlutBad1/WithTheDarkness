using UnityEngine;

namespace PlayerScriptsNS
{
	public abstract class BasePlayerLook : MonoBehaviour
	{
		public abstract Vector3 PlayerCameraCurRotation { get; protected set; }
	}
}