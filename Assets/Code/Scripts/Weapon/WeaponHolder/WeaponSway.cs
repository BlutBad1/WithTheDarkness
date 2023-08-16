using UnityEngine;

namespace WeaponNS.WeaponEffectsNS
{
    public class WeaponSway : MonoBehaviour
    {
        [Header("Sway Settings")]
        public PlayerInput _input;
        [SerializeField]
        private float rotateSpeed = 4f;
        [SerializeField]
        private float maxTurn = 3f;
        //[SerializeField]
        //private Camera cam;
        private void OnEnable()
        {
            _input = new PlayerInput();
            _input.Enable();
        }
        private void OnDisable()
        {
            _input.Disable();
            _input.Dispose();
        }
        private void Update()
        {
            Vector2 mouseInput = _input.OnFoot.Look.ReadValue<Vector2>();
            ApplyRotation(GetRotation(mouseInput));
        }
        Quaternion GetRotation(Vector2 mouse)
        {
            mouse = Vector2.ClampMagnitude(mouse, maxTurn);
            Quaternion rotX = Quaternion.AngleAxis(-mouse.y, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(mouse.x, Vector3.up);
            Quaternion targetRot = rotX * rotY;
            return targetRot;
        }
        private void ApplyRotation(Quaternion targetRot)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, rotateSpeed * Time.deltaTime);
        }
    }

}