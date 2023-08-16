using System.Collections;
using UnityEngine;

namespace WeaponNS.WeaponEffectsNS
{
    public class MuzzleFlash : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem muzzleFlash;
        [SerializeField]
        private Light[] muzzleLight;
        [SerializeField]
        private float timer;
        private float MuzzleTimer;
        public void MuzzleFlashEnable() =>
            StartCoroutine(MuzzleLight());
        IEnumerator MuzzleLight()
        {
            MuzzleTimer = 0;
            muzzleFlash.Play();
            for (int i = 0; i < muzzleLight.Length; i++)
                muzzleLight[i].enabled = true;
            while (MuzzleTimer < timer)
            {
                yield return null;
                MuzzleTimer += Time.deltaTime;
            }
            for (int i = 0; i < muzzleLight.Length; i++)
                muzzleLight[i].enabled = false;
        }
    }
}