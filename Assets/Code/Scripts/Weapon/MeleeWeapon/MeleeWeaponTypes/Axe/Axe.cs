using MyConstants.WeaponConstants;
using MyConstants.WeaponConstants.MeleeWeaponConstants;
using PlayerScriptsNS;
using SoundNS;
using System.Collections;
using UnityEngine;
using WeaponNS.MeleeWeaponNS;
using WeaponNS.ShootingWeaponNS;

public class Axe : MeleeWeaponBase
{
    public AudioSourcesManager AudioSourcesManager;
    [Header("Attacking"), Min(0)]
    public float MaxHoldingUpAttackMultiplier = 1.5f;
    [Min(0)]
    public float TimeOfHoldingUp = 2f;
    [HideInInspector]
    public bool IsHoldingUp = false;
    [Header("Blocking")]
    public float BlockingTranTime = 0.5f;
    [Range(0, 100)]
    public float PercentBlockingResistance = 35f;
    public PlayerHealth PlayerHealth;
    [HideInInspector]
    public bool IsBlocking = false;
    private bool isBlockingAnim = false;
    private Coroutine holdingUpAttackCoroutine = null;

    public override void AttachActions()
    {
        base.AttachActions();
        OnHit += PlayAudioOnHit;
        PlayerBattleInput.AttackInputCanceled += PerformAttack;
        PlayerBattleInput.AltAttackInputStarted += StartBlockingAnim;
        PlayerBattleInput.AltAttackInputCanceled += StopBlocking;
    }
    public override void DettachActions()
    {
        base.DettachActions();
        OnHit -= PlayAudioOnHit;
        PlayerBattleInput.AttackInputCanceled -= PerformAttack;
        PlayerBattleInput.AltAttackInputStarted -= StartBlockingAnim;
        PlayerBattleInput.AltAttackInputStarted -= StopBlocking;
    }
    public override void DefineAnim()
    {
        string nextAnim = string.Empty;
        float tranTime = MeleeData.AnimTransitionTime;
        if (IsHoldingUp && !attacking && !IsBlocking && !isBlockingAnim)
            nextAnim = AxeConstants.ATTACK_PREPARING;
        else if (attacking)
            nextAnim = AxeConstants.ATTACK_PERFORMING;
        else if (isBlockingAnim)
            nextAnim = AxeConstants.BLOCKING;
        else
            nextAnim = MainMeleeWeaponConstants.IDLE;
        if (currentAnimationState == AxeConstants.BLOCKING || nextAnim == AxeConstants.BLOCKING)
            tranTime = BlockingTranTime;
        if (!CheckAnimConditions(nextAnim)) return;
        animator.CrossFadeInFixedTime(nextAnim, tranTime);
        currentAnimationState = nextAnim;
    }
    public void StartBlockingAnim() =>
        isBlockingAnim = true;
    public void Block()
    {
        IsBlocking = true;
        PlayerHealth.PercentResistance = PercentBlockingResistance;
    }
    public void StopBlocking()
    {
        isBlockingAnim = false;
        IsBlocking = false;
        PlayerHealth.PercentResistance = 0;
    }
    public void PlayAudioOnHit(RaycastHit hit)
    {
        AudioSourcesManager.CreateNewAudioSourceAndPlay(AxeConstants.HIT_SOUND);
    }
    public override bool CanAttack()
    {
        if (animator.GetNextAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) ||
                  animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) ||
                  animator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PICKING_UP)) return false;
        return true;
    }
    public override void Attack()
    {
        if (!CanAttack()) return;
        IsHoldingUp = true;
        // Invoke(nameof(ResetAttack), MeleeData.AttackTime);
    }
    public void AttackPreparing()
    {
        damageAndForceMultiplier = 1f;
        if (holdingUpAttackCoroutine != null)
            StopCoroutine(holdingUpAttackCoroutine);
        holdingUpAttackCoroutine = StartCoroutine(AttackTimer());
    }
    protected void PerformAttack()
    {
        if (holdingUpAttackCoroutine != null)
            StopCoroutine(holdingUpAttackCoroutine);
        holdingUpAttackCoroutine = null;
        if (IsHoldingUp && !isBlockingAnim)
            attacking = true;
        IsHoldingUp = false;
    }
    protected IEnumerator AttackTimer()
    {
        float time = 0;
        while (time <= TimeOfHoldingUp)
        {
            damageAndForceMultiplier = Mathf.Lerp(1, MaxHoldingUpAttackMultiplier, time / TimeOfHoldingUp);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
