using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManagementNS
{
    public class AfterMoving : MonoBehaviour
    {
        //By default gameobject is disabled 
        [HideInInspector]
        public bool EnableAfterMoving = false;
        private void Update()
        {
            SceneManager.UnloadSceneAsync(MyConstants.SceneConstants.LOADING);
            this.gameObject.SetActive(EnableAfterMoving);
            Destroy(this);
        } 
    }
}
