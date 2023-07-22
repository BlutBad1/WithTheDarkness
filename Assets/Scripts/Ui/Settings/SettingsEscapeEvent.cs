using TMPro;
using UIControlling;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SettingsNS
{
    public class SettingsEscapeEvent : WindowEscapeEvent
    {
        EventSystem inputSystem;
        WindowsManagement windowsManagement;
        //Maybe it's not the best solution, but it works 
        public TMP_Dropdown[] DropdownSelectebles;
        private void Start()
        {
            DefineWindowsManagementAndAddListener();
        }
        void DefineWindowsManagementAndAddListener()
        {
            windowsManagement = FindObjectOfType<WindowsManagement>();
            UnityEvent.AddListener(windowsManagement.CloseCurrentWindow);
        }
        public override void EventInvoke()
        {
            if (windowsManagement != FindObjectOfType<WindowsManagement>())
                DefineWindowsManagementAndAddListener();
            if (!inputSystem)
                inputSystem = FindObjectOfType<EventSystem>();
            if (inputSystem && inputSystem.currentSelectedGameObject)
            {
                if (inputSystem.currentSelectedGameObject.TryGetComponent(out TMP_InputField inputField) && inputField.isFocused)
                    return;
                foreach (var dropdown in DropdownSelectebles)
                {
                    if (dropdown.IsExpanded)
                        return;
                }
            }
            base.EventInvoke();
        }
    }
}