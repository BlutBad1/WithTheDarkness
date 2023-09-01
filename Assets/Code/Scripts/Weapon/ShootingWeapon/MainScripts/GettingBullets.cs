using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeaponNS.ShootingWeaponNS
{
    public class GettingBullets : MonoBehaviour
    {

        public GunData revolver;

        public void AddBullets(int amount)
        {
            revolver.ReserveAmmo += amount;


        }

    }
}