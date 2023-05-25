using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManagementNS
{
    public class Loader
    {
        public static LoaderCallback LoaderCallbackInstance;
        private static Action onLoaderCallback;

        public static void Load(string scene)
        {
            SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING);
            onLoaderCallback = () => { SceneManager.LoadSceneAsync(scene); };
        }
        public static void Load(string scene, GameObject gameObjectToMove, bool enableAfterMoving)
        {
            onLoaderCallback = null;
            gameObjectToMove.SetActive(false);
            SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING, LoadSceneMode.Additive);
            onLoaderCallback = () => { LoaderCallbackInstance.StartCoroutine(LoadAndMoveGameObject(scene, gameObjectToMove, enableAfterMoving)); };
        }

        private static IEnumerator LoadAndMoveGameObject(string newSceneName, GameObject gameObjectToMove, bool enableAfterMoving)
        {
            if (gameObjectToMove)
            {
                Scene scneToUnload = SceneManager.GetActiveScene();
                AsyncOperation s = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
                s.allowSceneActivation = false;
                while (s.progress < 0.9f)
                    yield return null;
                Scene sceneToLoad = SceneManager.GetSceneByName(newSceneName);
                SceneManager.MoveGameObjectToScene(gameObjectToMove, sceneToLoad);
                AfterLoading afterMoving = gameObjectToMove.AddComponent<AfterLoading>();
                afterMoving.EnableGameobjectAfterMoving = enableAfterMoving;
                afterMoving.AfterLoadingMode = AfterLoadingMode.GameObjectMode;
                SceneManager.UnloadSceneAsync(scneToUnload);
                s.allowSceneActivation = true;
                gameObjectToMove.SetActive(true);
                // SceneManager.SetActiveScene(sceneToLoad);
            }
        }
        public static void LoadWithGameplay(string scene)
        {
            bool gameplayLoaded = false;
            for (int i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).name == MyConstants.SceneConstants.GAMEPLAY)
                    gameplayLoaded = true;
            if (!gameplayLoaded)
                SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING);
            else
            {
                if (GameObject.Find(MyConstants.CommonConstants.MAIN_CAMERA_PATH).gameObject)
                    GameObject.Find(MyConstants.CommonConstants.MAIN_CAMERA_PATH).gameObject.SetActive(false);
                SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING, LoadSceneMode.Additive);
            }
            onLoaderCallback = () => { LoaderCallbackInstance.StartCoroutine(LoadWithGameplayCoroutine(scene, gameplayLoaded)); };
        }
        private static IEnumerator LoadWithGameplayCoroutine(string scene, bool gameplayLoaded)
        {
            AsyncOperation gameplay;
            if (!gameplayLoaded)
            {
                gameplay = SceneManager.LoadSceneAsync(MyConstants.SceneConstants.GAMEPLAY, LoadSceneMode.Additive);
                while (gameplay.progress < 0.9)
                    yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MyConstants.SceneConstants.GAMEPLAY));
            for (int i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).name != MyConstants.SceneConstants.GAMEPLAY && SceneManager.GetSceneAt(i).name != MyConstants.SceneConstants.LOADING)
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            AsyncOperation s = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (s.progress < 0.9)
                yield return null;
            AfterLoading afterMoving = new GameObject().AddComponent<AfterLoading>();
            afterMoving.AfterLoadingMode = AfterLoadingMode.SceneMode;
        }
        public static void LoaderCallback()
        {
            if (onLoaderCallback != null)
            {
                onLoaderCallback();
                onLoaderCallback = null;
            }
        }
    }
}