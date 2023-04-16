using EnemyBaseNS;
using MyConstants;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySkillsNS
{


    [CreateAssetMenu(fileName = "Jump Skill", menuName = "ScriptableObject/Skills/Jump")]
    public class JumpSkill : SkillScriptableObject
    {
        public float MinJumpDistance = 1.5f;
        public float MaxJumpDistance = 5f;
        public AnimationCurve HeightCurve;
        public float JumpSpeed = 1;


        public override bool CanUseSkill(Enemy enemy, GameObject player)
        {
            if (base.CanUseSkill(enemy, player) && enemy.Movement.State == EnemyState.Chase)
            {

                float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

                Ray ray = new Ray(enemy.transform.position, player.transform.position - enemy.transform.position);
                if (Physics.SphereCast(ray, 0.6f, (player.transform.position - enemy.transform.position).magnitude, ~(1 << 11 | 1 << 12)))
                {

                    return false;
                }


                return !IsActivating && UseTime + Cooldown < Time.time && distance >= MinJumpDistance && distance <= MaxJumpDistance;
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
            RaycastHit groundHit;
            Vector3 endingPosition = player.transform.position,
                startingPosition = enemy.transform.position;

            Ray ray = new Ray(enemy.transform.position, -Vector3.up);
            if (Physics.Raycast(ray, out groundHit))
            {
                startingPosition.y = groundHit.point.y;
            }

            enemy.Agent.enabled = false;
            enemy.Movement.enabled = false;
            enemy.Movement.State = EnemyState.UsingAbility;
            enemy.Animator?.SetTrigger(EnemyConstants.JUMP);
            for (float time = 0; time < 1; time += Time.deltaTime * JumpSpeed)
            {
                if (time <= 0.6)
                {
                    endingPosition = player.transform.position;
                }
                ray = new Ray(enemy.transform.position, -Vector3.up);
                if (Physics.Raycast(ray, out groundHit))
                {
                    endingPosition.y = groundHit.point.y + 0.4f;
                }

                enemy.transform.position = Vector3.Lerp(startingPosition, endingPosition, time) + Vector3.up * HeightCurve.Evaluate(time);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(endingPosition - enemy.transform.position), time);
                yield return null;
            }

            enemy.Animator?.SetTrigger(EnemyConstants.LANDED);
            UseTime = Time.time;
            enemy.enabled = true;
            enemy.Movement.enabled = true;
            enemy.Agent.enabled = true;
            if (NavMesh.SamplePosition(endingPosition, out NavMeshHit hit, 1f, enemy.Agent.areaMask))
            {
                // enemy.Agent.Warp(hit.position);

                enemy.Movement.State = EnemyState.Chase;
            }

            IsActivating = false;

        }

    }
}
