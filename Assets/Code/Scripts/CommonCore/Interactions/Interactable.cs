using UnityEngine;

namespace InteractableNS
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField]
        protected bool useEvents;
        [SerializeField]
        private string promptMessage;

        protected EntityInteract lastWhoInteracted;

        protected virtual void Start() { }
        public virtual string OnLook()
        {
            return promptMessage;
        }
        public virtual void StartBaseInteraction(EntityInteract creatureInteract)
        {
            lastWhoInteracted = creatureInteract;
            Interact();
            if (useEvents)
                GetComponent<InteractionEvent>().OnInteract?.Invoke();
        }
        public virtual void EndInteraction(EntityInteract creatureInteract) { }
        public virtual void DeleteInteractableScript() =>
          Destroy(this);
        protected abstract void Interact();
    }
}