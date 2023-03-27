using PoolableObjectsNS;
using UnityEngine;
namespace WeaponNS.ShootingWeaponNS
{


    [RequireComponent(typeof(ShootingWeapon))]
    public class ShootRaycast : MonoBehaviour
    {
        [SerializeField]
        protected BulletHolesPool bulletHolesPool;
        [SerializeField]
        float maxDeviation;
        [SerializeField]
        public Camera CameraOrigin;
        [SerializeField]
        public LayerMask WhatIsEnemy;
        [SerializeField]
        public LayerMask WhatIsRayCastIgnore;
        protected virtual void CreateBulletHole(string bulletHoleName, RaycastHit hitInfo)
        {
            GameObject bulletHole = bulletHolesPool.GetObject(bulletHoleName);
            bulletHole.transform.position = hitInfo.point + (hitInfo.normal * 0.0001f);
            bulletHole.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            if (!bulletHole.TryGetComponent(out ParticleSystem particleSystem))
            {
                bulletHole.GetComponentInChildren<ParticleSystem>().Play();
            }
            else
            {
                particleSystem.Play();
            }
        }
        protected virtual void Start()
        {
            if (!CameraOrigin)
            {
                CameraOrigin = Camera.main;
            }
            GetComponent<ShootingWeapon>().OnShootRaycast += OnShootRaycast;
        }
        public virtual void OnShootRaycast(GunData gunData)
        {

            Vector3 forwardVector = Vector3.forward;
            float deviation = UnityEngine.Random.Range(0f, maxDeviation);
            float angle = UnityEngine.Random.Range(0f, 360f);
            forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
            forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
            forwardVector = CameraOrigin.transform.rotation * forwardVector;
          
            if (Physics.Raycast(CameraOrigin.transform.position, forwardVector, out RaycastHit hitInfo, gunData.maxDistance, ~WhatIsRayCastIgnore))
            {
               
                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                if (hitInfo.collider.gameObject.layer == WhatIsEnemy)
                {
                    damageable?.TakeDamage(gunData.damage, gunData.force, hitInfo.point);
                    if (bulletHolesPool)
                        CreateBulletHole("EnemyBulletHole", hitInfo);


               
                }
                else
                {
                    if (bulletHolesPool)
                        CreateBulletHole("DefaultBulletHole", hitInfo);

                }
            }
        }


    }
}
