using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManagementNS
{
    public enum AfterLoadingMode
    {
        GameObjectMode = 0, SceneMode
    }
    public class AfterLoading : MonoBehaviour
    {
        //By default gameobject is disabled 
        [HideInInspector]
        public bool EnableGameobjectAfterMoving = false;
        [HideInInspector]
        public AfterLoadingMode AfterLoadingMode;
        private bool isFirstUpdate = true;
        private AsyncOperation unloadLoading;
        private void Update()
        {
            if (isFirstUpdate)
            {
                unloadLoading = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(MyConstants.SceneConstants.LOADING);
                isFirstUpdate = false;
            }
            while (unloadLoading.progress < 0.9)
                return;
            switch (AfterLoadingMode)
            {
                case AfterLoadingMode.GameObjectMode:
                    this.gameObject.SetActive(EnableGameobjectAfterMoving);
                    Destroy(this);
                    break;
                case AfterLoadingMode.SceneMode:
                    foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (go.name == MyConstants.SceneConstants.GAMEPLAY)
                            if (go.transform.Find(MyConstants.CommonConstants.MAIN_CAMERA_PATH))
                                go.transform.Find(MyConstants.CommonConstants.MAIN_CAMERA_PATH).gameObject.SetActive(true);
                    }
                    Destroy(transform.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
