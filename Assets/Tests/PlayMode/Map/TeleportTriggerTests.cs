using LocationManagementNS;
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
		BlackScreenTeleportTrigger teleportTrigger;
		[SetUp]
		public void Setup()
		{
			//Arrange
			player = GameObject.Instantiate(new GameObject());
			gameObject = GameObject.Instantiate(new GameObject());
			// player.name = Constants.PLAYER;
			teleportPoint = gameObject.transform;
			teleportTrigger = player.AddComponent<BlackScreenTeleportTrigger>();
			teleportTrigger.TeleportPoint = teleportPoint;
		}
		[UnityTest]
		public IEnumerator StartTeleportingTest_Expect_TeleportedPlayer()
		{
			//Arrange
			player.transform.position = Vector3.zero + new Vector3(100, 0, 0);
			//Act
			yield return new WaitForSeconds(2.5f);
			//Assert
			Assert.AreEqual(teleportPoint.position, player.transform.position);
		}
	}
}