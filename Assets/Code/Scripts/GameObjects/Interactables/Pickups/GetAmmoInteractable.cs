using HUDConstantsNS;
using InteractableNS.Common;
using ScriptableObjectNS.Weapon.Gun;
using UnityEngine;
using UnityEngine.Serialization;

namespace InteractableNS.Pickups
{
	public class GetAmmoInteractable : InfoTextWriting
	{
		[SerializeField]
		private GunData gun;
		[SerializeField, MinMaxSlider(0, 100), FormerlySerializedAs("AmountOfBullets")]
		private Vector2 amountOfBullets;

		protected override void GetMessagePrinter()
		{
			base.GetMessagePrinter();
			messagePrint = HUDConstants.AmmoGetMessagePrinter;
		}
		protected override void Interact()
		{
			int amountOfBulletsToAdd = (int)Random.Range(amountOfBullets.x, amountOfBullets.y);
			messagePrint.PrintMessage(amountOfBulletsToAdd.ToString(), gameObject);
			base.Interact();
			gun.ReserveAmmoData.ReserveAmmo += amountOfBulletsToAdd;
			HUDConstants.AmmoPrinter.PrintMessage(gun.WeaponEntity, gameObject);
		}
	}
}
