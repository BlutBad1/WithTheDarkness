namespace MyConstants
{
    public class Constants { }
    public static class CommonConstants
    {
        public const string PLAYER = "Player";
        public const string MAIN_CAMERA_PATH = "Player/MainCamera";
        public const string MAIN_AUDIOMANAGER = "MainAudioManager";
        public const string POOLABLE_OBJECTS = "PoolableObjects";
        public const string WEAPON_HOLDER = "WeaponHolder";
    }
    public static class HUDConstants
    {
        public const string TEXTSHOWER = "InfoText";
        public const string BLACK_SCREEN_DIMMING = "BlackScreenDimming";
        public const string LIGHT_INTERES_LEFT = "LightInterestLeft";
    }
    public static class LocationsConstants
    {
        public const string MAPS = "Maps";
        public const string ENTRY_TO_LOCATION = "Triggers/EntryToLocation";
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
            MAIN_MENU = 0, LEVEL1, LOADING, TEST2, GAMEPLAY
        }
        public const string SCENE_MANAGER = "SceneManager";
        public const string LOADING = "Loading";
        public const string GAMEPLAY = "Gameplay";
        public const string MAIN_MENU = "MainMenu";
    }
    namespace WeaponConstants
    {
        public class WeaponConstants
        {
            public const string LAMP = "LeftHand";
            public const string PUTTING_DOWN = "PuttingDown";
        }
    }
    namespace ShootingWeaponConstants
    {
        public class MainShootingWeaponConstants
        {
            public const string RELOADING = "Reloading";
            public const string FIRING = "Firing";
            public const string OUT_OF_AMMO = "OutOfAmmo";
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
