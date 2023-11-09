using LightNS.Player;
using System.Collections;
using UnityEngine;
using WeaponManagement;

namespace InteractableNS
{
    public class LampPlace : ProgressingInteractable
    {
        public GameObject PickupableLamp;
        public bool IsLampPlaced = false;
        public LayerMask TriggerableLayers;
        public bool IsProgressInteraction = false;
        private new void Start()
        {
            base.Start();
            PickupableLamp.SetActive(IsLampPlaced);
        }
        protected override void Interact()
        {
            if (!IsLampPlaced)
            {
                WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
                if (weaponManager)
                {
                    weaponManager.Lamp.SetActive(false);
                    weaponManager.MainSpotLight.SetActive(false);
                }
                PickupableLamp.SetActive(true);
                IsLampPlaced = true;
            }
            else
            {
                WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
                if (weaponManager && !weaponManager.Lamp.activeInHierarchy)
                {
                    weaponManager.Lamp.SetActive(true);
                    weaponManager.MainSpotLight.SetActive(true);
                    PickupableLamp.SetActive(false);
                    IsLampPlaced = false;
                }
            }
        }
        protected override IEnumerator InteractionProgressing(EntityInteract creatureInteract)
        {
            if (IsProgressInteraction)
                yield return base.InteractionProgressing(creatureInteract);
            else
            {
                if (useEvents)
                    GetComponent<InteractionEvent>().OnInteract.Invoke();
                Interact();
                yield return null;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (IsLampPlaced && TriggerableLayers == (TriggerableLayers | (1 << other.gameObject.layer)))
            {
                PlayerLightExhaustion playerLightExhaustion = UtilitiesNS.Utilities.GetComponentFromGameObject<PlayerLightExhaustion>(other.gameObject);
                if (playerLightExhaustion && playerLightExhaustion.LightGlowTimer?.CurrentTimeLeftToGlow > 0)
                    playerLightExhaustion.IsExhastionEnabled = false;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (IsLampPlaced && TriggerableLayers == (TriggerableLayers | (1 << other.gameObject.layer)))
            {
                PlayerLightExhaustion playerLightExhaustion = UtilitiesNS.Utilities.GetComponentFromGameObject<PlayerLightExhaustion>(other.gameObject);
                WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(other.gameObject);
                if (playerLightExhaustion && !weaponManager.Lamp.activeInHierarchy)
                    playerLightExhaustion.IsExhastionEnabled = true;
            }
        }
    }
}