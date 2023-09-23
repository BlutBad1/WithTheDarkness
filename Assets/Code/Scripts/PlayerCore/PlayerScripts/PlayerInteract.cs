using InteractableNS;
using SettingsNS;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(InputManager)), RequireComponent(typeof(PlayerLook))]
    public class PlayerInteract : EntityInteract
    {
        private Camera cam;
        [SerializeField]
        private float distance = 3f;
        [SerializeField]
        private LayerMask interactableLayers;
        [SerializeField]
        private LayerMask obstacleLayers;
        private PlayerUi playerUi;
        private InputManager inputManager;
        private Interactable interactable = null;
        private Ray ray;
        private string promptMessageifEmpty;
        void Start()
        {
            cam = GetComponent<PlayerLook>().cam;
            playerUi = GetComponent<PlayerUi>();
            inputManager = GetComponent<InputManager>();
            GameSettings.OnInteracteRebind += CheckInteractKey;
            CheckInteractKey();
        }
        private void OnDisable()
        {
            GameSettings.OnInteracteRebind -= CheckInteractKey;
        }
        // Update is called once per frame
        void Update()
        {
            playerUi?.UpdateText(string.Empty);
            ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.SphereCast(ray, 0.2f, out RaycastHit hitInfo, distance, interactableLayers | obstacleLayers))
            {
                if ((obstacleLayers.value & (1 << hitInfo.collider.gameObject.layer)) == 0)
                {
                    if (hitInfo.collider.GetComponent<Interactable>() != null)
                        interactable = hitInfo.collider.GetComponent<Interactable>();
                    else if (hitInfo.collider.transform.parent.gameObject.GetComponent<Interactable>() != null)
                        interactable = hitInfo.collider.transform.parent.gameObject.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        if (playerUi)
                        {
                            if (interactable.promptMessage == "")
                                playerUi?.UpdateText(promptMessageifEmpty);
                            else
                                playerUi?.UpdateText(interactable.promptMessage);
                        }
                        if (inputManager.OnFoot.Interact.triggered)
                            interactable.StartBaseInteraction(this);
                        else if (!inputManager.OnFoot.Interact.IsPressed())
                        {
                            interactable.EndInteraction(this);
                            interactable = null;
                        }
                    }
                }
            }
            else if (interactable != null)
            {
                interactable.EndInteraction(this);
                interactable = null;
            }
        }
        public void CheckInteractKey()
        {
            InputManager inputManager = GameObject.Find(MyConstants.CommonConstants.PLAYER).GetComponent<InputManager>();
            int bindingIndex = inputManager.OnFoot.Interact.GetBindingIndexForControl(inputManager.OnFoot.Interact.controls[0]);
            promptMessageifEmpty = @$"[{InputControlPath.ToHumanReadableString(inputManager.OnFoot.Interact.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice).ToUpper()}]";
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ray.origin + ray.direction * distance, 0.1f);
        }
    }
}