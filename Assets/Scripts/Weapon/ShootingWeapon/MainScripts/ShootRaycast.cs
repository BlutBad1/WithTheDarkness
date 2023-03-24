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
        void Start()
        {
            GetComponent<ShootingWeapon>().OnBulletSpread += OnBulletSpread;
        }
        public virtual void OnBulletSpread(GunData gunData)
        {

            Vector3 forwardVector = Vector3.forward;
            float deviation = UnityEngine.Random.Range(0f, maxDeviation);
            float angle = UnityEngine.Random.Range(0f, 360f);
            forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
            forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
            forwardVector = Camera.main.transform.rotation * forwardVector;
            if (Physics.Raycast(Camera.main.transform.position, forwardVector, out RaycastHit hitInfo, gunData.maxDistance, ~(1 << 20 | 1 << 2)))
            {
                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                if (hitInfo.collider.gameObject.layer == 13)
                {
                    damageable?.TakeDamage(gunData.damage, gunData.force, hitInfo.point);
                    CreateBulletHole("EnemyBulletHole", hitInfo);

                }
                else
                {
                    CreateBulletHole("DefaultBulletHole", hitInfo);

                }
            }
        }


    }
}
