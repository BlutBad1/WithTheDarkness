using UnityEngine;

namespace InteractableNS
{
    public abstract class Interactable : MonoBehaviour
    {
        public bool useEvents;
        [SerializeField]
        public string promptMessage;
        [HideInInspector]
        public EntityInteract LastWhoInteracted;
        protected void Start()
        {
        }
        public virtual string OnLook()
        {
            return promptMessage;
        }
        public virtual void StartBaseInteraction(EntityInteract creatureInteract)
        {
            LastWhoInteracted = creatureInteract;
            if (useEvents)
                GetComponent<InteractionEvent>().OnInteract.Invoke();
            Interact();
        }
        public virtual void EndInteraction(EntityInteract creatureInteract)
        {
        }
        protected virtual void Interact()
        {
        }
        public virtual void DeleteInteractableScript() =>
            Destroy(this);
    }
}