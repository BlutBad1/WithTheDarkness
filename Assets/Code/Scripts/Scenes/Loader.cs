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
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING);
            onLoaderCallback = () => { UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene); };
        }
        public static void Load(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING);
            onLoaderCallback = () => { UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex); };
        }
        public static void Load(string scene, GameObject gameObjectToMove, bool enableAfterMoving)
        {
            onLoaderCallback = null;
            gameObjectToMove.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING, LoadSceneMode.Additive);
            onLoaderCallback = () => { LoaderCallbackInstance.StartCoroutine(LoadAndMoveGameObject(scene, gameObjectToMove, enableAfterMoving)); };
        }

        private static IEnumerator LoadAndMoveGameObject(string newSceneName, GameObject gameObjectToMove, bool enableAfterMoving)
        {
            if (gameObjectToMove)
            {
                Scene scneToUnload = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                AsyncOperation s = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
                s.allowSceneActivation = false;
                while (s.progress < 0.9f)
                    yield return null;
                Scene sceneToLoad = UnityEngine.SceneManagement.SceneManager.GetSceneByName(newSceneName);
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObjectToMove, sceneToLoad);
                AfterLoading afterMoving = gameObjectToMove.AddComponent<AfterLoading>();
                afterMoving.EnableGameobjectAfterMoving = enableAfterMoving;
                afterMoving.AfterLoadingMode = AfterLoadingMode.GameObjectMode;
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scneToUnload);
                s.allowSceneActivation = true;
                gameObjectToMove.SetActive(true);
                // SceneManager.SetActiveScene(sceneToLoad);
            }
        }
        public static void LoadWithGameplay(string scene)
        {
            bool gameplayLoaded = false;
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
                if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name == MyConstants.SceneConstants.GAMEPLAY)
                    gameplayLoaded = true;
            if (!gameplayLoaded)
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING);
            else
            {
                if (GameObject.Find(MyConstants.CommonConstants.MAIN_CAMERA_PATH).gameObject)
                    GameObject.Find(MyConstants.CommonConstants.MAIN_CAMERA_PATH).gameObject.SetActive(false);
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MyConstants.SceneConstants.LOADING, LoadSceneMode.Additive);
            }
            onLoaderCallback = () => { LoaderCallbackInstance.StartCoroutine(LoadWithGameplayCoroutine(scene, gameplayLoaded)); };
        }
        private static IEnumerator LoadWithGameplayCoroutine(string scene, bool gameplayLoaded)
        {
            AsyncOperation gameplay;
            if (!gameplayLoaded)
            {
                gameplay = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MyConstants.SceneConstants.GAMEPLAY, LoadSceneMode.Additive);
                while (gameplay.progress < 0.9)
                    yield return null;
            }
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(MyConstants.SceneConstants.GAMEPLAY));
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
                if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name != MyConstants.SceneConstants.GAMEPLAY && UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name != MyConstants.SceneConstants.LOADING)
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i));
            AsyncOperation s = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
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