using ScriptableObjectNS.Locking;
using UnityEngine;
using UnityEngine.Serialization;

namespace InteractableNS.Usable.Locking
{
    public class KeyInteractable : Interactable
    {
        [SerializeField]
        private KeyData key;
        [SerializeField, HideInInspector, FormerlySerializedAs("AvailableKeyData")]
        public AvailableKeysData availableKeyData;

        public KeyData Key { get => key; }

        protected override void Interact()
        {
            KeysOnLevelManager.Instance.AddKeyToAvailable(key);
        }
    }
}
