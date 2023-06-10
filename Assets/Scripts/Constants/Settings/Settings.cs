namespace SettingsNS
{
    public class Settings {}
    public class InteracteBindingSettings
    {
        public delegate void InteracteBindingChangeEvent();
        static public InteracteBindingChangeEvent BindInteracteChangeEvent;
    }
    public class ChangeWeapoAfterPickupSettings
    {
        static public bool ChangeWeaponAfterPickup = true;
        public delegate void ChangeWeaponAfterPickupStatusChangeEvent();
        static public ChangeWeaponAfterPickupStatusChangeEvent WeaponChangeAfterPickupStatusChangeEvent;
    }
}