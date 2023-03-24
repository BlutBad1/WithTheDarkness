using EnemyAttackNS;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
namespace Enemies.Attack
{
    public class StraightAttackWithoutEnemyMovement : StraightAttack
    {
        public override bool CanAttack(GameObject enemy, GameObject player)
        {
            return true;
        }
    }

    public class StraightAttackTests
    {
        GameObject enemy;
        GameObject player;
        LayerMask playerLayerMask;
        StraightAttackWithoutEnemyMovement attackStraight;
        BoxCollider boxCollider;
        [SetUp]
        public void Setup()
        {
            //Arrange
            enemy = GameObject.Instantiate(new GameObject());
            player = GameObject.Instantiate(new GameObject());
            player.AddComponent<Damageable>();
            player.AddComponent<Rigidbody>();
            player.name = "Player";
            attackStraight = enemy.AddComponent<StraightAttackWithoutEnemyMovement>();
            attackStraight.AttackColider = enemy.AddComponent<SphereCollider>();
            boxCollider = player.AddComponent<BoxCollider>();
            attackStraight.ObjectUnderAttack = player;
            boxCollider.size = new Vector3(100, 1000, 1000);
            playerLayerMask = (1 << player.layer);
            attackStraight.WhatIsPlayer = playerLayerMask;
            attackStraight.AttackColider.isTrigger = true;
            attackStraight.AttackColider.radius = 3000f;
            attackStraight.IsAttacking = true;


        }
        [UnityTest]
        public IEnumerator OnTriggerEnterTest_Expect_DamageToPlayer()
        {
            //Act
            attackStraight.TryAttack();

            yield return new WaitForSeconds(0.5f);

            // Assert
            Assert.IsTrue(player.GetComponent<Damageable>().Health < 100);
        }
    }
}