using DamageableNS;
using EffectsNS.PlayerEffects;
using SceneConstantsNS;
using ScenesManagementNS;
using SoundNS;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerDamageableEvents : MonoBehaviour
    {
        [Header("Visual"), SerializeField]
        private Image[] overlays;
        [SerializeField]
        private PostProcessVolume postProcessVolume;
        [SerializeField]
        private BlackScreenDimming blackScreenDimming;
        [SerializeField]
        private float dimmingFadeSpeed = 0.5f;
        [SerializeField]
        private float dimmingWaitTime = 0.001f;
        [SerializeField]
        private PlayerLook playerLook;
        [SerializeField]
        private InputManager playerInputManager;
        [Header("Sounds"), SerializeField]
        private AudioSourcesManager takeDamageAudioSourcesManager;
        [SerializeField]
        private AudioSourcesManager deadAudioSourcesManager;
        [SerializeField]
        private MuteSound muteSound;
        [SerializeField]
        private float muteTime = 10f;

        private Coroutine takeDamageCoroutine;
        private Coroutine deathCoroutine;
        private PlayerHealth playerHealth;

        private void Start()
        {
            playerHealth = GetComponent<PlayerHealth>();
            playerHealth.OnTakeDamageWithDamageData += OnPlayerTakeDamage;
            playerHealth.OnTakeDamageWithoutDamageData += OnPlayerTakeDamageWithoutData;
            playerHealth.OnDead += OnPlayerDead;
        }
        private void OnDestroy()
        {
            playerHealth.OnTakeDamageWithDamageData -= OnPlayerTakeDamage;
            playerHealth.OnDead -= OnPlayerDead;
            playerLook.SetLookingInputLockStats(false);
            playerInputManager.SetMovingLock(false);
            Time.timeScale = 1f;
        }
        public void OnPlayerTakeDamage(TakeDamageData takeDamageData)
        {
            if (TryGetComponent(out CameraShake cameraShake))
                if ((float)takeDamageData.Damage / 50f > 1f)
                    cameraShake.FooCameraShake(cameraShake.Magnitude * ((float)takeDamageData.Damage / 50f), cameraShake.Roughness);
                else
                    cameraShake.FooCameraShake();
            OnPlayerTakeDamageWithoutData();
        }
        public void OnPlayerTakeDamageWithoutData()
        {
            if (overlays != null && playerHealth.Health < playerHealth.HealthOnStart)
            {
                if (takeDamageCoroutine != null)
                    StopCoroutine(takeDamageCoroutine);
                takeDamageCoroutine = StartCoroutine(TakeDamageOverlayCoroutine());
            }
            OnTakeDamageAudio();
        }
        public void OnTakeDamageAudio()
        {
            AudioSourceObject randomAudioSourceObject = takeDamageAudioSourcesManager.GetRandomSound();
            takeDamageAudioSourcesManager.CreateNewAudioSourceAndPlay(randomAudioSourceObject);
        }
        public void OnPlayerDead()
        {
            SettingsNS.GameSettings.PlayerInput.OnFoot.EscapeMenu.Disable();
            muteSound.MuteVolume(muteTime);
            playerLook.SetLookingInputLockStats(true);
            playerInputManager.SetMovingLock(true, true);
            //OnDeadAudio();
            blackScreenDimming.DimmingEnable(dimmingFadeSpeed, false, dimmingWaitTime);
            if (deathCoroutine != null)
                StopCoroutine(deathCoroutine);
            deathCoroutine = StartCoroutine(DeathCoroutine());
        }
        public void OnDeadAudio()
        {
            AudioSourceObject randomAudioSourceObject = deadAudioSourcesManager.GetRandomSound();
            deadAudioSourcesManager.CreateNewAudioSourceAndPlay(randomAudioSourceObject);
        }
        private IEnumerator TakeDamageOverlayCoroutine()
        {
            EnableAndLerpWeightPostProccesVolume();
            while (playerHealth.Health < playerHealth.HealthOnStart)
            {
                SetOverlayAlphaByCurrentHealthPercent();
                yield return null;
            }
            foreach (var overlay in overlays)
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
            postProcessVolume.enabled = false;
            postProcessVolume.weight = 0;
        }
        private void EnableAndLerpWeightPostProccesVolume()
        {
            float healthRatio;
            postProcessVolume.enabled = true;
            for (float time = 0; time < 1; time += Time.deltaTime)
            {
                healthRatio = 1 - (float)(playerHealth.Health / playerHealth.HealthOnStart);
                foreach (var overlay in overlays)
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.Lerp(overlay.color.a, healthRatio, time));
                postProcessVolume.weight = Mathf.Lerp(postProcessVolume.weight, healthRatio, time);
            }
        }
        private void SetOverlayAlphaByCurrentHealthPercent()
        {
            float healthRatio = 1 - (float)(playerHealth.Health / playerHealth.HealthOnStart);
            foreach (var overlay in overlays)
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, healthRatio);
            postProcessVolume.weight = healthRatio;
        }
        private IEnumerator DeathCoroutine()
        {
            yield return new WaitWhile(() => muteSound.IsAnyOperationProceeding || blackScreenDimming.BlackScreen.color.a < 1f);
            Debug.Log(muteSound.IsAnyOperationProceeding || blackScreenDimming.BlackScreen.color.a < 1f);
            SceneDeterminant sceneManager = SceneDeterminant.Instance;
            if (sceneManager)
                Loader.Load(sceneManager.GetRandomLoseScene());
            else
                Loader.Load(SceneConstants.MAIN_MENU);
        }
    }
}