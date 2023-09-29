using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DataSaving.SceneSaving
{
    public class SaveCurrentScene : MonoBehaviour
    {
        public bool SaveOnStart = false;
        [HideInInspector]
        public bool UseThisSceneName = true;
        [HideInInspector]
        public string SceneName;
        private void Start()
        {
            if (SaveOnStart)
                SaveScene();
        }
        public void SaveScene()
        {
            string sceneName;
            if (UseThisSceneName)
                sceneName = SceneManager.GetActiveScene().name;
            else
                sceneName = SceneName;
            ProgressSaving.SetCurrentProgressScene(sceneName);
            ProgressSaving.SaveProgressData();
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(SaveCurrentScene))]
    public class SaveCurrentScene_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            SaveCurrentScene script = (SaveCurrentScene)target;
            // draw checkbox for the bool
            script.UseThisSceneName = EditorGUILayout.Toggle("Use This Scene Name", script.UseThisSceneName);
            if (!script.UseThisSceneName) // if bool is false, show other fields
                script.SceneName = EditorGUILayout.TextField("Scene Name", script.SceneName);
        }
    }
#endif
}