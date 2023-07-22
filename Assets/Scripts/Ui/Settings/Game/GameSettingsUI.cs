using SettingsNS;
using UnityEngine;
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
            XSensitivitySlider.value = Mathf.Abs(GameSettings.XSensitivity);
            YSensitivitySlider.value = Mathf.Abs(GameSettings.YSensitivity);
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
    }
}