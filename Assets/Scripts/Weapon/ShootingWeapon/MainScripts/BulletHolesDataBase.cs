using MyConstants;
using MyConstants.ShootingWeaponConstants;
using PoolableObjectsNS;
using System;
using UnityEngine;
using WeaponNS.ShootingWeaponNS.Inspector;

namespace WeaponNS.ShootingWeaponNS
{

    [Serializable]
    public struct BulletObject
    {
        [BulletObjectTagPopupField]
        public string Tag;
        public LayerMask LayerMask;
        [BulletObjectTypePopupField]
        public string BulletHoleType;
    }
    public class BulletHolesDataBase : MonoBehaviour
    {
        public BulletObject[] BulletObjects;
        protected BulletHolesPool bulletHolesPool;
        protected virtual void Start()
        {
            bulletHolesPool = GameObject.FindObjectOfType<BulletHolesPool>();
        }
        public void MakeBulletHoleByLayer(RaycastHit hitInfo)
        {
            BulletObject[] bulletHoles = new BulletObject[0];
            bulletHoles = Array.FindAll(BulletObjects, x => x.Tag == hitInfo.collider.gameObject.tag);          
            bulletHoles = Array.FindAll(bulletHoles.Length == 0 ? BulletObjects : bulletHoles, x => x.LayerMask == (x.LayerMask|(1 << hitInfo.collider.gameObject.layer)));
            if (bulletHoles == null||bulletHoles.Length == 0)
            {
                bulletHoles = new BulletObject[1];
                bulletHoles[0].BulletHoleType = MainShootingWeaponConstants.DEFAULT_BULLET_HOLE;
            }
            int randomNumber = UnityEngine.Random.Range(0, bulletHoles.Length);
            CreateBulletHole(bulletHoles[randomNumber].BulletHoleType, hitInfo);
        }
        protected virtual void CreateBulletHole(string bulletHoleName, RaycastHit hitInfo)
        {
        
            GameObject bulletHole = bulletHolesPool.GetObject(bulletHoleName);
            bulletHole.transform.parent = hitInfo.collider.gameObject.transform;
            bulletHole.transform.position = hitInfo.point + (hitInfo.normal * 0.0001f);
            bulletHole.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            if (!bulletHole.TryGetComponent(out ParticleSystem particleSystem))
            {
                if (bulletHole.GetComponentInChildren<ParticleSystem>())
                    bulletHole.GetComponentInChildren<ParticleSystem>().Play();   
            }
            else
            {
                particleSystem.Play();
            }

        }

    }


}