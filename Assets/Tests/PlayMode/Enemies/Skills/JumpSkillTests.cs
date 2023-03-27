using System.Collections;
using System.Collections.Generic;
using EnemyBaseNS;
using EnemySkillsNS;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
namespace Enemies.Skills
{


    public class EnemyMovementForTesting : EnemyMovement
    {
        protected override IEnumerator FollowTarget()
        {
            yield return null;
        }
    }
public class JumpSkillTests
{

        GameObject enemy;
        Enemy enemyScript;
        GameObject player;
        JumpSkill jumpSkill;
        BoxCollider boxCollider;
        Rigidbody rigidbody;

        [SetUp]
        public void Setup()
        {
            //Arrange
            enemy = GameObject.Instantiate(new GameObject());
            player = GameObject.Instantiate(new GameObject());
            player.name = MyConstants.CommonConstants.PLAYER;
            enemyScript = enemy.AddComponent<Enemy>();

        
            enemy.AddComponent<EnemyLineOfSightChecker>();
            enemy.AddComponent<NavMeshAgent>();
            enemyScript.Movement = enemy.AddComponent<EnemyMovementForTesting>();
            enemyScript.Movement.Player = player;


            enemyScript.Agent = enemyScript.Movement.Agent;
            jumpSkill = new JumpSkill();
            jumpSkill.MinJumpDistance = 0f;
            jumpSkill.HeightCurve = AnimationCurve.Constant(0,1,1);
            jumpSkill.MaxJumpDistance = 10000f;
            jumpSkill.Cooldown = 0;
          
          
            Vector3 fwd = enemy.transform.TransformDirection(Vector3.forward);
            player.transform.position = fwd + new Vector3(100, 0, 0);

        }
        [UnityTest]
        public IEnumerator CanUseSkill_Expect_True()
        {
           //Assert
            enemyScript.Movement.DefaultState= EnemyState.Chase;
            enemyScript.Movement.State = EnemyState.Chase;
                yield return new WaitForSeconds(2f);
            //Act
            Assert.AreEqual(true, jumpSkill.CanUseSkill(enemyScript, player));
          

        }
      
    }
}