using UnityEngine.Events;

namespace InteractableNS.Pickups
{
    public class MeleeWeaponPickups : WeaponPickups
    {
        public UnityEvent IfWeaponNotUnlocked;
        public UnityEvent IfWeaponAlreadyUnlocked;
        protected override void Start()
        {
            base.Start();
            ActionIfWeaponNotUnlocked += OnNotUnlocked;
            ActionIfWeaponUnlocked += OnAlreadyUnlocked;
        }
        protected void OnNotUnlocked()
        {
            IfWeaponNotUnlocked?.Invoke();
        }
        protected void OnAlreadyUnlocked()
        {
            IfWeaponAlreadyUnlocked?.Invoke();
        }
    }
}