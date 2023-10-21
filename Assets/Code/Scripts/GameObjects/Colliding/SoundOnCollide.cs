using SoundNS;
using UnityEngine;
namespace GameObjectsControllingNS.ColliderControlling
{
    [RequireComponent(typeof(Rigidbody))]
    public class SoundOnCollide : MonoBehaviour
    {
        public AudioSourcesManager AudioSourcesManager;
        public float VelocityOfMaximumSound = 1f;
        protected Rigidbody rigidbody;
        private Vector3 lastFrameVelocity;
        private float lastSoundTime = 0;
        private void Start()
        {
            if (!AudioSourcesManager)
                AudioSourcesManager = GetComponent<AudioSourcesManager>();
            rigidbody = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            // Cache the velocity every frame
            lastFrameVelocity = rigidbody.velocity;
            lastSoundTime += Time.deltaTime;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (lastSoundTime > 0.05f)
            {
                lastSoundTime = 0;
                AudioSourceObject audioSourceObject = AudioSourcesManager.AudioSourceObjects[
                   Random.Range(0, AudioSourcesManager.AudioSourceObjects.Length)];
                float currentVolume = audioSourceObject.AudioSource.volume;
                audioSourceObject.AudioSource.volume = Mathf.Lerp(0, audioSourceObject.AudioSource.volume, lastFrameVelocity.magnitude / VelocityOfMaximumSound);
                if (AudioSourcesManager)
                    AudioSourcesManager.CreateNewAudioSourceAndPlay(audioSourceObject);
                audioSourceObject.AudioSource.volume = currentVolume;
            }
        }
    }
}
