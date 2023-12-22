using UnityEngine;

namespace ScriptableObjectNS.Weapon.Gun.ReserveAmmo
{
    [CreateAssetMenu(fileName = "ReserveAmmoData", menuName = "ScriptableObject/Weapon/ReserveAmmoData")]
    public class ReserveAmmoData : ScriptableObject
    {
        public int ReserveAmmo;
    }
}