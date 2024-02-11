using EnemyConstantsNS;
using EnemyNS.Base;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace EnemyNS.Skills
{
    [CreateAssetMenu(fileName = "Jump Skill", menuName = "ScriptableObject/Enemy/Skills/Jump")]
    public class JumpSkill : SkillScriptableObject
    {
        [SerializeField, FormerlySerializedAs("MinJumpDistance")]
        private float minJumpDistance = 1.5f;
        [SerializeField, FormerlySerializedAs("MaxJumpDistance")]
        private float maxJumpDistance = 5f;
        [SerializeField, FormerlySerializedAs("HeightCurve")]
        private AnimationCurve heightCurve;
        [SerializeField, FormerlySerializedAs("JumpSpeed")]
        private float jumpSpeed = 1;
        [SerializeField, FormerlySerializedAs("LayersToIgnore")]
        private LayerMask layersToIgnore;
        [SerializeField, FormerlySerializedAs("UpAxis")]
        private Axis upAxis;

        public override bool CanUseSkill(EnemySkillInfo enemySkillInfo)
        {
            if (base.CanUseSkill(enemySkillInfo) && enemySkillInfo.EnemyMovement.State == EnemyState.Chase)
            {
                Vector3 enemyPosition = enemySkillInfo.EnemyMovement.transform.position, targetPosition = enemySkillInfo.PursuedTarget.transform.position;
                float distance = Vector3.Distance(enemyPosition, targetPosition);
                Ray ray = new Ray(enemyPosition, targetPosition - enemyPosition);
                if (Physics.SphereCast(ray, 0.6f, (targetPosition - enemyPosition).magnitude, ~(layersToIgnore |= (1 << enemySkillInfo.PursuedTarget.layer))))
                    return false;
                return distance >= minJumpDistance && distance <= maxJumpDistance;
            }
            return false;
        }
        public override void UseSkill(EnemySkillInfo enemySkillInfo)
        {
            base.UseSkill(enemySkillInfo);
            enemySkillInfo.SkillCoroutine = enemySkillInfo.Damageable.StartCoroutine(Jump(enemySkillInfo));
        }
        private IEnumerator Jump(EnemySkillInfo enemySkillInfo)
        {
            Transform enemyTransform = enemySkillInfo.EnemyMovement.transform, targetTransform = enemySkillInfo.PursuedTarget.transform;
            Vector3 endPosition = targetTransform.position;
            Vector3 startPosition = GetEnemyStartPosition(enemyTransform);
            Quaternion startRotation = enemyTransform.rotation;
            PrepareEnemyToJump(enemySkillInfo);
            for (float time = 0; time < 1; time += Time.deltaTime * jumpSpeed)
            {
                if (time <= 0.6f)
                    endPosition = targetTransform.position;
                Ray ray = new Ray(enemyTransform.position, -Vector3.up);
                if (Physics.Raycast(ray, out RaycastHit groundHit))
                    endPosition.y = groundHit.point.y + 0.1f;
                enemyTransform.position = Vector3.Lerp(startPosition, endPosition, time) + Vector3.up * heightCurve.Evaluate(time);
                if (time <= 0.6f)
                    enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, Quaternion.LookRotation(endPosition - enemyTransform.position), time);
                else
                    enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, DefineHeightRotation(enemyTransform.rotation, startRotation), time);
                yield return null;
            }
            EnemyAfterJump(enemySkillInfo, endPosition);
            timeFromLastUse = Time.time;
            isSkillActive = false;
        }
        private Vector3 GetEnemyStartPosition(Transform enemyTransform)
        {
            Vector3 startingPosition = enemyTransform.position;
            Ray ray = new Ray(startingPosition, -Vector3.up);
            if (Physics.Raycast(ray, out RaycastHit groundHit))
                startingPosition.y = groundHit.point.y;
            return startingPosition;
        }
        private void PrepareEnemyToJump(EnemySkillInfo enemySkillInfo)
        {
            EnemyMovement enemyMovement = enemySkillInfo.EnemyMovement;
            enemyMovement.Agent.enabled = false;
            enemyMovement.State = EnemyState.UsingAbility;
            enemySkillInfo.Animator?.SetTrigger(EnemyConstants.JUMP);
            enemySkillInfo.EnemyAttack.TryAttack();
        }
        private void EnemyAfterJump(EnemySkillInfo enemySkillInfo, Vector3 endPosition)
        {
            EnemyMovement enemyMovement = enemySkillInfo.EnemyMovement;
            enemySkillInfo.EnemyAttack.StopAttack();
            enemySkillInfo.Animator?.SetTrigger(EnemyConstants.LANDED);
            enemyMovement.Agent.enabled = true;
            if (NavMesh.SamplePosition(endPosition, out NavMeshHit hit, 1f, enemyMovement.Agent.areaMask))
                enemyMovement.State = EnemyState.Chase;
            else
                enemyMovement.State = enemyMovement.DefaultState;
        }
        private Quaternion DefineHeightRotation(Quaternion currentRotation, Quaternion startRotation)
        {
            Quaternion definedRot = new Quaternion(currentRotation.x, currentRotation.y, currentRotation.z, currentRotation.w);
            switch (upAxis)
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
