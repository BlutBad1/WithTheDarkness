using SerializableTypes;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManagementNS
{
    public class SceneDeterminant : MonoBehaviour, ISerializationCallbackReceiver
    {
        [HideInInspector]
        public static List<string> SceneNames;
        [Tooltip("One from mask will be chosen"), ListToMultiplePopup(typeof(SceneDeterminant), "SceneNames")]
        public int NextScenes;
        [Tooltip("One from mask will be chosen"), ListToMultiplePopup(typeof(SceneDeterminant), "SceneNames")]
        public int ScenesAfterLose;
        [HideInInspector, SerializeField]
        public SerializableDictionary<string, float> NextScenesSpawnChances;
        [HideInInspector, SerializeField]
        public SerializableDictionary<string, float> AfterLoseScenesSpawnChances;
        private SceneDeterminant instance;
        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
        /// <summary>
        /// If scenes chosen in scenemask, last scene always has 100%, otherwise it will return main menu scene.
        /// </summary>
        /// <param name="SceneMask"></param>
        /// <returns></returns>
        public string GetRandomScene(int SceneMask, SerializableDictionary<string, float> collection)
        {
            string scene = "";
            for (int i = 0; i < GetAllScenesInBuld().Count; i++)
            {
                if ((SceneMask & (1 << i)) != 0)
                {
                    if (collection[GetAllScenesInBuld()[i]] > Random.Range(0, 100))
                        return GetAllScenesInBuld()[i];
                    scene = GetAllScenesInBuld()[i];
                }
            }
            if (string.IsNullOrEmpty(scene))
                return MyConstants.SceneConstants.MAIN_MENU;
            else
                return scene;
        }
        public List<string> GetAllScenesInBuld()
        {
            List<string> SceneList = new List<string>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string SceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                SceneList.Add(SceneName);
            }
            return SceneList;
        }
        public void OnAfterDeserialize()
        {
        }
        public void OnBeforeSerialize()
        {
            InitializeAndCheckCollectionOfChances(NextScenesSpawnChances);
            InitializeAndCheckCollectionOfChances(AfterLoseScenesSpawnChances);
            SceneNames = GetAllScenesInBuld();
            ScenesAfterLose = ScenesAfterLose == 0 ? 1 : ScenesAfterLose;
            NextScenes = NextScenes == 0 ? 1 : NextScenes;
        }
        private void InitializeAndCheckCollectionOfChances(SerializableDictionary<string, float> collection)
        {
            if (collection == null || collection.Count != GetAllScenesInBuld().Count)
            {
                if (collection == null)
                    collection = new SerializableDictionary<string, float>();
                foreach (var scene in GetAllScenesInBuld())
                {
                    if (!collection.ContainsKey(scene))
                        collection.Add(scene, 0);
                }
                foreach (var scene in collection.Keys.Where(x => !GetAllScenesInBuld().Contains(x)).ToList())
                    collection.Remove(scene);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(SceneDeterminant))]
    public class SceneDeterminantEditor : Editor
    {
        bool visibleNextScenes = true;
        bool visibleAfterLoseScenes = true;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SceneDeterminant script = (SceneDeterminant)target;
            DrawCollectionOfChances(ref visibleNextScenes, "Next Scenes Chances", script, script.NextScenesSpawnChances, script.NextScenes);
            DrawCollectionOfChances(ref visibleAfterLoseScenes, "After Lose Scenes Spawn Chances", script, script.AfterLoseScenesSpawnChances, script.ScenesAfterLose);
            Undo.RecordObject(target, "Changed Scene Spawn Chance");
        }
        private void DrawCollectionOfChances(ref bool visible, string label, SceneDeterminant script, SerializableDictionary<string, float> collection, int mask)
        {
            visible = EditorGUILayout.BeginFoldoutHeaderGroup(visible, label);
            if (visible)
            {
                string scene;
                for (int i = 0; i < script.GetAllScenesInBuld().Count; i++)
                {
                    if ((mask & (1 << i)) != 0)
                    {
                        scene = script.GetAllScenesInBuld()[i];
                        collection[scene] = EditorGUILayout.Slider(scene, collection[scene], 0f, 100f);
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
#endif
}