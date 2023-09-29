using UnityEngine;
using UnityEngine.UI;

namespace InteractableNS
{
    public class EntityInteract : MonoBehaviour
    {
        [SerializeField]
        protected Image ProgressImage;
        public Image GetInteractionProgressImage() =>
             ProgressImage;
    }
}

