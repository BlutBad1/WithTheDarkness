using EnemyBaseNS;
using EnemyOnTakeDamageNS;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
namespace Enemies.OnTakeDamage
{

    public class KnockoutEffectTests
    {
        GameObject enemy;
        Enemy enemyScript;
        GameObject player;
        KnockoutEffect knockoutEffect;
        BoxCollider boxCollider;
        Rigidbody rigidbody;

        [SetUp]
        public void Setup()
        {
            //Arrange
            enemy = GameObject.Instantiate(new GameObject());
            player = GameObject.Instantiate(new GameObject());
            player.name = "Player";
            enemyScript = enemy.AddComponent<Enemy>();

            rigidbody = enemy.AddComponent<Rigidbody>();
            enemy.AddComponent<EnemyLineOfSightChecker>();
            enemyScript.Movement = enemy.AddComponent<EnemyMovement>();
            enemyScript.Agent = enemyScript.Movement.Agent;
            knockoutEffect = enemy.AddComponent<KnockoutEffect>();
            knockoutEffect.KnockoutEnable = true;
        }
        [UnityTest]
        public IEnumerator TakeDamageKnockoutEffect_Expect_DisablingActivities()
        {
            //Act+Assert

            enemyScript.TakeDamage(10, 100, new Vector3(0,0,0));
            Assert.AreEqual(false, enemyScript.Agent.enabled);
            Assert.AreEqual(false, rigidbody.isKinematic);
            Assert.AreEqual(Vector3.zero, enemyScript.Agent.velocity);
            yield return new WaitForSeconds(2f);
            Assert.AreEqual(true, enemyScript.Agent.enabled);
            Assert.AreEqual(true, rigidbody.isKinematic);

        }
    }
}