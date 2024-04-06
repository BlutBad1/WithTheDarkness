using AudioConstantsNS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UtilitiesNS;

namespace SoundNS.Ambient
{
	public class AmbientSoundController : MonoBehaviour
	{
		public static AmbientSoundController Instance { get; private set; }

		[SerializeField]
		private AmbientSound[] ambientSounds;
		[SerializeField]
		private float soundChangeTransitionIn = 5f;
		[SerializeField]
		private float soundChangeTransitionOut = 7f;
		[SerializeField]
		private bool autoPlayOn = true;
		[SerializeField]
		private bool pauseSourceInGameMenu = false;

		private AmbientSound currentPlayingSound;
		private AudioSourceManager audioSourceManager;
		private Dictionary<AudioMixerGroup, Coroutine> activeCoroutines = new Dictionary<AudioMixerGroup, Coroutine>();

		public bool AutoPlayOn { get => autoPlayOn; set => autoPlayOn = value; }
		public AmbientSound CurrentPlayingSound { get => currentPlayingSound; set => currentPlayingSound = value; }

		private void Start()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(this);
			audioSourceManager = gameObject.AddComponent<AudioSourceManager>();
			audioSourceManager.SetAudioSource(gameObject.AddComponent<AudioSource>());
			audioSourceManager.PauseSourcesInGameMenu = pauseSourceInGameMenu;
			ChooseAndPlayRandomAmbientSound();
		}
		private void FixedUpdate()
		{
			if (AutoPlayOn && !CurrentPlayingSound.source.isPlaying && CurrentPlayingSound.source.enabled)
				ChooseAndPlayRandomAmbientSound();
		}
		public void StopCurrentAmbient() =>
			StopCurrentAmbient(soundChangeTransitionOut);
		public void StopCurrentAmbient(float transitionTime) =>
			 audioSourceManager.StopAudioSourceSmoothly(transitionTime);
		public void MuteAmbientGroup(AudioMixerGroup audioMixerGroup, float muteTime)
		{
			CheckAndDeleteCoroutineFromDictionary(audioMixerGroup);
			activeCoroutines.Add(audioMixerGroup, StartCoroutine(ChangeAudioVolume(audioMixerGroup, muteTime, 0f)));
		}
		public void UnmuteAmbientGroup(AudioMixerGroup audioMixerGroup, float unmuteTime)
		{
			CheckAndDeleteCoroutineFromDictionary(audioMixerGroup);
			activeCoroutines.Add(audioMixerGroup, StartCoroutine(ChangeAudioVolume(audioMixerGroup, unmuteTime, 1f)));
		}
		public void ChooseAndPlayRandomAmbientSound()
		{
			AmbientSound[] ambientSoundsWithoutCurrent = ambientSounds.Where(x => x != CurrentPlayingSound).ToArray();
			CurrentPlayingSound = ambientSoundsWithoutCurrent == null || ambientSoundsWithoutCurrent.Length <= 0 ? ambientSounds[Random.Range(0, ambientSounds.Length)]
				: ambientSoundsWithoutCurrent[Random.Range(0, ambientSoundsWithoutCurrent.Length)];
			audioSourceManager.ChangeSoundSmoothly(CurrentPlayingSound, soundChangeTransitionOut, soundChangeTransitionIn, true);
			audioSourceManager.AudioObject.AudioSource.outputAudioMixerGroup = CurrentPlayingSound.audioMixerGroup;
		}
		private void CheckAndDeleteCoroutineFromDictionary(AudioMixerGroup audioMixerGroup)
		{
			if (activeCoroutines.ContainsKey(audioMixerGroup))
			{
				if (activeCoroutines[audioMixerGroup] != null)
					StopCoroutine(activeCoroutines[audioMixerGroup]);
				activeCoroutines.Remove(audioMixerGroup);
			}
		}
		private IEnumerator ChangeAudioVolume(AudioMixerGroup audioMixerGroup, float changeVolumeTime, float newVolume)
		{
			float time = 0;
			audioMixerGroup.audioMixer.GetFloat(audioMixerGroup.name + AudioConstants.MIXER_VOLUME, out float currentVolume);
			currentVolume = AudioUtilities.FromLogToLinear(currentVolume);
			while (Mathf.Abs(currentVolume - newVolume) >= 0.001f)
			{
				currentVolume = Mathf.Lerp(currentVolume, newVolume, time / changeVolumeTime);
				time += Time.deltaTime;
				audioMixerGroup.audioMixer.SetFloat(audioMixerGroup.name + AudioConstants.MIXER_VOLUME, AudioUtilities.FromLinearToLog(currentVolume));
				yield return null;
			}
			activeCoroutines.Remove(audioMixerGroup);
		}
	}
}
