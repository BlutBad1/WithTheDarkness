using EnemyNS.Attack;
using MyConstants;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Enemies.Base
{
    public class EnemyLineOfSightCheckerTests
    {
        GameObject enemy;
        GameObject player;
        LayerMask playerLayerMask;
        EnemyLineOfSightChecker enemyLineOfSight;
        BoxCollider boxCollider;
        bool checkBool;
        void OnLoseSight(GameObject player) =>
            checkBool = false;
        void OnGainSight(GameObject player) =>
            checkBool = true;
        [SetUp]
        public void Setup()
        {
            //Arrange
            enemy = GameObject.Instantiate(new GameObject());
            player = GameObject.Instantiate(new GameObject());
            player.AddComponent<Rigidbody>();
            player.name = CommonConstants.PLAYER;
            enemyLineOfSight = enemy.AddComponent<EnemyLineOfSightChecker>();
            boxCollider = player.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(10, 10, 10);
            playerLayerMask = (1 << player.layer);
            enemyLineOfSight.LayersForProcessing = playerLayerMask;
            enemyLineOfSight.OnGainSight += OnGainSight;
            enemyLineOfSight.OnLoseSight += OnLoseSight;
            Vector3 fwd = enemy.transform.TransformDirection(Vector3.forward);
            player.transform.position = fwd + new Vector3(100, 0, 0);

        }
        [UnityTest]
        public IEnumerator OnGainSightTest_Expect_CheckBoolTrue()
        {
            //Act+Assert
            checkBool = false;
            enemyLineOfSight.ViewDistance = 20000f;
            enemyLineOfSight.FieldOfView = 360;
            yield return new WaitForSeconds(2f);
            Assert.IsTrue(checkBool);
        }
        [UnityTest]
        public IEnumerator OnLoseSightTest_Expect_CheckBoolFalse()
        {
            //Act+Assert
            checkBool = true;
            enemyLineOfSight.ViewDistance = 0f;
            enemyLineOfSight.FieldOfView = 0;
            yield return new WaitForSeconds(2f);
            Assert.IsTrue(!checkBool);
        }
    }
}