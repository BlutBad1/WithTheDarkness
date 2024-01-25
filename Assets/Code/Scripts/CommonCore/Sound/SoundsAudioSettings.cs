using System.Linq;
using UnityEngine;
using static SettingsNS.AudioSettings;

namespace SoundNS
{
    public class SoundsAudioSettings : MonoBehaviour
    {
        public AudioSourcesManager AudioSourcesManager;
        public void PlayRandomSound(AudioKind audioKind)
        {
            AudioSourceObject[] audioSourceObject =
                AudioSourcesManager.AudioSourceObjects.Where(x => x.AudioSource.outputAudioMixerGroup == MixerVolumeChanger.Instance.GetAudioMixerGroup(audioKind)).ToArray();
            AudioSourcesManager.PlayAudioSourceOnceAtTime(audioSourceObject[UnityEngine.Random.Range(0, audioSourceObject.Length)]);
        }
    }
}