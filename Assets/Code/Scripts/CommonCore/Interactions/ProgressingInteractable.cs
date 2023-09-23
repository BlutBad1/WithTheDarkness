using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InteractableNS
{
    public class ProgressingInteractable : Interactable
    {
        public Image InteractingImage;
        public float InteractingSpeed = 1.5f;
        public float CoroutineDelay = 0.01f;
        [HideInInspector]
        public bool IsInteracting = false;
        public Coroutine InteractingCoroutine;
        new protected virtual void Start()
        {
            if (!InteractingImage)
                InteractingImage = GameObject.Find(MyConstants.HUDConstants.INTERACTING_PROGRESS_IMAGE).GetComponent<Image>();
        }
        public sealed override void StartBaseInteraction(EntityInteract creatureInteract)
        {
            IsInteracting = true;
            WhoInteracted = creatureInteract;
            InteractingCoroutine = StartCoroutine(InteractionProgress(creatureInteract));
        }
        public override void EndInteraction(EntityInteract creatureInteract)
        {
            base.EndInteraction(creatureInteract);
            IsInteracting = false;
            if (InteractingCoroutine != null)
                StopCoroutine(InteractingCoroutine);
            InteractingCoroutine = null;
            if (InteractingImage)
                InteractingImage.fillAmount = 0;
        }
        protected virtual IEnumerator InteractionProgress(EntityInteract creatureInteract)
        {
            if (InteractingImage)
                InteractingImage.fillAmount = 0;
            float progress = 0;
            while (progress < 1f)
            {
                progress += InteractingSpeed * Time.deltaTime;
                if (InteractingImage)
                    InteractingImage.fillAmount = progress;
                yield return new WaitForSeconds(CoroutineDelay);
            }
            if (useEvents)
                GetComponent<InteractionEvent>().OnInteract.Invoke();
            Interact();
            EndInteraction(creatureInteract);
        }
    }
}