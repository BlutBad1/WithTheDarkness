using AYellowpaper;
using HudNS.Weapon.ShootingWeapon;
using InteractableNS;
using UnityEngine;
using WeaponNS;

namespace HUDConstantsNS
{
	public class AvailablePrinters : MonoBehaviour
	{
		public static AvailablePrinters Instance { get; private set; }

		[SerializeField, RequireInterface(typeof(IMessagePrinter))]
		private MonoBehaviour messagePrinter;
		[SerializeField, RequireInterface(typeof(IMessagePrinter))]
		private MonoBehaviour lightPercentPrinter;

		public Printer MessagePrinter;
		public Printer LightPercentPrinter { get; private set; }
		public Printer AmmoGetMessagePrinter { get; private set; }
		public AmmoShowPrinter AmmoShow { get; private set; }

		private void Start()
		{
			if (Instance == null)
				Instance = this;
			else
				Destroy(this);
			MessagePrinter = new Printer() { MessagePrinter = (IMessagePrinter)messagePrinter };
			LightPercentPrinter = new Printer() { MessagePrinter = (IMessagePrinter)lightPercentPrinter };
			AmmoGetMessagePrinter = new Printer() { MessagePrinter = (IMessagePrinter)messagePrinter };
			AmmoShow = new AmmoShowPrinter();
		}
	}
	public class Printer : IMessagePrinter
	{
		public IMessagePrinter MessagePrinter;
		public void PrintMessage(string message, GameObject fromGO)
		{
			MessagePrinter.PrintMessage(message, fromGO);
		}
	}
	public class AmmoShowPrinter : Printer
	{
		public void PrintMessage(WeaponEntity weaponEntity, GameObject fromGameObject)
		{
			Interactable interactable = fromGameObject.GetComponent<Interactable>();
			if (interactable)
			{
				ShowAmmo showAmmo = UtilitiesNS.Utilities.GetComponentFromGameObject<ShowAmmo>(interactable.LastWhoInteracted.gameObject);
				if (showAmmo)
					showAmmo.ShowAmmoLeftOfWeapon(weaponEntity);
			}
		}
	}
}