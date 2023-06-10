using InteractableNS;
using UnityEngine;
namespace PlayerScriptsNS
{
    public class PlayerInteract : MonoBehaviour
    {
        private Camera cam;
        [SerializeField]
        private float distance = 3f;
        [SerializeField]
        private LayerMask mask;
        private PlayerUi playerUi;
        private InputManager inputManager;
        Ray ray;
        void Start()
        {
            cam = GetComponent<PlayerLook>().cam;
            playerUi = GetComponent<PlayerUi>();
            inputManager = GetComponent<InputManager>();
        }

        // Update is called once per frame
        void Update()
        {
            playerUi.UpdateText(string.Empty);
            ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.SphereCast(ray, 0.1f, out RaycastHit hitInfo, distance, mask))
            {
                if (hitInfo.collider.GetComponent<Interactable>() != null)
                {
                    Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                    playerUi.UpdateText(interactable.promptMessage);

                    if (inputManager.OnFoot.Interact.triggered)
                        interactable.BaseInteract();

                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ray.origin + ray.direction * distance, 0.1f);
        }
    }
}