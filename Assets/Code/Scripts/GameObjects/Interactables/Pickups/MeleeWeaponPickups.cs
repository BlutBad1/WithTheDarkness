using UnityEngine.Events;

namespace InteractableNS.Pickups
{
    public class MeleeWeaponPickups : WeaponPickups
    {
        public UnityEvent IfWeaponNotUnlocked;
        public UnityEvent IfWeaponAlreadyUnlocked;
        public UnityEvent IfWeaponTypeIsOccupiedByOther;
        protected override void Start()
        {
            base.Start();
            ActionIfWeaponTypeIsNotOccupied += OnNotUnlocked;
            ActionIfWeaponTypeIsOccupiedBySame += OnAlreadyUnlocked;
            ActionIfWeaponTypeIsOccupiedByOther += OnOccupiedByOther;
        }
        public void OnNotUnlocked() =>
            IfWeaponNotUnlocked?.Invoke();
        public void OnAlreadyUnlocked() =>
            IfWeaponAlreadyUnlocked?.Invoke();
        public void OnOccupiedByOther() =>
            IfWeaponTypeIsOccupiedByOther?.Invoke();
    }
}