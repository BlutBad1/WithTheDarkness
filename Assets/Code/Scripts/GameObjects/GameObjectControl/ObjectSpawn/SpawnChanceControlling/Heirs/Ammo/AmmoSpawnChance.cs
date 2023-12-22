using ScriptableObjectNS.Weapon.Gun;
using System;
using WeaponManagement;
using WeaponNS;
using WeaponNS.ShootingWeaponNS;

namespace GameObjectsControllingNS
{
    public class AmmoSpawnChance : ObjectSpawnChance<AmmoSpawnChance, AmmoSupply>
    {
        public WeaponEntity WeaponType;
        WeaponManager weaponManager;
        protected override void Start()
        {
            weaponManager = FindAnyObjectByType<WeaponManager>();
            WeaponManagement.Weapon weapon = Array.Find(weaponManager.Weapons, x => x.WeaponData.WeaponEntity == WeaponType && weaponManager.IsActiveWeapon(x.WeaponData));
            if (Chance < 100 && Chance > 0)
            {
                if (weapon != null)
                {
                    GunData gunData = (GunData)weapon.WeaponData;
                    float chanceToAddByAmmoLeft = (gunData.CurrentAmmo + gunData.ReserveAmmoData.ReserveAmmo) < gunData.MagSize ?
                        (gunData.MagSize - (gunData.CurrentAmmo + gunData.ReserveAmmoData.ReserveAmmo)) * 10
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
