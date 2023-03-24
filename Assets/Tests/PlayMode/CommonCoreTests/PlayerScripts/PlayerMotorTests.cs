using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
namespace CommonCore.PlayerScripts
{

    public class PlayerMotorTests
    {
        GameObject player;
        GameObject expectedPlayer;
        CharacterController character;
        CharacterController expectedCharacter;
        PlayerMotor playerMotor;
        Vector3 expectedVelocity;
        [SetUp]
        public void Setup()
        {
            //Arrange
            player = GameObject.Instantiate(new GameObject());
            expectedPlayer = GameObject.Instantiate(new GameObject());
            character = player.AddComponent<CharacterController>();
            expectedCharacter = expectedPlayer.AddComponent<CharacterController>();
            playerMotor = player.AddComponent<PlayerMotor>();
        }

        [UnityTest]
        public IEnumerator ProcessMoveTest_Expect_True()
        {
            //Arrange
            Vector2 input;
            input.x = 1;
            input.y = 1;
            Vector3 expectedMoveDirection = Vector3.zero;
            expectedMoveDirection.x = 1;
            expectedMoveDirection.z = 1;
            expectedCharacter.Move(expectedPlayer.transform.TransformDirection(expectedMoveDirection) * 5 * Time.deltaTime);
            expectedVelocity.y += (-9.8f) * Time.deltaTime;
            expectedCharacter.Move(expectedVelocity * Time.deltaTime);

            //Act
            playerMotor.ProcessMove(input);
            yield return new WaitForSeconds(0.1f);
            Vector3 buffer = (expectedPlayer.transform.TransformDirection(expectedMoveDirection) * 5 * Time.deltaTime) - playerMotor.currentVelocity;

            Assert.IsTrue(Mathf.Abs(buffer.magnitude) < 0.2f);
        }
        [UnityTest]
        public IEnumerator ProcessMoveTestInJump_Expect_True()
        {
            //Arrange
            Vector2 input;
            input.x = 1;
            input.y = 1;
            Vector3 expectedMoveDirection = Vector3.zero;
            expectedMoveDirection.x = 1;
            expectedMoveDirection.z = 1;
            expectedCharacter.Move(expectedPlayer.transform.TransformDirection(expectedMoveDirection) * 5 * Time.deltaTime);
            expectedVelocity.y += Mathf.Sqrt(3f * -3.0f * -9.8f);
            expectedCharacter.Move(expectedVelocity * Time.deltaTime);

            //Act
            playerMotor.isGrounded = true;
            playerMotor.Jump();
            playerMotor.ProcessMove(input);
            yield return new WaitForSeconds(0.1f);
            Vector3 buffer = (expectedPlayer.transform.TransformDirection(expectedMoveDirection) * 5 * Time.deltaTime) - playerMotor.currentVelocity;

            Assert.IsTrue(Mathf.Abs(buffer.magnitude) < 0.2f);
        }
    }
}