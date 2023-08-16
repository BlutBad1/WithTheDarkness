using SettingsNS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UINS
{
    public class GameSettingsUI : MonoBehaviour
    {
        public Slider XSensitivitySlider;
        public Slider YSensitivitySlider;
        public Toggle InverseX;
        public Toggle InverseY;
        public Toggle AutoSwitchWeapon;
        public void InitializeSensitivitySliders()
        {
            UnityEngine.UI.Slider.SliderEvent sliderBufferEvent = XSensitivitySlider.onValueChanged;
            XSensitivitySlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            XSensitivitySlider.value = Mathf.Abs(GameSettings.XSensitivity);
            XSensitivitySlider.onValueChanged = sliderBufferEvent;
            ////
            sliderBufferEvent = YSensitivitySlider.onValueChanged;
            YSensitivitySlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            YSensitivitySlider.value = Mathf.Abs(GameSettings.YSensitivity);
            YSensitivitySlider.onValueChanged = sliderBufferEvent;
        }
        public void InitializeToggles()
        {
            InverseX.isOn = GameSettings.XInverse;
            InverseY.isOn = GameSettings.YInverse;
            AutoSwitchWeapon.isOn = GameSettings.ChangeWeaponAfterPickup;
        }
        public void AutoSwitchWeaponToggle(bool IsSwitch)
        {
            if (IsSwitch)
                GameSettings.ChangeWeaponAfterPickup = true;
            else
                GameSettings.ChangeWeaponAfterPickup = false;
        }
        public void ChangeXSensitivity() =>
        GameSettings.XSensitivity = XSensitivitySlider.value;
        public void ChangeYSensitivity() =>
        GameSettings.YSensitivity = YSensitivitySlider.value;
        public void XInverseToggle(bool isInverse)
        {
            if (isInverse)
                GameSettings.XInverse = true;
            else
                GameSettings.XInverse = false;
        }
        public void YInverseToggle(bool isInverse)
        {
            if (isInverse)
                GameSettings.YInverse = true;
            else
                GameSettings.YInverse = false;
        }
        public void ResetRebindingsButton()
        {
            foreach (var map in GameSettings.PlayerInput.asset.actionMaps)
            {
                var bindings = map.bindings;
                for (var i = 0; i < bindings.Count; ++i)
                {
                    if (!string.IsNullOrEmpty(bindings[i].overridePath))
                        map.ApplyBindingOverride(i, new InputBinding { overridePath = null });
                }
            }
            GameSettings.OnKeyRebind?.Invoke();
        }
    }
}