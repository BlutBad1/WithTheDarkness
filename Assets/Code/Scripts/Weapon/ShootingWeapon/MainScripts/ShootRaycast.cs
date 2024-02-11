using DamageableNS;
using ScriptableObjectNS.Weapon.Gun;
using UnityEngine;
using WeaponConstantsNS;

namespace WeaponNS.ShootingWeaponNS
{
	[RequireComponent(typeof(ShootingWeapon))]
	public class ShootRaycast : MonoBehaviour
	{
		[SerializeField, Tooltip("if not set, will be the main camera")]
		private Camera cameraOrigin;
		[SerializeField]
		private LayerMask whatIsRayCastIgnore;
		[SerializeField, Tooltip("if not set, will be the main dataBase")]
		private DamageDecalDataBase bulletHolesDataBase;

		private float maxDeviation;

		protected virtual void Start()
		{
			if (!cameraOrigin)
				cameraOrigin = Camera.main;
			GetComponent<ShootingWeapon>().OnShootRaycast = null;
			GetComponent<ShootingWeapon>().OnShootRaycast += OnShootRaycast;
			maxDeviation = GetComponent<ShootingWeapon>().GunData.MaxDeviation;
			if (!bulletHolesDataBase)
				bulletHolesDataBase = GameObject.Find(WeaponConstants.DAMAGE_DECALS_DATA_BASE).GetComponent<DamageDecalDataBase>();
		}
		public virtual void OnShootRaycast(GunData gunData)
		{
			Vector3 forwardVector = Vector3.forward;
			float deviation = UnityEngine.Random.Range(0f, maxDeviation);
			float angle = UnityEngine.Random.Range(0f, 360f);
			forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
			forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
			forwardVector = cameraOrigin.transform.rotation * forwardVector;
			if (Physics.Raycast(cameraOrigin.transform.position, forwardVector, out RaycastHit hitInfo, gunData.MaxDistance, ~whatIsRayCastIgnore))
				OnRaycastHit(hitInfo, gunData);
		}
		protected virtual void OnRaycastHit(RaycastHit hitInfo, GunData gunData)
		{
			IDamageable damageable = UtilitiesNS.Utilities.GetComponentFromGameObject<IDamageable>(hitInfo.transform.gameObject);
			damageable?.TakeDamage(new TakeDamageData(damageable, gunData.Damage, gunData.Force, new HitData(hitInfo), gameObject));
			bulletHolesDataBase?.MakeBulletHoleByInfo(hitInfo, cameraOrigin.transform.position, gunData.WeaponEntity);
		}
	}
}
