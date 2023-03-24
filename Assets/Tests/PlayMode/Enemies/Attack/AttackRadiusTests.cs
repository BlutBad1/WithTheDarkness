using EnemyAttackNS;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Enemies.Attack
{
    public class AttackRadiusTests
    {
        GameObject enemy;
        GameObject player;
        LayerMask playerLayerMask;
        AttackRadius attackRadius;
        BoxCollider boxCollider;
        [SetUp]
        public void Setup()
        {
            //Arrange
            enemy = GameObject.Instantiate(new GameObject());
            attackRadius = enemy.AddComponent<AttackRadius>();

            player = GameObject.Instantiate(new GameObject());
            player.AddComponent<Damageable>();
            player.AddComponent<Rigidbody>();

            attackRadius.Collider = enemy.AddComponent<SphereCollider>();
            boxCollider = player.AddComponent<BoxCollider>();

            boxCollider.size = new Vector3(100, 1000, 1000);
            playerLayerMask = (1 << player.layer);
            attackRadius.WhatIsPlayer = playerLayerMask;
            attackRadius.Collider.isTrigger = true;
            attackRadius.Collider.radius = 3000f;


        }
        [UnityTest]
        public IEnumerator OnTriggerEnterTest_Expect_DamageToPlayer()
        {


            //Act
            yield return new WaitForSeconds(0.1f);
            //Assert

            Assert.IsTrue(player.GetComponent<Damageable>().Health < 100);
        }
    }
}