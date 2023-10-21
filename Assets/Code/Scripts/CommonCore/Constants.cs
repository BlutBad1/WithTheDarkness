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
        public static string[] AMMO_GET_MESSAGES =  { "Found ", "Got "};
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
                public const int MAX_AMOUNT_OF_CYCLES = 5;
            }
            public static class KeySpawnerConstants
            {
                public const float REQUIRED_KEY_SPAWN_CHANCE_COEFF_ON_SECOND_HALF = 2;
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
    public static class MainAudioManagerConstants
    {
        public const string TRANSITION = "transitionSound";
    }
    public static class SceneConstants
    {
        public const string PROGRESS_MANAGER = "ProgressManager";
        public const string LOADING = "Loading";
        public const string GAMEPLAY = "Gameplay";
        public const string MAIN_MENU = "MainMenu";
        public const string LEVEL1 = "Level1";
    }
    namespace WeaponConstants
    {
        public static class MainWeaponConstants
        {
            public const string PUTTING_DOWN = "PuttingDown";
            public const string PICKING_UP = "PickingUp";
            public const string DAMAGE_DECALS_DATA_BASE = "Data/MainDamageDecalsDataBase";
            public const string DEFAULT_DAMAGE_DECAL = "DefaultDamageDecal";
        }
        public static class LampConstants
        {
            public const string LAMP = "LeftHand";
            public const string SPEED = "speed";
            public const string IS_LIGHT_UP = "IsLightUp";
            public const string LIGHT_UP = "LightUp";
        }
        namespace ShootingWeaponConstants
        {
            public static class MainShootingWeaponConstants
            {
                public const string RELOADING = "Reloading";
                public const string FIRING = "Firing";
                public const string ALT_FIRING = "AltFiring";
                public const string OUT_OF_AMMO = "OutOfAmmo";
                public const string DIFFERENCE = "Difference";
                public const string IDLE = "Idle";
            }
            public static class RevolverConstants
            {
                public const string RELOADING_DELAY = "RelodingDelay";
                public const string RELOADING_ANIMATION_SPEED = "ReloadingSpeed";
                public const string RELOADING_ENDING = "ReloadingEnding";
                public const string REVOLVER_RELOADING_SOUND = "RevolverReloading";
                public const string REVOLVER_RELOADING_CYLINDER_SOUND = "RevolverCylinder";
            }
        }
        namespace MeleeWeaponConstants
        {
            public static class MainMeleeWeaponConstants
            {
                public const string ATTACK = "Attacking";
                public const string IDLE = "Idle";
            }
            public static class AxeConstants
            {
                public const string ATTACK2 = "Attacking2";
                public const string BLOCKING = "Blocking";
                public const string HIT_SOUND = "HitSound";
                public const string ATTACK_PREPARING = "AttackPreparing";
                public const string ATTACK_PERFORMING = "AttackPerforming";
            }
        }
    }
}
