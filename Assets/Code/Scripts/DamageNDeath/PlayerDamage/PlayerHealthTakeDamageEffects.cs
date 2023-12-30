using DamageableNS;
using EffectsNS.PlayerEffects;
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
        public float DimmingWaitTime = 0.01f;
        public PlayerLook PlayerLook;
        public InputManager PlayerInputManager;
        [Header("Sounds")]
        public AudioManager PlayerAudioManager;
        public Sound[] TakeDamageSounds;
        public Sound[] DeathSounds;
        public MuteSound MuteSound;
        [SerializeField]
        private float muteTime = 10f;
        private AudioSource currentAudioSourceIsPlaying;
        private Coroutine takeDamageCoroutine;
        private Coroutine deathCoroutine;
        private BlackScreenDimming dimming;
        private PlayerHealth playerHealth;
        private void Start()
        {
            playerHealth = GetComponent<PlayerHealth>();
            playerHealth.OnTakeDamageWithDamageData += TakeDamageVisual;
            playerHealth.OnTakeDamageWithoutDamageData += TakeDamageVisual;
            playerHealth.OnTakeDamageWithoutDamageData += TakeDamageAudio;
            playerHealth.OnTakeDamageWithDamageData += TakeDamageAudio;
            playerHealth.OnDead += Death;
            if (!PlayerLook)
                PlayerLook = GetComponent<PlayerLook>();
            if (!PlayerInputManager)
                PlayerInputManager = GetComponent<InputManager>();
            dimming = GameObject.Find(HUDConstants.BLACK_SCREEN_DIMMING).GetComponent<BlackScreenDimming>();
            if (!PlayerAudioManager)
                PlayerAudioManager = GetComponent<AudioManager>();
        }
        private void OnDestroy()
        {
            playerHealth.OnTakeDamageWithDamageData -= TakeDamageVisual;
            playerHealth.OnTakeDamageWithDamageData -= TakeDamageAudio;
            playerHealth.OnDead -= Death;
            Time.timeScale = 1f;
            PlayerLook.SetLookingInputLockStats(false);
            PlayerInputManager.SetMovingLock(false);
        }
        //Play Take Damage Sounds
        public void TakeDamageAudio(TakeDamageData takeDamageData) =>
              PlaySound(TakeDamageSounds);
        public void TakeDamageAudio() =>
            PlaySound(TakeDamageSounds);
        void PlaySound(Sound[] sounds)
        {
            if (PlayerAudioManager && sounds.Length > 0)
            {
                int n = Random.Range(sounds.Length > 1 ? 1 : 0, sounds.Length);
                // Move picked sound to index 0 so it's not picked next time.
                Sound temp = sounds[n];
                sounds[n] = sounds[0];
                sounds[0] = temp;
                currentAudioSourceIsPlaying = PlayerAudioManager.CreateAndPlay(temp);
            }
        }
        public void TakeDamageVisual(TakeDamageData takeDamageData)
        {
            //Shake Camera
            if (TryGetComponent(out CameraShake cameraShake))
                if ((float)takeDamageData.Damage / 50f > 1f)
                    cameraShake.FooCameraShake(cameraShake.magnitude * ((float)takeDamageData.Damage / 50f), cameraShake.roughness);
                else
                    cameraShake.FooCameraShake();
            //Overlay and PostProcessEffect
            TakeDamageVisual();
        }
        public void TakeDamageVisual()
        {
            if (overlays != null && playerHealth.Health < playerHealth.HealthOnStart)
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
                damageEffectIntensity = 1 - (float)(playerHealth.Health / playerHealth.HealthOnStart);
                foreach (var overlay in overlays)
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.Lerp(overlay.color.a, damageEffectIntensity, time));
                PostProcessVolume.weight = Mathf.Lerp(PostProcessVolume.weight, damageEffectIntensity, time);
            }
            while (playerHealth.Health < playerHealth.HealthOnStart)
            {
                damageEffectIntensity = 1 - (float)(playerHealth.Health / playerHealth.HealthOnStart);
                foreach (var overlay in overlays)
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, damageEffectIntensity);
                PostProcessVolume.weight = damageEffectIntensity;
                yield return null;
            }
            foreach (var overlay in overlays)
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1 - (float)(playerHealth.Health / playerHealth.HealthOnStart));
            PostProcessVolume.enabled = false;
            PostProcessVolume.weight = 0;
        }
        public void Death()
        {
            playerHealth.OnTakeDamageWithDamageData -= TakeDamageAudio;
            SettingsNS.GameSettings.PlayerInput.OnFoot.EscapeMenu.Disable();
            MuteSound.MuteVolume(muteTime);
            Time.timeScale = 0.5f;
            PlayerLook.SetLookingInputLockStats(true);
            PlayerInputManager.SetMovingLock(true, true);
            PlaySound(DeathSounds);
            dimming.FadeSpeed = DimmingFadeSpeed;
            dimming.DimmingEnable(false, DimmingWaitTime);
            if (deathCoroutine != null)
                StopCoroutine(deathCoroutine);
            deathCoroutine = StartCoroutine(DeathCoroutine());
        }
        IEnumerator DeathCoroutine()
        {
            while (dimming.BlackScreen.color.a < 1 || (currentAudioSourceIsPlaying && currentAudioSourceIsPlaying.isPlaying))
                yield return null;
            SceneDeterminant sceneManager = SceneDeterminant.Instance;
            if (sceneManager)
                Loader.Load(sceneManager.GetRandomScene(sceneManager.ScenesAfterLose, sceneManager.AfterLoseScenesSpawnChances));
            else
                Loader.Load(MyConstants.SceneConstants.MAIN_MENU);
            Time.timeScale = 1f;
            //PlayerLook.SetLookingInputLockStats(false);
            //PlayerInputManager.SetMovingLock(false);
        }
    }
}