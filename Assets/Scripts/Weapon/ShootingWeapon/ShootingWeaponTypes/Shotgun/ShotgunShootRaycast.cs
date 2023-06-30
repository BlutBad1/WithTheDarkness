using UnityEngine;

namespace WeaponNS.ShootingWeaponNS.ShotgunNS
{
    [RequireComponent(typeof(ShootingWeapon))]
    public class ShotgunShootRaycast : ShootRaycast
    {
        public override void OnShootRaycast(GunData gunData)
        {
            for (int i = 0; i < 4; i++)
                base.OnShootRaycast(gunData);
        }
    }
}