using UnityEngine;

namespace ScenesManagementNS
{
    public class SceneDeterminant : MonoBehaviour
    {
        public MyConstants.SceneConstants.AvailableScenes NextScene;
        public MyConstants.SceneConstants.AvailableScenes SceneAfterLose;
        SceneDeterminant instance;
        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
    }
}