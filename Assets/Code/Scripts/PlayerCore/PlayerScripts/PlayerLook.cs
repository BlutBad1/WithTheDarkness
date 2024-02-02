using UnityEngine;
namespace PlayerScriptsNS
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

        private float xRotation = 0f;
        private InputManager inputManager;
        private bool isLookingInputLocked = false;

        public Vector3 PlayerCameraCurRotation { get; private set; }

        private void Start()
        {
            Cursor.visible = false;
#if UNITY_EDITOR
            inputManager = GetComponent<InputManager>();
#endif
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (inputManager != null && inputManager.OnFoot.CurFreeze.triggered)
            {
                if (!isLookingInputLocked)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    SetLookingInputLockStats(true);
                }
                else
                    SetLookingInputLockStats(false);
            }
        }
#endif
        public void SetLookingInputLockStats(bool isLocked) =>
            isLookingInputLocked = isLocked;
        public void ProcessLook(Vector2 input)
        {
            if (!isLookingInputLocked)
            {
                float mouseX = input.x;
                float mouseY = input.y;
                xRotation -= (mouseY / 80f) * SettingsNS.GameSettings.YSensitivity;
                xRotation = Mathf.Clamp(xRotation, -80f, 70f);
                cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
                float yRotation = (mouseX / 80f) * SettingsNS.GameSettings.XSensitivity;
                PlayerCameraCurRotation = new Vector3(xRotation, yRotation, 0);
                transform.Rotate(Vector3.up * (mouseX / 80f) * SettingsNS.GameSettings.XSensitivity);
            }
        }
    }
}