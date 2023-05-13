using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManagementNS
{
    public class Loader
    {
        private static Action onLoaderCallback;
        private static string lastSceneName;
        private static GameObject gameObjectToMove;
        private static bool enableAfterMoving = false;
        public static void Load(string scene)
        {
            SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING);
            onLoaderCallback = () => { SceneManager.LoadSceneAsync(scene); };
        }
        public static void Load(string scene, GameObject gameObjectToMove)
        {
            onLoaderCallback = null;
            Loader.gameObjectToMove = gameObjectToMove;
            lastSceneName = scene;
            gameObjectToMove.SetActive(false);
            //foreach (var item in Camera.allCameras)
            //    item.gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING, LoadSceneMode.Additive);
        }
        //By default gameobject is disabled 
        public static void Load(string scene, GameObject gameObjectToMove, bool enableAfterMoving)
        {
            Loader.enableAfterMoving = enableAfterMoving;
            Load(scene, gameObjectToMove);
        }
        public static IEnumerator LoadAndMoveGameObject()
        {
            if (gameObjectToMove)
            {
                Scene scneToUnload = SceneManager.GetActiveScene();
                AsyncOperation s = SceneManager.LoadSceneAsync(lastSceneName, LoadSceneMode.Additive);
                s.allowSceneActivation = false;
                while (s.progress < 0.9f)
                    yield return null;
                Scene sceneToLoad = SceneManager.GetSceneByName(lastSceneName);
                SceneManager.MoveGameObjectToScene(gameObjectToMove, sceneToLoad);
                gameObjectToMove.AddComponent<AfterMoving>().EnableAfterMoving = enableAfterMoving;
                SceneManager.UnloadSceneAsync(scneToUnload);
                s.allowSceneActivation = true;
                gameObjectToMove.SetActive(true);
                gameObjectToMove = null;
                // SceneManager.SetActiveScene(sceneToLoad);

            }
        }


        public static bool LoaderCallback()
        {
            if (onLoaderCallback != null)
            {
                onLoaderCallback();
                onLoaderCallback = null;
                return true;
            }
            return false;
        }
    }
}