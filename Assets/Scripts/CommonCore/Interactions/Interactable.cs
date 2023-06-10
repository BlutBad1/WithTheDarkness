using PlayerScriptsNS;
using SettingsNS;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractableNS
{
    public abstract class Interactable : MonoBehaviour
    {
        public bool useEvents;
        [SerializeField]
        public string promptMessage;
        protected virtual void Start()
        {
            if (promptMessage == "")
            {
                InteracteBindingSettings.BindInteracteChangeEvent += CheckInteractKey;
                CheckInteractKey();
            }
        }
        public void CheckInteractKey()
        {
            InputManager inputManager = GameObject.Find(MyConstants.CommonConstants.PLAYER).GetComponent<InputManager>();
            int bindingIndex = inputManager.OnFoot.Interact.GetBindingIndexForControl(inputManager.OnFoot.Interact.controls[0]);
            promptMessage = @$"[{InputControlPath.ToHumanReadableString(inputManager.OnFoot.Interact.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice).ToUpper()}]";
        }
        public virtual string OnLook()
        {
            return promptMessage;
        }
        public void BaseInteract()
        {
            if (useEvents)
                GetComponent<InteractionEvent>().OnInteract.Invoke();
            Interact();
        }
        protected virtual void Interact()
        {

        }
    }
}