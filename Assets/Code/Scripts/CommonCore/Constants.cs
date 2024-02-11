namespace MyConstants
{
	public static class Constants { }
	public static class CommonConstants
	{
		public const string PLAYER = "Player";
		public const string EVENT_SYSTEM = "EventSystem";
		public const string MAIN_CAMERA_PATH = "Player/MainCamera";
		public const string IMPORTANT_SOUNDS = "ImportantSounds";
		public const string POOLABLE_OBJECTS = "Data/PoolableObjects";
		public const string FOOTSTEPS_DATA_BASE = "Data/footsteps_database";
		public const string WEAPON_HOLDER = "WeaponHolder";
		public const string PIVOT_MAIN_LIGHT = "PivotSpotLight(MainLight)";
		public const string MAIN_LIGHT = "SpotLight(MainLight)";
	}
	public static class DataConstants
	{
		public const string ENCRYPTION_CODE_WORD = "dark";
		public const string SETTINGS_DATA_PATH = "settings";
		public const string PROGRESS_DATA_PATH = "save";
		public const string CREATURE_TYPES_INSTANCE = "Creature Types";
	}
	namespace UIConstants
	{
		public static class MainUIConstants
		{
			public const string SETTINGS_MENU = "SettingsMenu";
			public const string UI_SOUNDS = "UISounds";
		}
		public static class UISoundsNameConstants
		{
			public const string BUTTON_CLICK_SOUND = "button_click_regular";
			public const string BUTTON_HOVER_SOUND = "button_hover_regular";
			public const string BUTTON_CLICK_SOUND_HIGHER = "button_click_higher";
			public const string BUTTON_HOVER_SOUND_HIGHER = "button_hover_higher";
			public const string SLIDER_CHANGE = "slider_change";
		}
	}
	public static class HUDConstants
	{
		public const string TEXTSHOWER = "InfoText";
		public const string INTERACTABLE_TEXT = "InteractableText";
		public const string INTERACTING_PROGRESS_IMAGE = "InteractingProgress";
		public const string BLACK_SCREEN_DIMMING = "BlackScreenDimming";
		public const string LIGHT_INTERES_LEFT = "LightInterestLeft";
		public const string AMMO_LEFT = "AmmoLeft";
		public static string[] AMMO_GET_MESSAGES = { "Found ", "Got " };
	}
	public static class LocationsConstants
	{
		public const string MAPS = "Maps";
		public const string ENTRY_TO_LOCATION = "Triggers/EntryToLocation";
	}
	namespace EnironmentConstants
	{
		namespace SpawnerConstants
		{
			public static class MainSpawnerConstants
			{
			}
			public static class KeySpawnerConstants
			{
				public const float REQUIRED_KEY_SPAWN_CHANCE_DIV_COEFF_ON_FIRST_HALF = 2;
			}
		}
		namespace ItemConstants
		{
			public static class PropConstants
			{
				public const string NEW_PROP_ROOT = "NewAddedInGameTimeProps";
			}
		}
		namespace DecorConstants
		{
			public static class LockConstants
			{
				public const string LOCK_ANIMATOR_TRIGGER = "Open";
				public const string LOCK_ANIMATOR_BOOL = "IsLocked";
			}
			public static class DoorsConstants
			{
				public const string DOORS_ANIMATOR_IS_OPENED = "IsOpened";
				public const string DOORS_ANIMATOR_IS_LOCKED = "IsLocked";
				public const string DOORS_ANIMATOR_TRIGGER = "PushDoors";
			}
		}
	}
	namespace CreatureConstants
	{
		public static class MainCreatureConstants
		{
			public const string ALONE_CREATURE_TYPE = "Alone";
		}
		namespace EnemyConstants
		{
			public static class MainEnemyConstants
			{
				public const string IS_WALKING = "IsWalking";
				public const string JUMP = "Jump";
				public const string LANDED = "Landed";
				public const string ATTACK_TRIGGER = "Attack";
				public const string DEATH_TRIGGER = "Death";
			}
			public static class GramophoneConstants
			{
				public const string PLAY_TRIGGER = "Play";
				public const string STOP_PLAYING_TRIGGER = "StopPlaying";
			}
		}
	}
	public static class SceneConstants
	{
		public const string PROGRESS_MANAGER = "ProgressManager";
		public const string LOADING = "Loading";
		public const string GAMEPLAY = "Gameplay";
		public const string MAIN_MENU = "MainMenu";
		public const string LEVEL1 = "Level1";
	}
}
