using System.Collections;
using System.Collections.Generic;
using ControllingParticlesNS;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
namespace ControllingParticles
{


    public class ParitcleDecalPoolTests
    {
        GameObject paticalGameObject;
        ParticleDecalPool particleDecalPool;
        ParticleCollisionEvent particleCollisionEvent = new ParticleCollisionEvent();
        Gradient colorGradient;
        [SetUp]
        public void Setup()
        {
            //Arrange
            colorGradient = new Gradient();
            paticalGameObject = GameObject.Instantiate(new GameObject());
            paticalGameObject.AddComponent<ParticleSystem>();
            particleDecalPool = paticalGameObject.AddComponent<ParticleDecalPool>();
            particleDecalPool.decalsMinSize = 0;

        }
        [UnityTest]
        public IEnumerator ParticleHitTest_Expect_NotEqual()
        {
            //Act
            ParticleDecalData particleDecalData = new ParticleDecalData();
            particleDecalData.Rotation = particleDecalPool.particleData[0].Rotation;
            particleDecalData.Size = particleDecalPool.particleData[0].Size;
            particleDecalData.Color = particleDecalPool.particleData[0].Color;
            particleDecalPool.ParticleHit(particleCollisionEvent, colorGradient);
            yield return new WaitForSeconds(0.1f);

            //Assert
            Assert.AreNotEqual(particleDecalData.Size, particleDecalPool.particleData[0].Size);
            Assert.AreNotEqual(particleDecalData.Rotation, particleDecalPool.particleData[0].Rotation);
        }
    }
}