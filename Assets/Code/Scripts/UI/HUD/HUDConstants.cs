namespace HUDConstantsNS
{
	public static class HUDConstants
	{
		public const string TEXTSHOWER = "InfoText";
		public const string INTERACTABLE_TEXT = "InteractableText";
		public const string INTERACTING_PROGRESS_IMAGE = "InteractingProgress";
		public const string BLACK_SCREEN_DIMMING = "BlackScreenDimming";
		public const string LIGHT_INTERES_LEFT = "LightInterestLeft";
		public const string AMMO_LEFT = "AmmoLeft";
		public static string[] AMMO_GET_MESSAGES = { "Found ", "Got " };

		public static Printer MessagePrinter = AvailablePrinters.Instance.MessagePrinter;
		public static Printer LightPercentPrinter = AvailablePrinters.Instance.LightPercentPrinter;
		public static Printer AmmoGetMessagePrinter = AvailablePrinters.Instance.AmmoGetMessagePrinter;
		public static AmmoShowPrinter AmmoPrinter = AvailablePrinters.Instance.AmmoShow;
	}
}
