using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InteractableNS
{
    public class ProgressingInteractable : Interactable
    {
        public float InteractingSpeed = 1.5f;
        public float CoroutineDelay = 0.01f;
        [HideInInspector]
        public bool IsInteracting = false;
        public Coroutine InteractingCoroutine;
        [HideInInspector]
        public float InteractionProgress = 0f;
        public sealed override void StartBaseInteraction(EntityInteract creatureInteract)
        {
            ResetProgress(creatureInteract);
            IsInteracting = true;
            LastWhoInteracted = creatureInteract;
            InteractingCoroutine = StartCoroutine(InteractionProgressing(creatureInteract));
        }
        public override void EndInteraction(EntityInteract creatureInteract)
        {
            base.EndInteraction(creatureInteract);
            ResetProgress(creatureInteract);
        }
        public virtual void ResetProgress(EntityInteract creatureInteract)
        {
            InteractionProgress = 0;
            Image InteractingImage = creatureInteract.GetInteractionProgressImage();
            if (InteractingImage)
                InteractingImage.fillAmount = 0;
            if (InteractingCoroutine != null)
                StopCoroutine(InteractingCoroutine);
            InteractingCoroutine = null;
            IsInteracting = false;
        }
        protected virtual IEnumerator InteractionProgressing(EntityInteract creatureInteract)
        {
            Image InteractingImage = creatureInteract.GetInteractionProgressImage();
            if (InteractingImage)
                InteractingImage.fillAmount = 0;
            while (InteractionProgress < 1f)
            {
                InteractionProgress += InteractingSpeed * Time.deltaTime;
                if (InteractingImage)
                    InteractingImage.fillAmount = InteractionProgress;
                yield return new WaitForSeconds(CoroutineDelay);
            }
            if (useEvents)
                GetComponent<InteractionEvent>().OnInteract.Invoke();
            Interact();
            EndInteraction(creatureInteract);
            InteractingCoroutine = null;
        }
        protected override void Interact()
        {
        }
    }
}