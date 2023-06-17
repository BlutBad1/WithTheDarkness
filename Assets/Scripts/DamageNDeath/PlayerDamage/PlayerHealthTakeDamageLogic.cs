using HudNS;
using MyConstants;
using ScenesManagementNS;
using SoundNS;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerHealthTakeDamageLogic : MonoBehaviour
    {
        [Header("Overlay")]
        public Image overlay;
        [Header("Sounds")]
        public Sound[] TakeDamageSounds;
        public Sound[] DeathSounds;
        private AudioSource currentAudioSourceIsPlaying;
        private Coroutine takeDamageCoroutine;
        private Coroutine deathCoroutine;
        private BlackScreenDimming dimming;
        private void Start()
        {
            GetComponent<PlayerHealth>().OnTakeDamage += TakeDamageVisual;
            GetComponent<PlayerHealth>().OnTakeDamage += TakeDamageAudio;
            GetComponent<PlayerHealth>().OnDeath += Death;
            dimming = GameObject.Find(HUDConstants.BLACK_SCREEN_DIMMING).GetComponent<BlackScreenDimming>();
        }
        private void OnDestroy()
        {
            GetComponent<PlayerHealth>().OnTakeDamage -= TakeDamageVisual;
            GetComponent<PlayerHealth>().OnTakeDamage -= TakeDamageAudio;
            GetComponent<PlayerHealth>().OnDeath -= Death;
        }

        //Play Take Damage Sound
        public void TakeDamageAudio(float damage, float force, Vector3 hit) =>
              PlaySound(TakeDamageSounds);

        void PlaySound(Sound[] sounds)
        {
            if (TryGetComponent(out AudioManager audioManager) && sounds.Length > 0)
            {
                int n = Random.Range(sounds.Length > 1 ? 1 : 0, sounds.Length);
                // Move picked sound to index 0 so it's not picked next time.
                Sound temp = sounds[n];
                sounds[n] = sounds[0];
                sounds[0] = temp;
                currentAudioSourceIsPlaying = audioManager.CreateAndPlay(temp);
            }
        }
        public void TakeDamageVisual(float damage, float force, Vector3 hit)
        {
            //Shake Camera
            if (TryGetComponent(out CameraShake cameraShake))
                if ((float)damage / 50f > 1f)
                    cameraShake.FooCameraShake(cameraShake.magnitude * ((float)damage / 50f), cameraShake.roughness);
                else
                    cameraShake.FooCameraShake();
            //Appear overlay
            if (overlay != null && GetComponent<PlayerHealth>().Health < 100)
            {
                if (takeDamageCoroutine != null)
                    StopCoroutine(takeDamageCoroutine);
                takeDamageCoroutine = StartCoroutine(TakeDamageOverlayCoroutine());
            }
        }
        IEnumerator TakeDamageOverlayCoroutine()
        {
            for (float time = 0; time < 1; time += Time.deltaTime)
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.Lerp(overlay.color.a, (100 - (float)GetComponent<PlayerHealth>().Health), time));
            while (GetComponent<PlayerHealth>().Health < 100)
            {
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, (100 - (float)GetComponent<PlayerHealth>().Health) / 100);
                yield return null;
            }
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, (100 - (float)GetComponent<PlayerHealth>().Health) / 100);
        }
        public void Death()
        {
            GetComponent<PlayerHealth>().OnTakeDamage -= TakeDamageAudio;
            PlaySound(DeathSounds);
            dimming.fadeSpeed = 3f;
            dimming.DimmingEnable();
            if (deathCoroutine != null)
                StopCoroutine(deathCoroutine);
            deathCoroutine = StartCoroutine(DeathCoroutine());
        }
        IEnumerator DeathCoroutine()
        {
            while (dimming.blackScreen.color.a < 1 || (currentAudioSourceIsPlaying && currentAudioSourceIsPlaying.isPlaying))
                yield return null;
            SceneDeterminant sceneManager = GameObject.Find(MyConstants.SceneConstants.SCENE_MANAGER).GetComponent<SceneDeterminant>();
            if (sceneManager)
                Loader.Load((int)sceneManager.SceneAfterLose);
            else
                Loader.Load(MyConstants.SceneConstants.MAIN_MENU);
        }
    }
}