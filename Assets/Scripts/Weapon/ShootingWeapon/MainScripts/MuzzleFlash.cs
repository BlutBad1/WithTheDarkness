using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeaponNS.ShootingWeaponNS
{
    public class MuzzleFlash : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem muzzleFlash;
        [SerializeField]
        private Light muzzleLight;
        [SerializeField]
        private float timer;
        private float MuzzleTimer;

        public void MuzzleFlashEnable()
        {
            MuzzleTimer = 0;
            muzzleLight.enabled = true;
            muzzleFlash.Play();
            StartCoroutine(MuzzleLight());


        }
        IEnumerator MuzzleLight()
        {
            while (MuzzleTimer < timer)
            {
                yield return null;
                MuzzleTimer += Time.deltaTime;
            }
            muzzleLight.enabled = false;


        }

    }
}