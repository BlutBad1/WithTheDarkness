using LightNS;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
namespace CommonCore.LampNLight
{


    public class LightIntensityControllingTests
    {
        private LightGlowTimer lightsTimer;
        private GameObject gameObject;
        private Light light;


        [SetUp]
        public void Setup()
        {
            //Arrange
            gameObject = GameObject.Instantiate(new GameObject());
            light = gameObject.AddComponent<Light>();
            light.intensity = 1f;
            lightsTimer = gameObject.AddComponent<LightGlowTimer>();
            lightsTimer.SetGlowTime(1f);
            gameObject.AddComponent<LightIntensityControlling>();
        }
        [UnityTest]
        public IEnumerator UpdateTest_Expect_SlowDecreaseIntensityByTime1()
        {

            yield return new WaitForSeconds(2f);
            //Assert
            Assert.IsTrue(light.intensity <= 0f);

        }
        [UnityTest]
        public IEnumerator UpdateTest_Expect_SlowDecreaseIntensityByTime2()
        {
            LightGlowTimer.AddTime(5f);
              yield return new WaitForSeconds(0.5f);
            //Assert
            Assert.IsTrue(light.intensity > 0f);
            Assert.IsTrue(light.intensity < 5f);


        }
    }
}