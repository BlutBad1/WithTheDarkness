using DamageableNS;
using MyConstants.WeaponConstants;
using MyConstants.WeaponConstants.MeleeWeaponConstants;
using ScriptableObjectNS.Weapon;
using System;
using UnityEngine;
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
            if (!bulletHolesDataBase)
                bulletHolesDataBase = GameObject.Find(MainWeaponConstants.DAMAGE_DECALS_DATA_BASE).GetComponent<DamageDecalDataBase>();
            if (!animator)
                animator = GetComponent<Animator>();
            if (!cameraOrigin)
                cameraOrigin = Camera.main;
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
                  animator.GetNextAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) ||
                 animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) ||
                  animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PICKING_UP)) return false;
            return true;
        }
        public virtual void Attack()
        {
            if (!CanAttack()) return;
            attacking = true;
            Invoke(nameof(ResetAttack), meleeData.AttackTime);
        }
        public virtual bool CanAttack()
        {
            if (attacking) return false;
            if (animator.GetNextAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) ||
                animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) ||
                animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PICKING_UP)) return false;
            return true;
        }
        public virtual void AttackRaycast()
        {
            if (Physics.SphereCast(cameraOrigin.transform.position, meleeData.AttackRadius, cameraOrigin.transform.forward, out RaycastHit hitInfo, meleeData.AttackDistance, ~whatIsRayCastIgnore))
            {
                IDamageable damageable = UtilitiesNS.Utilities.GetComponentFromGameObject<IDamageable>(hitInfo.transform.gameObject);
                if (damageable != null && !damageable.Equals(null))
                {
                    damageable.TakeDamage(new TakeDamageData(damageable, meleeData.Damage * damageAndForceMultiplier, meleeData.Force * damageAndForceMultiplier, new HitData(hitInfo), GameObject.Find(MyConstants.CommonConstants.PLAYER)));
                    DecreaseDurability();
                }
                OnHit?.Invoke(hitInfo);
                //if (Physics.Raycast(CameraOrigin.transform.position, CameraOrigin.transform.forward, out RaycastHit hitInfo2, MeleeData.AttackDistance + 2 * MeleeData.AttackRadius, ~WhatIsRayCastIgnore))
                //    OnHit?.Invoke(hitInfo2);
            }
        }
        public virtual void DecreaseDurability()
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
