using HudNS;
using System.Collections;
using UnityEngine;
namespace ScenesManagementNS
{
    public class BackToMenuTrigger : MonoBehaviour
    {
        BlackScreenDimming bSD;
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == MyConstants.CommonConstants.PLAYER)
            {
                bSD = GameObject.Find(MyConstants.HUDConstants.BLACK_SCREEN_DIMMING).GetComponent<BlackScreenDimming>();
                bSD.fadeSpeed = 0.8f;
                bSD.DimmingEnable();
                StartCoroutine(ToTheNextLocation());
            }
        }
        IEnumerator ToTheNextLocation()
        {
            while (bSD.blackScreen.color.a < 0.9f)
                yield return null;
            SceneDeterminant sceneManager = GameObject.Find(MyConstants.SceneConstants.SCENE_MANAGER).GetComponent<SceneDeterminant>();
            if (sceneManager)
                Loader.Load((int)sceneManager.NextScene);
            else
                Loader.Load(MyConstants.SceneConstants.MAIN_MENU);
        }
    }
}