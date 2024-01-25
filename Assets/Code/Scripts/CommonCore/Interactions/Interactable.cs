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
        protected virtual void Start()
        {
        }
        public virtual string OnLook()
        {
            return promptMessage;
        }
        public virtual void StartBaseInteraction(EntityInteract creatureInteract)
        {
            LastWhoInteracted = creatureInteract;
            Interact();
            if (useEvents)
                GetComponent<InteractionEvent>().OnInteract?.Invoke();
        }
        public virtual void EndInteraction(EntityInteract creatureInteract)
        {
        }
        public virtual void DeleteInteractableScript() =>
          Destroy(this);
        protected virtual void Interact()
        {
        }
    }
}