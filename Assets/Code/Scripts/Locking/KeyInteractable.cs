using ScriptableObjectNS.Locking;
using UnityEngine;
using UnityEngine.Serialization;

namespace InteractableNS.Usable.Locking
{
    public class KeyInteractable : Interactable, ILockingInteractable
    {
        [SerializeField]
        private KeyData key;
        [SerializeField, HideInInspector, FormerlySerializedAs("AvailableKeyData")]
        public AvailableKeysData availableKeyData;

        public KeyData GetKeyData() =>
            key;
        public void SetKeyData(KeyData keyData) =>
            key = keyData;
        protected override void Interact()
        {
            KeysOnLevelManager.Instance.AddKeyToAvailable(key);
        }
    }
}
