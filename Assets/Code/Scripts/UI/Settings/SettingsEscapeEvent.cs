using TMPro;
using UIControlling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace SettingsNS
{
	public class SettingsEscapeEvent : UIOnCloseEvent
	{
		[Tooltip("Settings menu won't close if some dropdown is opened.")]
		[SerializeField, FormerlySerializedAs("DropdownSelectebles")]
		public TMP_Dropdown[] dropdownSelectables;

		private EventSystem inputSystem;
		private WindowsManagement windowsManagement;

		private void Start()
		{
			DefineWindowsManagementAndAddListener();
		}
		private void DefineWindowsManagementAndAddListener()
		{
			windowsManagement = FindObjectOfType<WindowsManagement>();
			unityEvent.AddListener(windowsManagement.CloseCurrentWindow);
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
				foreach (var dropdown in dropdownSelectables)
				{
					if (dropdown.IsExpanded)
						return;
				}
			}
			base.EventInvoke();
		}
	}
}