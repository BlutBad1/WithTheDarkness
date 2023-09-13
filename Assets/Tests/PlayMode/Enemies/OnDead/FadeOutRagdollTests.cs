using EnemyNS.Attack;
using EnemyNS.Base;
using EnemyNS.Death;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Enemies.OnDead
{
    public class FadeOutRagdollTests
    {
        GameObject enemy;
        GameObject player;
        Enemy enemyScript;
        FadeOutRagdoll fadeOutRagdoll;
        GameObject children;
        Vector3 defaultPosition;
        [SetUp]
        public void Setup()
        {
            //Arrange
            enemy = GameObject.Instantiate(new GameObject());
            children = GameObject.Instantiate(new GameObject());
            player = GameObject.Instantiate(new GameObject());
            children.transform.parent = enemy.transform;
            children.AddComponent<Rigidbody>();
            player.name = "Player";
            enemyScript = enemy.AddComponent<Enemy>();
            enemy.AddComponent<Animator>();
            enemy.AddComponent<EnemyLineOfSightChecker>();
            enemy.AddComponent<RagdollEnabler>();
            enemy.GetComponent<RagdollEnabler>().RagdollRoot = children.transform;
            enemyScript.Movement = enemy.AddComponent<EnemyMovement>();
            fadeOutRagdoll = enemy.AddComponent<FadeOutRagdoll>();
            defaultPosition = enemy.transform.position;
        }
        [UnityTest]
        public IEnumerator OnDeadTest_Expect_DeadStatus()
        {
            //Act+Assert
            enemyScript.TakeDamage(110);
            yield return new WaitForSeconds(3f);
            Assert.AreEqual(EnemyState.Dead, enemyScript.Movement.State);
            Assert.AreEqual(false, enemyScript.Movement.Agent.enabled);
            Assert.AreEqual(false, enemyScript.Movement.enabled);
            Assert.AreNotEqual(defaultPosition, enemy.transform.position);
        }
    }
}