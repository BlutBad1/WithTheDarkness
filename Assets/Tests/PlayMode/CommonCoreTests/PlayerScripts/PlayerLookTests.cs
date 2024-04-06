using NUnit.Framework;
using PlayerScriptsNS;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace CommonCore.PlayerScripts
{
    public class PlayerLookTests
    {
        GameObject player;
        GameObject camera;
        FPSPlayerLook playerLook;
        float xRotation = 0;
        [SetUp]
        public void Setup()
        {
            //Arrange
            player = GameObject.Instantiate(new GameObject());
            camera = GameObject.Instantiate(new GameObject());
            camera.transform.parent = player.transform;
            camera.AddComponent<Camera>();
            playerLook = player.AddComponent<FPSPlayerLook>();
        }
        [UnityTest]
        public IEnumerator PlayerLookTestInput50xAnd50y_Expect_Equal()
        {
            //Arrange
            Vector2 input;
            input.x = 50;
            input.y = 50;
            xRotation -= (50 * Time.deltaTime) * 30f;
            xRotation = Mathf.Clamp(xRotation, -80f, 70f);
            Quaternion expectedCameraLocalRotation = Quaternion.Euler(xRotation, 0, 0);
            GameObject expectedTransformRotation = GameObject.Instantiate(new GameObject());
            expectedTransformRotation.transform.Rotate(Vector3.up * (50 * Time.deltaTime) * 30f);
            //Act
            playerLook.ProcessLook(input);
          
            yield return new WaitForSeconds(0.1f);
            Quaternion cameraLocalRotation = camera.transform.localRotation;
            //Assert
            Assert.AreEqual(expectedCameraLocalRotation, cameraLocalRotation);
            Assert.AreEqual(expectedTransformRotation.transform.rotation, player.transform.rotation);
        }
    }
}