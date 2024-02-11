using SceneConstantsNS;
using SerializableTypes;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace ScenesManagementNS
{
	public class SceneDeterminant : MonoBehaviour, ISerializationCallbackReceiver
	{
		public static List<string> SceneNames; // can't be a prop, because ListToMultiplePopup

		public static SceneDeterminant Instance { get; private set; }

		[SerializeField, ListToMultiplePopup(typeof(SceneDeterminant), "SceneNames"), FormerlySerializedAs("NextScenes")]
		private int nextScenes;
		[SerializeField, ListToMultiplePopup(typeof(SceneDeterminant), "SceneNames"), FormerlySerializedAs("ScenesAfterLose")]
		private int scenesAfterLose;

		public SerializableDictionary<string, float> NextScenesSpawnChances { get; private set; }
		public SerializableDictionary<string, float> AfterLoseScenesSpawnChances { get; private set; }
		public int ScenesAfterLose { get => scenesAfterLose; private set => scenesAfterLose = value; }
		public int NextScenes { get => nextScenes; private set => nextScenes = value; }

		private void Start()
		{
			if (Instance == null)
				Instance = this;
			else
				Destroy(this);
		}
		public void OnAfterDeserialize()
		{
		}
		public void OnBeforeSerialize()
		{
			InitializeAndCheckCollectionOfChances(NextScenesSpawnChances);
			InitializeAndCheckCollectionOfChances(AfterLoseScenesSpawnChances);
			SceneNames = GetAllScenesInBuild();
			ScenesAfterLose = ScenesAfterLose == 0 ? 1 : ScenesAfterLose;
			NextScenes = NextScenes == 0 ? 1 : NextScenes;
		}
		public List<string> GetAllScenesInBuild()
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
		public string GetRandomNextScene() =>
			GetRandomScene(NextScenes, NextScenesSpawnChances);
		public string GetRandomLoseScene() =>
			GetRandomScene(ScenesAfterLose, AfterLoseScenesSpawnChances);
		private string GetRandomScene(int SceneMask, SerializableDictionary<string, float> collection)
		{
			string scene = "";
			Dictionary<string, float> notZeroChancesList = collection.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
			int mutualSpawnChance = (int)notZeroChancesList.Values.Sum();
			int randomNumber = UnityEngine.Random.Range(1, mutualSpawnChance + 1);
			float spawnChanceCounter = 0;
			foreach (var spawningGameObject in notZeroChancesList)
			{
				if (randomNumber > spawnChanceCounter && randomNumber <= spawnChanceCounter + spawningGameObject.Value)
				{
					scene = spawningGameObject.Key;
					break;
				}
				spawnChanceCounter += spawningGameObject.Value;
			}
			if (string.IsNullOrEmpty(scene))
				return SceneConstants.MAIN_MENU;
			else
				return scene;
		}
		private void InitializeAndCheckCollectionOfChances(SerializableDictionary<string, float> collection)
		{
			if (collection == null || collection.Count != GetAllScenesInBuild().Count)
			{
				if (collection == null)
					collection = new SerializableDictionary<string, float>();
				foreach (var scene in GetAllScenesInBuild())
				{
					if (!collection.ContainsKey(scene))
						collection.Add(scene, 0);
				}
				foreach (var scene in collection.Keys.Where(x => !GetAllScenesInBuild().Contains(x)).ToList())
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
				for (int i = 0; i < script.GetAllScenesInBuild().Count; i++)
				{
					if ((mask & (1 << i)) != 0)
					{
						scene = script.GetAllScenesInBuild()[i];
						collection[scene] = EditorGUILayout.Slider(scene, collection[scene], 0f, 100f);
					}
				}
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
		}
	}
#endif
}