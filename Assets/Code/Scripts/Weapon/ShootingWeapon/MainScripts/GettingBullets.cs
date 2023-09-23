using ScriptableObjectNS.Weapon.Gun;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS
{
    public class GettingBullets : MonoBehaviour
    {
        public GunData revolver;
        public void AddBullets(int amount) =>
            revolver.ReserveAmmo += amount;
    }
}