using SceneConstantsNS;
using UnityEngine;

namespace ScenesManagementNS
{
	public class AfterLoading : MonoBehaviour
	{
		private bool isFirstUpdate = true;

		private void Update()
		{
			if (isFirstUpdate)
			{
				UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SceneConstants.LOADING);
				isFirstUpdate = false;
				enabled = false;
			}
		}
	}
}
