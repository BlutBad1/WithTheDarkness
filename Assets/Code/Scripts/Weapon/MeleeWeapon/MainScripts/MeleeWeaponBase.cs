using DamageableNS;
using PlayerScriptsNS;
using ScriptableObjectNS.Weapon;
using System;
using UnityEngine;
using WeaponConstantsNS;
using WeaponConstantsNS.MeleeWeaponConstantsNS;
using WeaponNS.ShootingWeaponNS;

namespace WeaponNS.MeleeWeaponNS
{
	public class MeleeWeaponBase : MonoBehaviour
	{
		[SerializeField]
		protected MeleeData meleeData;
		[SerializeField]
		protected Animator animator;
		[SerializeField, Tooltip("if not set, will be the main camera")]
		protected Camera cameraOrigin;
		[SerializeField]
		protected LayerMask whatIsRayCastIgnore;
		[SerializeField, Tooltip("if not set, will be the main dataBase")]
		protected DamageDecalDataBase bulletHolesDataBase;
		[SerializeField]
		protected DurabilityChange weaponDurabilityEnd;

		protected GameObject player;
		protected float damageAndForceMultiplier = 1f;
		protected event Action<RaycastHit> OnHit;
		protected bool attacking = false;
		protected string currentAnimationState;
		private static bool hasBeenInitialized = false;

		private void OnEnable()
		{
			if (hasBeenInitialized)
				AttachActions();
			attacking = false;
		}
		private void Start()
		{
			player = UtilitiesNS.Utilities.GetComponentFromGameObject<PlayerCreature>(gameObject).gameObject;
			if (!hasBeenInitialized)
				AttachActions();
			hasBeenInitialized = true;
		}
		private void OnDisable() =>
			DettachActions();
		private void Update()
		{
			DefineAnim();
		}
		public virtual void AttachActions()
		{
			OnHit += HitTarget;
			PlayerBattleInput.AttackInputStarted += Attack;
		}
		public virtual void DettachActions()
		{
			OnHit -= HitTarget;
			PlayerBattleInput.AttackInputStarted -= Attack;
		}
		public virtual void DefineAnim()
		{
			string nextAnim = string.Empty;
			float tranTime = meleeData.AnimTransitionTime;
			if (attacking)
				nextAnim = MainMeleeWeaponConstants.ATTACK;
			else
				nextAnim = MainMeleeWeaponConstants.IDLE;
			if (!CheckAnimConditions(nextAnim)) return;
			animator.CrossFadeInFixedTime(nextAnim, tranTime);
			currentAnimationState = nextAnim;
		}
		public virtual bool CheckAnimConditions(string nextAnim)
		{
			if (currentAnimationState == nextAnim ||
				  animator.GetNextAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) ||
				 animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) ||
				  animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PICKING_UP)) return false;
			return true;
		}
		public virtual void Attack()
		{
			if (!CanAttack()) return;
			attacking = true;
			Invoke(nameof(ResetAttack), meleeData.AttackTime);
		}
		protected virtual bool CanAttack()
		{
			if (attacking) return false;
			if (animator.GetNextAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) ||
				animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) ||
				animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PICKING_UP)) return false;
			return true;
		}
		public virtual void AttackRaycast()
		{
			if (Physics.SphereCast(cameraOrigin.transform.position, meleeData.AttackRadius, cameraOrigin.transform.forward, out RaycastHit hitInfo, meleeData.AttackDistance, ~whatIsRayCastIgnore))
			{
				IDamageable damageable = UtilitiesNS.Utilities.GetComponentFromGameObject<IDamageable>(hitInfo.transform.gameObject);
				if (damageable != null && !damageable.Equals(null))
				{
					damageable.TakeDamage(new TakeDamageData(damageable, meleeData.Damage * damageAndForceMultiplier,
						meleeData.Force * damageAndForceMultiplier, new HitData(hitInfo), player));
					DecreaseDurability();
				}
				OnHit?.Invoke(hitInfo);
			}
		}
		protected virtual void DecreaseDurability()
		{
			meleeData.CurrentDurability -= meleeData.MoveDurabilityCost;
			meleeData.CurrentDurability = meleeData.CurrentDurability <= 0 ? 0 : meleeData.CurrentDurability;
			weaponDurabilityEnd.OnDurabilityDecrease();
			if (meleeData.CurrentDurability <= 0)
				weaponDurabilityEnd.OnDurabilityEnd();
		}
		protected virtual void ResetAttack() =>
			attacking = false;
		protected virtual void HitTarget(RaycastHit hitInfo) =>
			bulletHolesDataBase.MakeBulletHoleByInfo(hitInfo, cameraOrigin.transform.position, meleeData.WeaponEntity);
	}
}
