using LocationManagementNS;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Map
{


    public class GameObjectsManagerTests
    {
        GameObject player;
        GameObject mainGameObject;
        GameObject childGameObject;
        GameObjectsManager gameObjectsManager;
        [SetUp]
        public void Setup()
        {
            player = GameObject.Instantiate(new GameObject());
            player.name = MyConstants.CommonConstants.PLAYER;
            mainGameObject = GameObject.Instantiate(new GameObject());
            childGameObject = GameObject.Instantiate(new GameObject());
            gameObjectsManager = mainGameObject.AddComponent<GameObjectsManager>();
            gameObjectsManager.SpawnDistance = -1f;
            childGameObject.transform.parent = mainGameObject.transform;
            gameObjectsManager.GameObjects = new GameObject[1];
            gameObjectsManager.GameObjects[0] = childGameObject;


        }

        [UnityTest]
        public IEnumerator StartTest_Expect_ChildGameObjectDisabled()
        {
            //Act+Assert
            Assert.IsTrue(childGameObject.active == false);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Update_Expect_ChildGameObjectEnabled()
        {
            //Act+Assert
          
            yield return new WaitForSeconds(0.1f);
            gameObjectsManager.SpawnDistance = 1f;
            yield return new WaitForSeconds(0.1f);
            Assert.IsTrue(childGameObject.active == true);
        }
    }
}