using SoundNS;
using System.Collections;
using UnityEngine;
namespace PlayerScriptsNS
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerSprintLogic : MonoBehaviour
    {
        public float SprintingTime = 5f;
        public float TimeBeforeRestore = 5f;
        public float StaminaRestoreMultiplier = 1.5f;
        [Header("Audio Effect")]
        public float PercentWhenEnableAudioEffect = 10f; // When will breathing start work
        public Sound StaminaLackSound;
        protected PlayerMotor motor;
        protected AudioSourceManager audioSourceManager;
        [Min(0)]
        protected float currentTime = 0f;
        protected Coroutine currentStartSprintCoroutine = null;
        protected Coroutine currentStaminaRestoreCoroutine = null;
        private bool isActive = false;
        private void Start()
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.pitch = StaminaLackSound.pitch;
            audioSource.loop = StaminaLackSound.loop;
            audioSource.clip = StaminaLackSound.clip;
            audioSource.volume = StaminaLackSound.volume;
            StaminaLackSound.source = audioSource;
            audioSourceManager = gameObject.AddComponent<AudioSourceManager>();
            audioSourceManager.audioType = StaminaLackSound.audioKind;
            audioSourceManager.SetAudioSource(audioSource);
        }
        private void Update()
        {
            if (currentTime >= SprintingTime * PercentWhenEnableAudioEffect / 100)
            {
                float soundVolumeCoef = (currentTime - SprintingTime * PercentWhenEnableAudioEffect / 100) /
                    (SprintingTime - SprintingTime * PercentWhenEnableAudioEffect / 100);
                //Example:
                //currentTime >= 10% :
                //  soundVolumeCoef = (currentTime - 10% of SprintingTime) / (SprintingTime - 10% of SprintingTime)

                //SprintingTime == 10 => when currentTime < 10% it won't work, when it's currentTime == 10% it will work on 0 volume
                //currentTime == 4 it will work on 1/3 volume 
                //currentTime == 10 it will work of full volume
                Debug.Log(soundVolumeCoef);
                soundVolumeCoef = Mathf.Clamp01(soundVolumeCoef);
                audioSourceManager.ChangeAudioSourceVolume(StaminaLackSound.volume * soundVolumeCoef);
                if (!isActive)
                    audioSourceManager.PlayAudioSource();
                isActive = true;
            }
            else if (isActive)
            {
                isActive = false;
                audioSourceManager.StopAudioSourceSmoothly(1.5f);
            }
        }
        private void OnEnable()
        {
            motor = GetComponent<PlayerMotor>();
            motor.OnSprintStartEvent += OnSprintStart;
            motor.OnSprintCanceltEvent += OnSprintCancel;
        }
        private void OnDisable()
        {
            motor.OnSprintStartEvent -= OnSprintStart;
            motor.OnSprintCanceltEvent -= OnSprintCancel;
        }
        public void OnSprintStart()
        {
            if (currentStaminaRestoreCoroutine != null && currentTime <= SprintingTime)
            {
                StopCoroutine(currentStaminaRestoreCoroutine);
                currentStaminaRestoreCoroutine = null;
            }
            if (currentStartSprintCoroutine != null)
                return;
            currentStartSprintCoroutine = StartCoroutine(SprintStartCoroutine());
        }
        public void OnSprintCancel()
        {
            if (currentStartSprintCoroutine != null)
            {
                StopCoroutine(currentStartSprintCoroutine);
                currentStartSprintCoroutine = null;
            }
            if (currentStaminaRestoreCoroutine != null)
                return;
            currentStaminaRestoreCoroutine = StartCoroutine(SprintStaminaRestore());
        }
        private IEnumerator SprintStartCoroutine()
        {
            while (true)
            {
                if (currentTime <= SprintingTime)
                    currentTime += Time.deltaTime;
                else
                {
                    currentTime = SprintingTime;
                    motor.CancelSprint();
                }
                yield return null;
            }
        }
        private IEnumerator SprintStaminaRestore()
        {
            yield return new WaitForSeconds(TimeBeforeRestore);
            while (currentTime > 0)
            {
                currentTime -= StaminaRestoreMultiplier * Time.deltaTime;
                yield return null;
            }
        }
    }
}