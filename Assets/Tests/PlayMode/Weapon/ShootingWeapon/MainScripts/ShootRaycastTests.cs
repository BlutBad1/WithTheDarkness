using DamageableNS;
using NUnit.Framework;
using ScriptableObjectNS.Weapon.Gun;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using WeaponNS.ShootingWeaponNS;
namespace Weapon.ShootingWeaponNS.MainScripts
{
    public class ShootRaycastTestClass : ShootRaycast
    {
        protected override void Start()
        {
        }
    }
    public class ShootRaycastTests
    {
        GameObject enemy;
        GameObject player;
        GameObject thisGun;
        BoxCollider boxCollider;
        GunData thisGunData;
        Damageable damageable;
        ShootRaycastTestClass shootRaycast;
        Camera cam;
        ShootingWeapon shootingWeapon;
        [SetUp]
        public void Setup()
        {
            //Arrange
            enemy = GameObject.Instantiate(new GameObject());
            player = GameObject.Instantiate(new GameObject());
            thisGun = GameObject.Instantiate(new GameObject());
            boxCollider = enemy.AddComponent<BoxCollider>();
            enemy.AddComponent<Rigidbody>();
            boxCollider.isTrigger = true;
            shootingWeapon = player.AddComponent<ShootingWeapon>();
            shootRaycast = player.AddComponent<ShootRaycastTestClass>();
            thisGun.AddComponent<Animator>();
            thisGunData = new GunData();
            thisGunData.MaxDistance = 1000f;
            thisGunData.Force = 1f;
            thisGunData.Damage = 15;
            shootingWeapon.gunData = thisGunData;
            //shootingWeapon.gun = thisGun;
            damageable = enemy.AddComponent<Damageable>();
            boxCollider.size = new Vector3(100, 100, 100);
            cam = player.AddComponent<Camera>();
            shootRaycast.CameraOrigin = cam;
            Vector3 fwd = cam.transform.TransformDirection(Vector3.forward);
            enemy.transform.position = fwd + new Vector3(101, 0, 0);
            player.transform.LookAt(enemy.transform.position);
            cam.transform.LookAt(enemy.transform.position);
        }
        [UnityTest]
        public IEnumerator OnShootRaycastTest_Expect_ReducingEnemyHealth()
        {
            //Act
            shootRaycast.OnShootRaycast(thisGunData);
            yield return new WaitForSeconds(0.5f);
            //Assert
            Assert.AreEqual(85, damageable.Health);
        }
    }
}