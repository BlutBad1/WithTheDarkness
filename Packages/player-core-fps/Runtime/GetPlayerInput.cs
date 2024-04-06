namespace InputNS
{
	public abstract class GetPlayerInput
	{
		private static PlayerInput playerInputInstance;

		public static PlayerInput GetInput()
		{
			if (playerInputInstance == null)
				playerInputInstance = new PlayerInput();
			return playerInputInstance;
		}
	}
}