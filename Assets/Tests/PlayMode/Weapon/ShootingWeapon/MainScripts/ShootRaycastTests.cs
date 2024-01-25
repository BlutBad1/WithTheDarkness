using DamageableNS;
using NUnit.Framework;
using ScriptableObjectNS.Weapon.Gun;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using WeaponNS.ShootingWeaponNS;
namespace Weapon.ShootingWeaponNS.MainScripts
{
    public class ShootRaycastTests
    {
        private GameObject enemy;
        private BoxCollider boxCollider;
        private GunData gunData;
        private Damageable damageable;
        private ShootRaycast shootRaycast;
        private Camera cam;

        [SetUp]
        public void Setup()
        {
            //Arrange
            enemy = GameObject.Instantiate(new GameObject());
            boxCollider = enemy.AddComponent<BoxCollider>();
            enemy.AddComponent<Rigidbody>().isKinematic = true;
            boxCollider.isTrigger = true;
            damageable = enemy.AddComponent<Damageable>();
            GameObject raycastTestGO = GameObject.Instantiate(Resources.Load("RaycastTest", typeof(GameObject)) as GameObject);
            shootRaycast = raycastTestGO.GetComponent<ShootRaycast>();
            gunData = raycastTestGO.GetComponent<ShootingWeapon>().GunData;
            cam = raycastTestGO.GetComponent<Camera>();
            boxCollider.size = new Vector3(gunData.MaxDistance, gunData.MaxDistance, gunData.MaxDistance);
            //Set random position, so as not to interfere with other tests
            Vector3 randomPosition = TestsNS.TestUtilities.GetRandomPosition();
            raycastTestGO.transform.position = randomPosition;
            enemy.transform.position = randomPosition + new Vector3(gunData.MaxDistance, 0, 0);
            raycastTestGO.transform.LookAt(enemy.transform.position);
            cam.transform.LookAt(enemy.transform.position);
        }
        [UnityTest]
        public IEnumerator OnShootRaycastTest_Expect_HealthNotEqualHealthOnStart()
        {
            //Act
            shootRaycast.OnShootRaycast(gunData);
            yield return new WaitForSeconds(0.5f);
            //Assert
            Assert.AreNotEqual(damageable.Health, damageable.HealthOnStart);
        }
    }
}