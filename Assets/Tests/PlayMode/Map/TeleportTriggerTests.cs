using LocationManagementNS;
using MyConstants;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
namespace Map
{

    public class TeleportTriggerTests
    {
        GameObject player;
        GameObject gameObject;
        Transform teleportPoint;
        TeleportTrigger teleportTrigger;
        [SetUp]
        public void Setup()
        {
            //Arrange
            player = GameObject.Instantiate(new GameObject());
            gameObject = GameObject.Instantiate(new GameObject());

            player.name = CommonConstants.PLAYER;
            teleportPoint = gameObject.transform;
            teleportTrigger = player.AddComponent<TeleportTrigger>();
            teleportTrigger.teleportPoint = teleportPoint;
        }
        [UnityTest]
        public IEnumerator StartTeleportingTest_Expect_TeleportedPlayer()
        {
            //Arrange
            player.transform.position = Vector3.zero + new Vector3(100, 0, 0);
            //Act
            teleportTrigger.StartTeleporting();
            yield return new WaitForSeconds(2.5f);
            //Assert
            Assert.AreEqual(teleportPoint.position, player.transform.position);
        }
    }
}