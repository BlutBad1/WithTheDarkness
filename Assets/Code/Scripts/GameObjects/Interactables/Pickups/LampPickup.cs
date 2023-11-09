using WeaponManagement;

namespace InteractableNS
{
    public class LampPickup : Interactable
    {
        protected override void Interact()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(LastWhoInteracted.gameObject);
            if (weaponManager)
            {
                weaponManager.Lamp.SetActive(true);
                weaponManager.MainSpotLight.SetActive(true);
            }
        }
    }
}
