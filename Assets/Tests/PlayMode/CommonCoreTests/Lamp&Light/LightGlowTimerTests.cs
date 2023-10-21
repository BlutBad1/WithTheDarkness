using LightNS;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace CommonCore.LampNLight
{
    public class LightGlowTimerTests
    {
        private LightGlowTimer lightsTimer;
        private GameObject gameObject;
        [SetUp]
        public void Setup()
        {
            //Arrange
            gameObject = GameObject.Instantiate(new GameObject());
            lightsTimer = gameObject.AddComponent<LightGlowTimer>();
            lightsTimer.MaxGlowTime = 100f;
            lightsTimer.GlowTime = 100f;
        }
        [UnityTest]
        public IEnumerator UpdateTest_Expect_SlowDecreaseCurrentTimeLeft()
        {

            yield return new WaitForSeconds(1f);
            //Assert
            //Assert.IsTrue(LightGlowTimer.CurrentTimeLeft < 99);

        }
        [UnityTest]
        public IEnumerator AddTime_Expect_AddedTimeToCurrentTimeLeftAndStartedTimeLeft()
        {
            //Act
            // LightGlowTimer.AddTime(50f);
            yield return new WaitForSeconds(1f);
            //Assert
            // Assert.IsTrue(LightGlowTimer.CurrentTimeLeft > 110);
            //Assert.IsTrue(LightGlowTimer.StartedTimeLeft > 110);

        }
    }
}