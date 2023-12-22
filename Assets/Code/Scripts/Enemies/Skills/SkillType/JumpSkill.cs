using EnemyNS.Base;
using MyConstants.CreatureConstants.EnemyConstants;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Skills
{
    public enum Axis
    {
        X, Y, Z
    }
    [CreateAssetMenu(fileName = "Jump Skill", menuName = "ScriptableObject/Enemy/Skills/Jump")]
    public class JumpSkill : SkillScriptableObject
    {
        public float MinJumpDistance = 1.5f;
        public float MaxJumpDistance = 5f;
        public AnimationCurve HeightCurve;
        public float JumpSpeed = 1;
        public LayerMask LayersToIgnore;
        public Axis UpAxis;
        public override bool CanUseSkill(Enemy enemy, GameObject target)
        {
            if (base.CanUseSkill(enemy, target) && enemy.Movement.State == EnemyState.Chase)
            {
                float distance = Vector3.Distance(enemy.transform.position, target.transform.position);
                Ray ray = new Ray(enemy.transform.position, target.transform.position - enemy.transform.position);
                if (Physics.SphereCast(ray, 0.6f, (target.transform.position - enemy.transform.position).magnitude, ~(LayersToIgnore |= (1 << target.layer))))
                    return false;
                return !IsActivating && UseTime + Cooldown < Time.time && distance >= MinJumpDistance && distance <= MaxJumpDistance;
            }
            return false;
        }
        public override void UseSkill(Enemy enemy, GameObject target)
        {
            base.UseSkill(enemy, target);
            enemy.skillCoroutine = enemy.StartCoroutine(Jump(enemy, target));
        }
        private IEnumerator Jump(Enemy enemy, GameObject target)
        {
            RaycastHit groundHit;
            Vector3 endingPosition = target.transform.position,
            startingPosition = enemy.transform.position;
            Ray ray = new Ray(enemy.transform.position, -Vector3.up);
            if (Physics.Raycast(ray, out groundHit))
                startingPosition.y = groundHit.point.y;
            enemy.Movement.Agent.enabled = false;
            // enemy.Movement.enabled = false;
            enemy.Movement.State = EnemyState.UsingAbility;
            enemy.Animator?.SetTrigger(MainEnemyConstants.JUMP);
            Quaternion startRotation = enemy.transform.rotation;
            enemy.EnemyAttack.TryAttack();
            for (float time = 0; time < 1; time += Time.deltaTime * JumpSpeed)
            {
                if (time <= 0.6f)
                    endingPosition = target.transform.position;
                ray = new Ray(enemy.transform.position, -Vector3.up);
                if (Physics.Raycast(ray, out groundHit))
                    endingPosition.y = groundHit.point.y + 0.1f;
                enemy.transform.position = Vector3.Lerp(startingPosition, endingPosition, time) + Vector3.up * HeightCurve.Evaluate(time);
                if (time <= 0.6f)
                    enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(endingPosition - enemy.transform.position), time);
                else
                    enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation,
                        DefineHeightRotation(enemy.transform.rotation, startRotation), time);
                yield return null;
            }
            enemy.EnemyAttack.StopAttack();
            enemy.Animator?.SetTrigger(MainEnemyConstants.LANDED);
            UseTime = Time.time;
            enemy.enabled = true;
            // enemy.Movement.enabled = true;
            enemy.Movement.Agent.enabled = true;
            if (NavMesh.SamplePosition(endingPosition, out NavMeshHit hit, 1f, enemy.Movement.Agent.areaMask))
            {
                // enemy.Agent.Warp(hit.position);
                enemy.Movement.State = EnemyState.Chase;
            }
            else
                enemy.Movement.State = enemy.Movement.DefaultState;
            IsActivating = false;
        }
        private Quaternion DefineHeightRotation(Quaternion currentRotation, Quaternion startRotation)
        {
            Quaternion definedRot = new Quaternion(currentRotation.x, currentRotation.y, currentRotation.z, currentRotation.w);
            switch (UpAxis)
            {
                case Axis.X:
                    definedRot.x = startRotation.x;
                    break;
                case Axis.Y:
                    definedRot.y = startRotation.y;
                    break;
                case Axis.Z:
                    definedRot.z = startRotation.z;
                    break;
                default:
                    break;
            }
            return definedRot;
        }
    }
}
