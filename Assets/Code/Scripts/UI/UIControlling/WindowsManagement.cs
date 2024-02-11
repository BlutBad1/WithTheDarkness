using ExtensionMethods;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UIControlling
{
	[System.Serializable]
	public class Window
	{
		public string Name;
		public GameObject[] WindowGameObjects;

		public Window(string Name, GameObject[] WindowGameObjects)
		{
			this.Name = Name;
			this.WindowGameObjects = WindowGameObjects;
		}
		public Window(string Name, GameObject WindowGameObject)
		{
			this.Name = Name;
			if (WindowGameObjects == null)
				WindowGameObjects = new GameObject[0];
			Array.Resize(ref WindowGameObjects, WindowGameObjects.Length + 1);
			WindowGameObjects[^1] = WindowGameObject;
		}
	}
	public class WindowsManagement : MonoBehaviour
	{
		public static WindowsManagement Instance { get; private set; }

		[SerializeField, FormerlySerializedAs("Menu"), Tooltip("The main window.")]
		private Window menu;
		[SerializeField, FormerlySerializedAs("Windows")]
		private Window[] windows;

		public Window CurrentWindow { get; private set; }
		public Window FirstMenu { get => menu; private set => menu = value; }

		private void Awake()
		{
			if (!Instance)
				Instance = this;
			else if (Instance != this)
				Destroy(this);
			CurrentWindow = FirstMenu;
			foreach (var item in CurrentWindow.WindowGameObjects)
				item.gameObject.SetActive(true);
			foreach (var item in windows)
			{
				if (item.WindowGameObjects == null || item.WindowGameObjects.Length <= 0)
				{
					item.WindowGameObjects = new GameObject[0];
					GameObject desiredGameObject = gameObject.GetGameObject(item.Name);
					if (desiredGameObject)
					{
						Array.Resize(ref item.WindowGameObjects, item.WindowGameObjects.Length + 1);
						item.WindowGameObjects[0] = desiredGameObject;
					}
				}
			}
		}
		public void ChangeWindow(string WindowName)
		{
			if (Array.Find(windows, x => x.Name == WindowName) == null)
			{
#if UNITY_EDITOR
				Debug.Log($"Window \"{WindowName}\" is not found!");
#endif
			}
			else
			{
				foreach (var item in CurrentWindow.WindowGameObjects)
					item.SetActive(false);
				CurrentWindow = Array.Find(windows, x => x.Name == WindowName);
				for (int i = 0; i < CurrentWindow.WindowGameObjects.Length; i++)
				{
					if (!CurrentWindow.WindowGameObjects[i])
						CurrentWindow.WindowGameObjects[i] = gameObject.GetGameObject(CurrentWindow.Name);
					CurrentWindow.WindowGameObjects[i].SetActive(true);
				}
			}
		}
		public void CloseCurrentWindow()
		{
			if (CurrentWindow != FirstMenu)
			{
				foreach (var item in CurrentWindow.WindowGameObjects)
					item.gameObject.SetActive(false);
				CurrentWindow = FirstMenu;
				foreach (var item in CurrentWindow.WindowGameObjects)
					item.gameObject.SetActive(true);
			}
		}
	}
}