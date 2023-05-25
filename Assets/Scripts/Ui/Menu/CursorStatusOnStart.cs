using UnityEngine;

namespace UiNS
{
    public class CursorStatusOnStart : MonoBehaviour
    {
        public bool EnableOnStart = false;
        void Start()
        {
            if (EnableOnStart)
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