using SoundNS;
using UnityEngine;
namespace GameObjectsControllingNS.ColliderControlling
{
    public class SoundOnCollide : MonoBehaviour
    {
        public AudioSourcesManager AudioSourcesManager;
        private void Start()
        {
            if (!AudioSourcesManager)
                AudioSourcesManager = GetComponent<AudioSourcesManager>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (AudioSourcesManager)
                AudioSourcesManager.CreateNewAudioSourceAndPlay(AudioSourcesManager.AudioSourceObjects[
                    Random.Range(0, AudioSourcesManager.AudioSourceObjects.Length)].Name);
        }
    }
}
