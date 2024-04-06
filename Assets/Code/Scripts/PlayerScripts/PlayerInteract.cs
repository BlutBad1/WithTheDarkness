using InteractableNS;
using SettingsNS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
	[RequireComponent(typeof(InputManager))]
	public class PlayerInteract : EntityInteract
	{
		[SerializeField]
		private Camera cam;
		[SerializeField, FormerlySerializedAs("InteracteDistance")]
		private float interacteDistance = 3f;
		[SerializeField, FormerlySerializedAs("InteracteRadius")]
		private float interacteRadius = 0.15f;
		[SerializeField]
		private LayerMask interactableLayers;
		[SerializeField]
		private LayerMask obstacleLayers;

		private InputManager inputManager;
		private IPlayerUI playerUi;
		private Interactable interactable = null;
		private Ray ray;
		private string promptMessageifEmpty;
		private float distance = 0;

		public float InteracteDistance { get => interacteDistance; set => interacteDistance = value; }

		private void Start()
		{
			GetComponents();
			CheckInteractKey();
		}
		private void OnEnable()
		{
			GameSettings.OnInteracteRebind += CheckInteractKey;
		}
		private void OnDisable()
		{
			GameSettings.OnInteracteRebind -= CheckInteractKey;
		}
		private void Update()
		{
			playerUi?.UpdateText(string.Empty);
			ray = new Ray(cam.transform.position, cam.transform.forward);
			if (Physics.SphereCast(ray, interacteRadius, out RaycastHit hitInfo, interacteDistance, interactableLayers))
			{
				distance = hitInfo.distance;
				if (!Physics.Raycast(ray, out RaycastHit hitInfo1, hitInfo.distance, obstacleLayers) || (obstacleLayers.value & (1 << hitInfo1.collider.gameObject.layer)) == 0)
				{
					interactable = UtilitiesNS.Utilities.GetComponentFromGameObject<Interactable>(hitInfo.collider.gameObject);
					if (interactable != null)
					{
						if (playerUi != null)
						{
							if (string.IsNullOrEmpty(interactable.OnLook()))
								playerUi?.UpdateText(promptMessageifEmpty);
							else
								playerUi?.UpdateText(interactable.OnLook());
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
				else if (interactable != null)
				{
					interactable.EndInteraction(this);
					interactable = null;
				}
			}
			else if (interactable != null)
			{
				interactable.EndInteraction(this);
				interactable = null;
			}
		}
		private void CheckInteractKey()
		{
			int bindingIndex = inputManager.OnFoot.Interact.GetBindingIndexForControl(inputManager.OnFoot.Interact.controls[0]);
			promptMessageifEmpty = @$"[{InputControlPath.ToHumanReadableString(inputManager.OnFoot.Interact.bindings[bindingIndex].effectivePath,
				InputControlPath.HumanReadableStringOptions.OmitDevice).ToUpper()}]";
		}
		private void GetComponents()
		{
			playerUi = GetComponent<IPlayerUI>();
			inputManager = GetComponent<InputManager>();
		}
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(ray.origin + ray.direction * interacteDistance, interacteRadius);
			Gizmos.color = Color.white;
			Gizmos.DrawRay(ray.origin, ray.direction * distance);
		}
	}
}