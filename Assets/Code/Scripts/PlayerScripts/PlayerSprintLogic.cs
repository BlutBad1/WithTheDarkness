using SoundNS;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
	[RequireComponent(typeof(PlayerMotor))]
	public class PlayerSprintLogic : MonoBehaviour
	{
		[SerializeField, FormerlySerializedAs("SprintingTime")]
		private float sprintingTime = 5f;
		[SerializeField, FormerlySerializedAs("TimeBeforeRestore")]
		private float timeBeforeRestore = 5f;
		[SerializeField, FormerlySerializedAs("StaminaRestoreMultiplier")]
		private float staminaRestoreMultiplier = 1.5f;
		[Header("Audio Effect"), SerializeField, FormerlySerializedAs("PercentWhenEnableAudioEffect")]
		private float percentWhenEnableAudioEffect = 10f; // When will breathing start work
		[SerializeField, FormerlySerializedAs("StaminaLackSound")]
		private Sound staminaLackSound;

		private float currentTime = 0f;
		protected PlayerMotor motor;
		protected AudioSourceManager audioSourceManager;
		protected Coroutine currentStartSprintCoroutine = null;
		protected Coroutine currentStaminaRestoreCoroutine = null;
		private bool isActive = false;

		public float SprintingTime { get => sprintingTime; set => sprintingTime = value; }

		private void Start()
		{
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.pitch = staminaLackSound.pitch;
			audioSource.loop = staminaLackSound.loop;
			audioSource.clip = staminaLackSound.clip;
			audioSource.volume = staminaLackSound.volume;
			staminaLackSound.source = audioSource;
			audioSourceManager = gameObject.AddComponent<AudioSourceManager>();
			audioSource.outputAudioMixerGroup = MixerVolumeChanger.Instance.GetAudioMixerGroup(staminaLackSound.audioKind);
			audioSourceManager.SetAudioSource(audioSource);
		}
		private void Update()
		{
			if (currentTime >= SprintingTime * percentWhenEnableAudioEffect / 100)
			{
				float soundVolumeCoef = (currentTime - SprintingTime * percentWhenEnableAudioEffect / 100) /
					(SprintingTime - SprintingTime * percentWhenEnableAudioEffect / 100);
				//Example:
				//currentTime >= 10% :
				//  soundVolumeCoef = (currentTime - 10% of SprintingTime) / (SprintingTime - 10% of SprintingTime)

				//SprintingTime == 10 => when currentTime < 10% it won't work, when it's currentTime == 10% it will work on 0 volume
				//currentTime == 4 it will work on 1/3 volume 
				//currentTime == 10 it will work of full volume
				soundVolumeCoef = Mathf.Clamp01(soundVolumeCoef);
				audioSourceManager.ChangeAudioSourceVolume(staminaLackSound.volume * soundVolumeCoef);
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
		private void OnSprintStart()
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
		private void OnSprintCancel()
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
			yield return new WaitForSeconds(timeBeforeRestore);
			while (currentTime > 0)
			{
				currentTime -= staminaRestoreMultiplier * Time.deltaTime;
				yield return null;
			}
		}
	}
}