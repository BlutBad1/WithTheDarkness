using WeaponManagement;

namespace InteractableNS
{
    public class LampPickup : Interactable
    {
        protected override void Interact()
        {
            WeaponManager weaponManager = UtilitiesNS.Utilities.GetComponentFromGameObject<WeaponManager>(lastWhoInteracted.gameObject);
            if (weaponManager)
            {
                weaponManager.Lamp.SetActive(true);
                weaponManager.MainSpotLight.SetActive(true);
            }
        }
    }
}
