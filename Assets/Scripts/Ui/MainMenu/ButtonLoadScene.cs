using ScenesManagementNS;
using SoundNS;
using System.Collections;
using UnityEngine;

namespace UINS
{
    public class ButtonLoadScene : MonoBehaviour
    {
        public AudioManager AudioManager;
        public void LoadTest(string scene) =>
          Loader.LoadWithGameplay(scene);
        public void LoadScene(string scene)
        {
            if (AudioManager != null)
                StartCoroutine(WaitSounds(scene));
            else
                Loader.Load(scene);
        }
        IEnumerator WaitSounds(string scene)
        {
            foreach (var s in AudioManager.sounds)
            {
                Debug.Log("aaa");
                if (s.source.isPlaying)
                {
                    Debug.Log("bb");
                    yield return new WaitForSeconds(0.03f);
                }
            }
            Loader.Load(scene);
        }
    }
}
