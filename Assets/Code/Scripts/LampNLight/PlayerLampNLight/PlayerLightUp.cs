using LightNS.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponManagement;

namespace PlayerScriptsNS
{
	public class PlayerLightUp : MonoBehaviour
	{
		[SerializeField, FormerlySerializedAs("InputManager")]
		private InputManager inputManager;
		[SerializeField, FormerlySerializedAs("LampAndHandAnimations")]
		private LampAndHandAnimations lampAndHandAnimations;
		[SerializeField, FormerlySerializedAs("WeaponManager")]
		private WeaponManager weaponManager;
		[SerializeField, FormerlySerializedAs("MainLight")]
		private Light mainLight;
		[SerializeField, FormerlySerializedAs("LightUpRange")]
		private float lightUpRange;
		[SerializeField, FormerlySerializedAs("LightUpSpotAngle")]
		private float lightUpSpotAngle;
		[SerializeField, FormerlySerializedAs("LightUpDuration")]
		private float lightUpDuration;

		private bool isCanLightUp = true;
		private float OriginRange;
		private float OriginSpotAngle;
		private bool isActive = false;
		private List<Coroutine> currentCoroutines = new List<Coroutine>();

		public float LightUpRange { get => lightUpRange; set => lightUpRange = value; }
		public float LightUpSpotAngle { get => lightUpSpotAngle; set => lightUpSpotAngle = value; }

		private void Start()
		{
			OriginRange = mainLight.range;
			OriginSpotAngle = mainLight.spotAngle;

		}
		private void OnEnable()
		{
			inputManager.OnFoot.LightUp.started += EnableLightUpInvoke;
			inputManager.OnFoot.LightUp.canceled += DisableLightUpInvoke;
			weaponManager.OnWeaponChange += DisableLightUp;
			lampAndHandAnimations.OnLampReloading += LampReloading;
			lampAndHandAnimations.OnLampAfterReloading += LampAfterReloading;
		}
		private void OnDisable()
		{
			inputManager.OnFoot.LightUp.started -= EnableLightUpInvoke;
			inputManager.OnFoot.LightUp.canceled -= DisableLightUpInvoke;
			weaponManager.OnWeaponChange -= DisableLightUp;
			lampAndHandAnimations.OnLampReloading -= LampReloading;
			lampAndHandAnimations.OnLampAfterReloading -= LampAfterReloading;
		}
		private void EnableLightUp()
		{
			Weapon weapon = weaponManager.GetCurrentSelectedWeapon();
			if (weaponManager.Lamp.activeInHierarchy && (weapon == null || !weapon.WeaponData.IsTwoHanded) && isCanLightUp)
			{
				if (currentCoroutines != null)
				{
					for (int i = 0; i < currentCoroutines.Count; i++)
						StopCoroutine(currentCoroutines[i]);
					currentCoroutines.Clear();
				}
				currentCoroutines.Add(StartCoroutine(ChangeValueByTime((result) =>
				{
					mainLight.range = result;
				}, mainLight.range, LightUpRange, lightUpDuration)));
				currentCoroutines.Add(StartCoroutine(ChangeValueByTime((result) =>
				{
					mainLight.spotAngle = result;
				}, mainLight.spotAngle, LightUpSpotAngle, lightUpDuration)));
				lampAndHandAnimations.EnableLightUp();
				isActive = true;
			}
		}
		private void DisableLightUp()
		{
			if (isActive)
			{
				if (currentCoroutines != null)
				{
					for (int i = 0; i < currentCoroutines.Count; i++)
						StopCoroutine(currentCoroutines[i]);
					currentCoroutines.Clear();
				}
				currentCoroutines.Add(StartCoroutine(ChangeValueByTime((result) =>
				{
					mainLight.range = result;
				}, mainLight.range, OriginRange, lightUpDuration)));
				currentCoroutines.Add(StartCoroutine(ChangeValueByTime((result) =>
				{
					mainLight.spotAngle = result;
				}, mainLight.spotAngle, OriginSpotAngle, lightUpDuration)));
				lampAndHandAnimations.DisableLightUp();
				isActive = false;
			}
		}
		private void EnableLightUpInvoke(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
		   EnableLightUp();
		private void DisableLightUpInvoke(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
		   DisableLightUp();
		private void LampReloading()
		{
			isCanLightUp = false;
			DisableLightUp();
		}
		private void LampAfterReloading() =>
			isCanLightUp = true;
		private IEnumerator ChangeValueByTime(System.Action<float> callback, float value, float newValue, float duration)
		{
			float time = 0f;
			float beginValue = value;
			while (time < 1f)
			{
				value = Mathf.Lerp(beginValue, newValue, time);
				if (callback != null) callback(value);
				time += Time.deltaTime / duration;
				yield return null;
			}
		}
	}
}