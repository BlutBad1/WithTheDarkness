using HudNS;
using MyConstants;
using ScenesManagementNS;
using SoundNS;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerHealthTakeDamageEffects : MonoBehaviour
    {
        [Header("Visual")]
        public Image[] overlays;
        public PostProcessVolume PostProcessVolume;
        public float DimmingFadeSpeed = 3f;
        [Header("Sounds")]
        public Sound[] TakeDamageSounds;
        public Sound[] DeathSounds;
        public MuteSound MuteSound;
        [SerializeField]
        float muteTime = 10f;
        private AudioSource currentAudioSourceIsPlaying;
        private Coroutine takeDamageCoroutine;
        private Coroutine deathCoroutine;
        private BlackScreenDimming dimming;
        private PlayerHealth playerHealth;
        private void Start()
        {
            playerHealth = GetComponent<PlayerHealth>();
            playerHealth.OnTakeDamage += TakeDamageVisual;
            playerHealth.OnTakeDamage += TakeDamageAudio;
            playerHealth.OnDeath += Death;
            dimming = GameObject.Find(HUDConstants.BLACK_SCREEN_DIMMING).GetComponent<BlackScreenDimming>();
        }
        private void OnDestroy()
        {
            playerHealth.OnTakeDamage -= TakeDamageVisual;
            playerHealth.OnTakeDamage -= TakeDamageAudio;
            playerHealth.OnDeath -= Death;
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
            //Overlay and PostProcessEffect
            if (overlays != null && playerHealth.Health < playerHealth.OriginalHealth)
            {
                if (takeDamageCoroutine != null)
                    StopCoroutine(takeDamageCoroutine);
                takeDamageCoroutine = StartCoroutine(TakeDamageOverlayCoroutine());
            }
        }
        IEnumerator TakeDamageOverlayCoroutine()
        {
            float damageEffectIntensity = 0;
            PostProcessVolume.enabled = true;
            for (float time = 0; time < 1; time += Time.deltaTime)
            {
                damageEffectIntensity = 1 - (float)(playerHealth.Health / playerHealth.OriginalHealth);
                foreach (var overlay in overlays)
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.Lerp(overlay.color.a, damageEffectIntensity, time));
                PostProcessVolume.weight = Mathf.Lerp(PostProcessVolume.weight, damageEffectIntensity, time);
            }
            while (playerHealth.Health < playerHealth.OriginalHealth)
            {
                damageEffectIntensity = 1 - (float)(playerHealth.Health / playerHealth.OriginalHealth);
                foreach (var overlay in overlays)
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, damageEffectIntensity);
                PostProcessVolume.weight = damageEffectIntensity;
                yield return null;
            }
            foreach (var overlay in overlays)
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1 - (float)(playerHealth.Health / playerHealth.OriginalHealth));
            PostProcessVolume.enabled = false;
            PostProcessVolume.weight = 0;
        }
        public void Death()
        {
            playerHealth.OnTakeDamage -= TakeDamageAudio;
            SettingsNS.GameSettings.PlayerInput.OnFoot.EscapeMenu.Disable();
            MuteSound.MuteVolume(muteTime);
            PlaySound(DeathSounds);
            dimming.fadeSpeed = DimmingFadeSpeed;
            dimming.DimmingEnable();
            if (deathCoroutine != null)
                StopCoroutine(deathCoroutine);
            deathCoroutine = StartCoroutine(DeathCoroutine());
        }
        IEnumerator DeathCoroutine()
        {
            while (dimming.blackScreen.color.a < 1 || (currentAudioSourceIsPlaying && currentAudioSourceIsPlaying.isPlaying))
                yield return null;
            SceneDeterminant sceneManager = GameObject.Find(MyConstants.SceneConstants.PROGRESS_MANAGER).GetComponent<SceneDeterminant>();
            if (sceneManager)
                Loader.Load((int)sceneManager.SceneAfterLose);
            else
                Loader.Load(MyConstants.SceneConstants.MAIN_MENU);
        }
    }
}