using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Jump Skill", menuName = "ScriptableObject/Skills/Jump")]
public class JumpSkill : SkillScriptableObject
{
    public float MinJumpDistance = 1.5f;
    public float MaxJumpDistance = 5f;
    public AnimationCurve HeightCurve;
    public float JumpSpeed = 1;

    public override bool CanUseSkill(Enemy enemy, GameObject player)
    {
        if (base.CanUseSkill(enemy, player))
        {
            float distance = Vector3.Distance(enemy.transform.position, player.transform.position);
            return !IsActivating && UseTime+Cooldown<Time.time&&distance>=MaxJumpDistance&& distance<=MaxJumpDistance;
        }
                return false;
    }
    public override void UseSkill(Enemy enemy, GameObject player)
    {
        base.UseSkill(enemy, player);
        enemy.StartCoroutine(Jump(enemy, player)); ;
    }
    private IEnumerator Jump(Enemy enemy, GameObject player)
    {
        enemy.Agent.enabled = false;
        enemy.Movement.enabled = false;
        enemy.Movement.State = EnemyState.UsingAbility;
        Vector3 startingPosition = enemy.transform.position;
        enemy.Animator.SetTrigger(EnemyMovement.Jump);
        for (float time = 0; time < 1; time+=Time.deltaTime*JumpSpeed)
        {
            enemy.transform.position = Vector3.Lerp(startingPosition, player.transform.position, time)+ Vector3.up*HeightCurve.Evaluate(time);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(player.transform.position-enemy.transform.position),time);
            yield return null;
        }
        enemy.Animator.SetTrigger(EnemyMovement.Landed);
        UseTime = Time.time;
        enemy.enabled = true;
        enemy.Movement.enabled = true;
        enemy.Agent.enabled = true;
        if(NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit , 1f, enemy.Agent.areaMask))
        {
            enemy.Agent.Warp(hit.position);
            enemy.Movement.State = EnemyState.Chase;
        }
        IsActivating = false;
    }
}
