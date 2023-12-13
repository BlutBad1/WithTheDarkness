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
        public MeleeData MeleeData;
        [SerializeField]
        protected Animator animator;
        [SerializeField, Tooltip("if not set, will be the main camera")]
        public Camera CameraOrigin;
        public LayerMask attackLayer;
        [Tooltip("if not set, will be the main dataBase")]
        public DamageDecalDataBase bulletHolesDataBase;
        //public AudioClip swordSwing;
        //public AudioClip hitSound;
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
            if (!CameraOrigin)
                CameraOrigin = Camera.main;
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
            float tranTime = MeleeData.AnimTransitionTime;
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
            Invoke(nameof(ResetAttack), MeleeData.AttackTime);
        }
        public virtual bool CanAttack()
        {
            if (attacking) return false;
            if (animator.GetNextAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) ||
                animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) ||
                animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PICKING_UP)) return false;
            return true;
        }
        protected virtual void ResetAttack() =>
            attacking = false;
        public virtual void AttackRaycast()
        {
            if (Physics.SphereCast(CameraOrigin.transform.position, MeleeData.AttackRadius, CameraOrigin.transform.forward, out RaycastHit hitInfo, MeleeData.AttackDistance, attackLayer))
            {
                IDamageable damageable = IDamageable.GetDamageableFromGameObject(hitInfo.transform.gameObject);
                Debug.Log(MeleeData.Damage * damageAndForceMultiplier);
                damageable?.TakeDamage(new TakeDamageData(damageable, MeleeData.Damage * damageAndForceMultiplier, MeleeData.Force * damageAndForceMultiplier, hitInfo.point, GameObject.Find(MyConstants.CommonConstants.PLAYER)));
                Physics.Raycast(CameraOrigin.transform.position, CameraOrigin.transform.forward, out RaycastHit hitInfo2, attackLayer);
                OnHit?.Invoke(hitInfo2);
            }
        }
        protected virtual void HitTarget(RaycastHit hitInfo)
        {
            bulletHolesDataBase.MakeBulletHoleByInfo(hitInfo, CameraOrigin.transform.position, MeleeData.WeaponType);
        }
    }
}
