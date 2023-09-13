using UnityEngine;

namespace InteractableNS
{
    public abstract class Interactable : MonoBehaviour
    {
        public bool useEvents;
        [SerializeField]
        public string promptMessage;
        [HideInInspector]
        protected EntityInteract WhoInteracted;
        protected void Start()
        {
        }
        public virtual string OnLook()
        {
            return promptMessage;
        }
        public void BaseInteract(EntityInteract creatureInteract)
        {
            WhoInteracted = creatureInteract;
            if (useEvents)
                GetComponent<InteractionEvent>().OnInteract.Invoke();
            Interact();
        }
        protected virtual void Interact()
        {

        }
        public virtual void DeleteInteractableScript() =>
            Destroy(this);
    }
}