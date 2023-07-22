using UnityEngine;
namespace PlayerScriptsNS
{
    public class PlayerLook : MonoBehaviour
    {
        public Camera cam;
        private float xRotation = 0f;
        private InputManager inputManager;//dev delete it when developing ended  
        private bool isFreez = false;//dev 
        private void Start()
        {
            Cursor.visible = false;
#if UNITY_EDITOR
            inputManager = GetComponent<InputManager>();//dev
#endif
        }
#if UNITY_EDITOR
        private void Update()//dev all Update method for testing   
        {
            if (inputManager != null && inputManager.OnFoot.CurFreeze.triggered)
            {
                if (!isFreez)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    isFreez = true;
                }
                else
                {
                    isFreez = false;
                }
            }
        }
#endif
        public void ProcessLook(Vector2 input)
        {
            if (!isFreez)//dev
            {
                float mouseX = input.x;
                float mouseY = input.y;
                xRotation -= (mouseY * Time.deltaTime) * SettingsNS.GameSettings.YSensitivity;
                xRotation = Mathf.Clamp(xRotation, -80f, 70f);
                cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
                transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * SettingsNS.GameSettings.XSensitivity);
            }
        }
    }
}