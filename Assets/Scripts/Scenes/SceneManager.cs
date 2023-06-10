using UnityEngine;

namespace ScenesManagementNS
{
    public class SceneManager : MonoBehaviour
    {
        public MyConstants.SceneConstants.AvailableScenes NextScene;
        public MyConstants.SceneConstants.AvailableScenes SceneAfterLose;
        SceneManager instance;
        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
    }
}