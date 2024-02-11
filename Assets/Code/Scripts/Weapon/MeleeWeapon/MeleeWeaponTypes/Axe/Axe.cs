using WeaponConstantsNS.MeleeWeaponConstantsNS;
using PlayerScriptsNS;
using SoundNS;
using System.Collections;
using UnityEngine;
using WeaponConstantsNS;
using WeaponNS.MeleeWeaponNS;
using WeaponNS.ShootingWeaponNS;

public class Axe : MeleeWeaponBase
{
	[SerializeField]
	private AudioSourcesManager audioSourcesManager;
	[Header("Attacking"), SerializeField, Min(0)]
	private float maxHoldingUpAttackMultiplier = 1.5f;
	[SerializeField, Min(0)]
	private float timeOfHoldingUp = 2f;
	[Header("Blocking"), SerializeField]
	private float blockingTranTime = 0.5f;
	[SerializeField, Range(0, 100)]
	private float percentBlockingResistance = 35f;
	[SerializeField]
	private PlayerHealth playerHealth;

	private bool isHoldingUp = false;
	private bool isBlocking = false;
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
		PlayerBattleInput.AltAttackInputCanceled -= StopBlocking;
	}
	public override void DefineAnim()
	{
		string nextAnim = string.Empty;
		float tranTime = meleeData.AnimTransitionTime;
		if (isHoldingUp && !attacking && !isBlocking && !isBlockingAnim)
			nextAnim = AxeConstants.ATTACK_PREPARING;
		else if (attacking)
			nextAnim = AxeConstants.ATTACK_PERFORMING;
		else if (isBlockingAnim)
			nextAnim = AxeConstants.BLOCKING;
		else
			nextAnim = MainMeleeWeaponConstants.IDLE;
		if (currentAnimationState == AxeConstants.BLOCKING || nextAnim == AxeConstants.BLOCKING)
			tranTime = blockingTranTime;
		if (!CheckAnimConditions(nextAnim)) return;
		animator.CrossFadeInFixedTime(nextAnim, tranTime);
		currentAnimationState = nextAnim;
	}
	public void StartBlockingAnim() =>
		isBlockingAnim = true;
	public void Block()
	{
		isBlocking = true;
		playerHealth.PercentResistance = percentBlockingResistance;
		playerHealth.OnTakeDamageWithoutDamageData += DecreaseDurability;
	}
	public void StopBlocking()
	{
		isBlockingAnim = false;
		isBlocking = false;
		playerHealth.PercentResistance = 0;
		playerHealth.OnTakeDamageWithoutDamageData -= DecreaseDurability;
	}
	protected override void DecreaseDurability()
	{
		meleeData.CurrentDurability -= meleeData.MoveDurabilityCost;
		meleeData.CurrentDurability = meleeData.CurrentDurability <= 0 ? 0 : meleeData.CurrentDurability;
		weaponDurabilityEnd.OnDurabilityDecrease();
		if (meleeData.CurrentDurability <= 0)
		{
			StopBlocking();
			weaponDurabilityEnd.OnDurabilityEnd();
		}
	}
	public void PlayAudioOnHit(RaycastHit hit)
	{
		audioSourcesManager.CreateNewAudioSourceAndPlay(AxeConstants.HIT_SOUND);
	}
	protected override bool CanAttack()
	{
		if (animator.GetNextAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) ||
				  animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) ||
				  animator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PICKING_UP)) return false;
		return true;
	}
	public override void Attack()
	{
		if (!CanAttack()) return;
		isHoldingUp = true;
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
		if (isHoldingUp && !isBlockingAnim)
			attacking = true;
		isHoldingUp = false;
	}
	protected IEnumerator AttackTimer()
	{
		float time = 0;
		while (time <= timeOfHoldingUp)
		{
			damageAndForceMultiplier = Mathf.Lerp(1, maxHoldingUpAttackMultiplier, time / timeOfHoldingUp);
			time += Time.deltaTime;
			yield return null;
		}
	}
}
