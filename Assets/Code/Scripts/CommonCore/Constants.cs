namespace MyConstants
{
    public class Constants { }
    public static class CommonConstants
    {
        public const string PLAYER = "Player";
        public const string EVENT_SYSTEM = "EventSystem";
        public const string MAIN_CAMERA_PATH = "Player/MainCamera";
        public const string IMPORTANT_SOUNDS = "ImportantSounds";
        public const string POOLABLE_OBJECTS = "PoolableObjects";
        public const string WEAPON_HOLDER = "WeaponHolder";
        public const string PIVOT_MAIN_LIGHT = "PivotSpotLight(MainLight)";
        public const string MAIN_LIGHT = "SpotLight(MainLight)";
    }
    public static class DataSavingConstants
    {
        public const string ENCRYPTION_CODE_WORD = "dark";
        public const string SETTINGS_DATA_PATH = "settings";
        public const string PROGRESS_DATA_PATH = "save";
    }
    public static class UIConstants
    {
        public const string SETTINGS_MENU = "SettingsMenu";
        public const string UI_SOUNDS = "UISounds";
        public class UISoundsNameConstants
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
        public const string BLACK_SCREEN_DIMMING = "BlackScreenDimming";
        public const string LIGHT_INTERES_LEFT = "LightInterestLeft";
        public const string AMMO_LEFT = "AmmoLeft";
    }
    public static class LocationsConstants
    {
        public const string MAPS = "Maps";
        public const string ENTRY_TO_LOCATION = "Triggers/EntryToLocation";
    }
    public static class EnironmentConstants
    {
        public static class Lock
        {
            public const string LOCK_ANIMATOR_TRIGGER = "Open";
            public const string LOCK_ANIMATOR_BOOL = "IsLocked";
        }
        public static class Doors
        {
            public const string DOORS_ANIMATOR_IS_OPENED = "IsOpened";
            public const string DOORS_ANIMATOR_IS_LOCKED = "IsLocked";
            public const string DOORS_ANIMATOR_TRIGGER = "PushDoors";
        }
    }
    public class EnemyConstants
    {
        public const string IS_WALKING = "IsWalking";
        public const string JUMP = "Jump";
        public const string LANDED = "Landed";
        public const string ATTACK_TRIGGER = "Attack";
        public const string DEATH_TRIGGER = "Death";
    }
    public class MainAudioManagerConstants
    {
        public const string TRANSITION = "transitionSound";
    }
    public class SceneConstants
    {
        public enum AvailableScenes
        {
            MAIN_MENU = 0, LOADING = 1, House_Breakpoint = 2, LEVEL1 = 3, TEST2, GAMEPLAY
        }
        public const string PROGRESS_MANAGER = "ProgressManager";
        public const string LOADING = "Loading";
        public const string GAMEPLAY = "Gameplay";
        public const string MAIN_MENU = "MainMenu";
    }
    namespace WeaponConstants
    {
        public class LampConstants
        {
            public const string LAMP = "LeftHand";
            public const string SPEED = "speed";
            public const string IS_LIGHT_UP = "IsLightUp";
            public const string LIGHT_UP = "LightUp";
        }
        public class WeaponConstants
        {
            public const string PUTTING_DOWN = "PuttingDown";
            public const string PICKING_UP = "PickingUp";
        }

        namespace ShootingWeaponConstants
        {
            public class MainShootingWeaponConstants
            {
                public const string RELOADING = "Reloading";
                public const string FIRING = "Firing";
                public const string ALT_FIRING = "AltFiring";
                public const string OUT_OF_AMMO = "OutOfAmmo";
                public const string DIFFERENCE = "Difference";
                public const string IDLE = "Idle";
                public const string BULLET_HOLES_DATA_BASE = "MainBulletHolesDataBase";
                public const string DEFAULT_BULLET_HOLE = "DefaultBulletHole";
            }
            public class RevolverConstants
            {
                public const string RELOADING_DELAY = "RelodingDelay";
                public const string RELOADING_ANIMATION_SPEED = "ReloadingSpeed";
                public const string RELOADING_ENDING = "ReloadingEnding";
                public const string REVOLVER_RELOADING_SOUND = "RevolverReloading";
                public const string REVOLVER_RELOADING_CYLINDER_SOUND = "RevolverCylinder";
            }
        }
    }
}
