using MyConstants;
using UnityEngine;
namespace DataSaving.SceneSaving
{
    public class ProgressData : ISaveData
    {
        public string SceneName;
        public ProgressData(string sceneName)
        {
            SceneName = sceneName;
        }
    }
    public class ProgressSaving
    {
        static ProgressData loadedScene;
        public static ProgressData LoadedScene
        {
            get { return loadedScene; }
            set { loadedScene = value; }
        }

        /// <returns>True - data exists and successfully loaded. False - data is empty</returns>
        public static bool DataIsLoaded() =>
         loadedScene != null ? true : false;
        public static void SetCurrentProgressScene(string sceneName)
        {
            if (LoadedScene == null)
                LoadedScene = new ProgressData(sceneName);
            else
                LoadedScene.SceneName = sceneName;
        }
        /// <returns>True - data exists and successfully loaded. False - data is empty</returns>
        public static bool LoadProgressData()
        {
            FileDataHandler fileDataHandler = new FileDataHandler(Application.persistentDataPath, DataConstants.PROGRESS_DATA_PATH, true);
            loadedScene = fileDataHandler.Load<ProgressData>();
            return DataIsLoaded();
        }
        public static void SaveProgressData()
        {
            if (loadedScene != null)
            {
                FileDataHandler fileDataHandler = new FileDataHandler(Application.persistentDataPath, DataConstants.PROGRESS_DATA_PATH, true);
                fileDataHandler.Save(loadedScene);
            }
        }
    }
}
