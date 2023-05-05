using NUnit.Framework;
using PlayerScriptsNS;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace CommonCore.PlayerScripts
{


    public class PlayerHealthTests
    {
        GameObject player;
        GameObject camera;
        CameraShake cameraShake;
        PlayerHealth playerHealth;
        [SetUp]
        public void Setup()
        {
            //Arrange
            player = GameObject.Instantiate(new GameObject());
            camera = GameObject.Instantiate(new GameObject());
            camera.transform.parent = player.transform;
            camera.AddComponent<Camera>();
            cameraShake = camera.AddComponent<CameraShake>();
            playerHealth = player.AddComponent<PlayerHealth>();
        }
        [UnityTest]
        public IEnumerator TakeDamageTest_Expect_90Health()
        {

            //Act
            playerHealth.TakeDamage(10);
            yield return new WaitForSeconds(0.1f);
            //Assert
            Assert.AreEqual(90,playerHealth.Health);
        }
    }
}