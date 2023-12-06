using ScriptableObjectNS.Weapon.Gun;
using System;
using WeaponManagement;
using WeaponNS;
using WeaponNS.ShootingWeaponNS;

namespace GameObjectsControllingNS
{
    public class AmmoSpawnChance : ObjectSpawnChance<AmmoSpawnChance, AmmoSupply>
    {
        public WeaponType WeaponType;
        WeaponManager weaponManager;
        protected override void Start()
        {
            weaponManager = FindAnyObjectByType<WeaponManager>();
            Weapon weapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponType == WeaponType && x.WeaponData.IsUnlocked);
            if (Chance < 100 && Chance > 0)
            {
                if (weapon != null)
                {
                    GunData gunData = (GunData)weapon.WeaponData;
                    float chanceToAddByAmmoLeft = (gunData.CurrentAmmo + gunData.ReserveAmmo) < gunData.MagSize ?
                        (gunData.MagSize - (gunData.CurrentAmmo + gunData.ReserveAmmo)) * 10
                        : 15;
                    Chance += chanceToAddByAmmoLeft;
                }
                else
                    Chance *= 0.5f;
            }
            base.Start();
        }
    }
}
