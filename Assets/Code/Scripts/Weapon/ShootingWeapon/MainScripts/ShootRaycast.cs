using DamageableNS;
using MyConstants.WeaponConstants;
using ScriptableObjectNS.Weapon.Gun;
using UnityEngine;
namespace WeaponNS.ShootingWeaponNS
{
    [RequireComponent(typeof(ShootingWeapon))]
    public class ShootRaycast : MonoBehaviour
    {
        [SerializeField, Tooltip("if not set, will be the main camera")]
        public Camera CameraOrigin;
        [SerializeField]
        public LayerMask WhatIsRayCastIgnore;
        [Tooltip("if not set, will be the main dataBase")]
        public DamageDecalDataBase bulletHolesDataBase;
        [Tooltip("Spread coeff")]
        float maxDeviation;
        protected virtual void Start()
        {
            if (!CameraOrigin)
                CameraOrigin = Camera.main;
            GetComponent<ShootingWeapon>().OnShootRaycast = null;
            GetComponent<ShootingWeapon>().OnShootRaycast += OnShootRaycast;
            maxDeviation = GetComponent<ShootingWeapon>().gunData.MaxDeviation;
            if (!bulletHolesDataBase)
                bulletHolesDataBase = GameObject.Find(MainWeaponConstants.DAMAGE_DECALS_DATA_BASE).GetComponent<DamageDecalDataBase>();
        }
        public virtual void OnShootRaycast(GunData gunData)
        {
            Vector3 forwardVector = Vector3.forward;
            float deviation = UnityEngine.Random.Range(0f, maxDeviation);
            float angle = UnityEngine.Random.Range(0f, 360f);
            forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
            forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
            forwardVector = CameraOrigin.transform.rotation * forwardVector;
            if (Physics.Raycast(CameraOrigin.transform.position, forwardVector, out RaycastHit hitInfo, gunData.MaxDistance, ~WhatIsRayCastIgnore))
            {
                IDamageable damageable = IDamageable.GetDamageableFromGameObject(hitInfo.transform.gameObject);
                damageable?.TakeDamage(new TakeDamageData(damageable, gunData.Damage, gunData.Force, hitInfo.point, GameObject.Find(MyConstants.CommonConstants.PLAYER)));
                bulletHolesDataBase.MakeBulletHoleByInfo(hitInfo, CameraOrigin.transform.position, gunData.WeaponType);
            }
        }
    }
}
