using System.Collections;
using UnityEngine;
using HudNS;
namespace ScenesManagementNS
{
    public class BackToMenuTrigger : MonoBehaviour
    {
        BlackScreenDimming bSD;
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == MyConstants.CommonConstants.PLAYER)
            {
                bSD = GameObject.Find(MyConstants.CommonConstants.BLACK_SCREEN_DIMMING).GetComponent<BlackScreenDimming>();
                bSD.fadeSpeed = 0.8f;
                bSD.DimmingEnable();
                StartCoroutine(BackToMenu());
            }
        }
        IEnumerator BackToMenu()
        {
            while (bSD.blackScreen.color.a < 0.9f)
                yield return null;

            Loader.Load(MyConstants.LocationsConstants.MAIN_MENU);
        }
    }
}