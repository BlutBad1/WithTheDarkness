using UnityEngine;

namespace EffectsNS.PlayerEffects
{
    public class BlackScreenEnabler : MonoBehaviour
    {
        public void EnableBlackScreen(float fadeSpeed)
        {
            BlackScreenDimming[] blackScreenDimmings = FindObjectsOfType<BlackScreenDimming>();
            foreach (BlackScreenDimming blackScreenDimming in blackScreenDimmings)
                blackScreenDimming.DimmingEnable(fadeSpeed);
        }
        public void DisableBlackScreen(float fadeSpeed)
        {
            BlackScreenDimming[] blackScreenDimmings = FindObjectsOfType<BlackScreenDimming>();
            foreach (BlackScreenDimming blackScreenDimming in blackScreenDimmings)
                blackScreenDimming.DimmingDisable(fadeSpeed);
        }
    }
}