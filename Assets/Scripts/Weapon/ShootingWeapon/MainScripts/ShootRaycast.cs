using MyConstants.ShootingWeaponConstants;
using UnityEngine;
namespace WeaponNS.ShootingWeaponNS
{


    [RequireComponent(typeof(ShootingWeapon))]
    public class ShootRaycast : MonoBehaviour
    {

        [SerializeField, Tooltip("Spread coeff")]
        float maxDeviation;
        [SerializeField, Tooltip("if not set, will be the main camera")]
        public Camera CameraOrigin;
        [SerializeField]
        public LayerMask WhatIsEnemy;
        [SerializeField]
        public LayerMask WhatIsRayCastIgnore;
        [Tooltip("if not set, will be the main dataBase")]
        public BulletHolesDataBase bulletHolesDataBase;

        protected virtual void Start()
        {
            if (!CameraOrigin)
            {
                CameraOrigin = Camera.main;
            }
            GetComponent<ShootingWeapon>().OnShootRaycast += OnShootRaycast;
            if (!bulletHolesDataBase)
            {
                bulletHolesDataBase = GameObject.Find(MainShootingWeaponConstants.BULLET_HOLES_DATA_BASE).GetComponent<BulletHolesDataBase>();
            }
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

                //беремо з об'єкта по якому попали компонент IDamageable, та визиваємо у нього метод TakeDamage
                hitInfo.transform.TryGetComponent(out IDamageable damageable);
                damageable?.TakeDamage(gunData.damage, gunData.force, hitInfo.point);


                bulletHolesDataBase.MakeBulletHoleByLayer(hitInfo);
            }
        }


    }
}
