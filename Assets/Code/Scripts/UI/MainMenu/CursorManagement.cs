using UnityEngine;
namespace UIControlling
{
    public class CursorManagement : MonoBehaviour
    {
        public bool ShowCursorOnEnable = false;
        public bool HideCursorOnDisable = false;
        private void OnEnable()
        {
            ChangeCursorVisibility(ShowCursorOnEnable);
        }
        private void OnDisable()
        {
            ChangeCursorVisibility(!HideCursorOnDisable);
        }
        public void ChangeCursorVisibility(bool isVisible)
        {
            if (isVisible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}