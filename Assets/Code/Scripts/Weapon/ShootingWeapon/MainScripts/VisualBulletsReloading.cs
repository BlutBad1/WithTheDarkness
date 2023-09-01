using UnityEngine;


namespace WeaponNS.ShootingWeaponNS
{
    public class VisualBulletsReloading : MonoBehaviour
    {
        [SerializeField]
        private GunData gunData;
        [SerializeField]
        private GameObject[] bullets;
        public void BulletsReloadingVisual()
        {
            foreach (var bullet in bullets)
                bullet.SetActive(false);
            for (int i = 0; i < gunData.CurrentAmmo; i++)
                bullets[i].SetActive(true);
        }
    }
}