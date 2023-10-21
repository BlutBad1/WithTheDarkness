using LightNS.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponManagement;

namespace PlayerScriptsNS
{
    public class PlayerLightUp : MonoBehaviour
    {
        public InputManager InputManager;
        public LampAndHandAnimations LampAndHandAnimations;
        public WeaponManager WeaponManager;
        public Light MainLight;
        public float LightUpRange;
        public float LightUpSpotAngle;
        public float LightUpDuration;
        [HideInInspector]
        public bool isCanLightUp = true;
        float OriginRange;
        float OriginSpotAngle;
        bool isActive = false;
        List<Coroutine> currentCoroutines = new List<Coroutine>();
        private void Start()
        {
            if (InputManager == null)
                InputManager = GetComponent<InputManager>();
            if (!MainLight)
                MainLight = GameObject.Find(MyConstants.CommonConstants.MAIN_LIGHT).GetComponent<Light>();
            OriginRange = MainLight.range;
            OriginSpotAngle = MainLight.spotAngle;
            InputManager.OnFoot.LightUp.started += EnableLightUpInvoke;
            //InputManager.OnFoot.LightUp.performed += DisableLightUpInvoke;
            InputManager.OnFoot.LightUp.canceled += DisableLightUpInvoke;
            WeaponManager.OnWeaponChange += DisableLightUp;
            LampAndHandAnimations.OnLampReloading += LampReloading;
            LampAndHandAnimations.OnLampAfterReloading += LampAfterReloading;
        }
        private void OnDisable()
        {
            InputManager.OnFoot.LightUp.started -= EnableLightUpInvoke;
            // InputManager.OnFoot.LightUp.performed -= DisableLightUpInvoke;
            InputManager.OnFoot.LightUp.canceled -= DisableLightUpInvoke;
            WeaponManager.OnWeaponChange -= DisableLightUp;
            LampAndHandAnimations.OnLampReloading -= LampReloading;
            LampAndHandAnimations.OnLampAfterReloading -= LampAfterReloading;
        }
        public void EnableLightUp()
        {
            Weapon weapon = WeaponManager.GetCurrentSelectedWeapon();
            if (WeaponManager.Lamp.activeInHierarchy && (weapon == null || !weapon.WeaponData.IsTwoHanded) && isCanLightUp)
            {
                if (currentCoroutines != null)
                {
                    for (int i = 0; i < currentCoroutines.Count; i++)
                        StopCoroutine(currentCoroutines[i]);
                    currentCoroutines.Clear();
                }
                currentCoroutines.Add(StartCoroutine(ChangeValueByTime((result) =>
                {
                    MainLight.range = result;
                }, MainLight.range, LightUpRange, LightUpDuration)));
                currentCoroutines.Add(StartCoroutine(ChangeValueByTime((result) =>
                {
                    MainLight.spotAngle = result;
                }, MainLight.spotAngle, LightUpSpotAngle, LightUpDuration)));
                LampAndHandAnimations.EnableLightUp();
                isActive = true;
            }
        }
        public void DisableLightUp()
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
                    MainLight.range = result;
                }, MainLight.range, OriginRange, LightUpDuration)));
                currentCoroutines.Add(StartCoroutine(ChangeValueByTime((result) =>
                {
                    MainLight.spotAngle = result;
                }, MainLight.spotAngle, OriginSpotAngle, LightUpDuration)));
                LampAndHandAnimations.DisableLightUp();
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