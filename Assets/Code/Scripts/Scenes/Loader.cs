using SceneConstantsNS;
using System;
using UnityEngine.SceneManagement;

namespace ScenesManagementNS
{
	public static class Loader
	{
		private const string LOADING_SCEEN = SceneConstants.LOADING;

		private static Action onLoaderCallback;

		public static LoaderCallback LoaderCallbackInstance1 { get; set; }

		public static string NameFromIndex(int BuildIndex)
		{
			string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
			int slash = path.LastIndexOf('/');
			string name = path.Substring(slash + 1);
			int dot = name.LastIndexOf('.');
			return name.Substring(0, dot);
		}
		public static void Load(string scene)
		{
			SceneManager.LoadSceneAsync(LOADING_SCEEN);
			onLoaderCallback = () => { SceneManager.LoadSceneAsync(scene); };
		}
		public static void Load(int sceneIndex)
		{
			SceneManager.LoadSceneAsync(LOADING_SCEEN);
			onLoaderCallback = () => { SceneManager.LoadSceneAsync(sceneIndex); };
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