using ExtensionMethods;
using System;
using UnityEngine;

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
        [Tooltip("The main window.")]
        public Window Menu;
        public Window[] Windows;
        Window currentWindow;
        WindowsManagement instance;
        private void Awake()
        {
            if (!instance)
                instance = this;
            else if (instance != this)
                Destroy(this);
            currentWindow = Menu;
            foreach (var item in currentWindow.WindowGameObjects)
                item.gameObject.SetActive(true);
            foreach (var item in Windows)
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
            if (Array.Find(Windows, x => x.Name == WindowName) == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Window \"{WindowName}\" is not found!");
#endif
            }
            else
            {
                foreach (var item in currentWindow.WindowGameObjects)
                    item.SetActive(false);
                currentWindow = Array.Find(Windows, x => x.Name == WindowName);
                for (int i = 0; i < currentWindow.WindowGameObjects.Length; i++)
                {
                    if (!currentWindow.WindowGameObjects[i])
                        currentWindow.WindowGameObjects[i] = gameObject.GetGameObject(currentWindow.Name);
                    currentWindow.WindowGameObjects[i].SetActive(true);
                }
            }
        }
        public void CloseCurrentWindow()
        {
            if (currentWindow != Menu)
            {
                foreach (var item in currentWindow.WindowGameObjects)
                    item.gameObject.SetActive(false);
                currentWindow = Menu;
                foreach (var item in currentWindow.WindowGameObjects)
                    item.gameObject.SetActive(true);
            }
        }
    }
}