using AudioConstantsNS;
using CreatureNS;
using EffectsNS.PlayerEffects;
using EnvironmentEffects.MatEffect.Dissolve;
using PlayerScriptsNS;
using SoundNS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilitiesNS;

namespace LocationManagementNS
{
	public class BlackScreenTeleportTrigger : TeleportTrigger
	{
		[SerializeField, Min(0)]
		private float spawnAfter = 2;
		[SerializeField]
		private BlackScreenDimming dimming;
		[SerializeField]
		private AudioManager audioManager;
		[Header("DissolvingEffect"), SerializeField]
		private Material dissolvingRefMat;
		[SerializeField]
		private float dissolvingTime = 5f;
		[SerializeField]
		private AudioSource dissolvingRefAudioSource;

		private Dictionary<GameObject, Coroutine> currentTeleportCoroutines = new Dictionary<GameObject, Coroutine>();
		private Dictionary<GameObject, Coroutine> currentDisolveCoroutines = new Dictionary<GameObject, Coroutine>();
		private Dictionary<GameObject, AudioSourcesManager> currentDisolveAudioSourcesManager = new Dictionary<GameObject, AudioSourcesManager>();

		protected override void Start()
		{
			base.Start();
			if (isConnectedToMapData)
			{
				if (dimming)
					dimming.FadeSpeed = 0.5f;
			}
		}
		private void OnTriggerEnter(Collider other)
		{
			if (layerObjectToTeleport == (layerObjectToTeleport | (1 << other.gameObject.layer)))
				StartTeleporting(other.attachedRigidbody == null ? other.gameObject : other.attachedRigidbody.gameObject);
		}
		public void StartTeleporting(GameObject gameObjectToTeleport)
		{
			DefineNextLoc();
			if (currentTeleportCoroutines.ContainsKey(gameObjectToTeleport) && currentTeleportCoroutines[gameObjectToTeleport] != null)
			{
				StopCoroutine(currentTeleportCoroutines[gameObjectToTeleport]);
				currentTeleportCoroutines[gameObjectToTeleport] = null;
			}

			if (Utilities.GetComponentFromGameObject<PlayerCreature>(gameObjectToTeleport) &&
				((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Player))) != 0)
			{
				dimming?.DimmingEnable();
				audioManager?.CreateAndPlay(AudioConstants.TRANSITION);
				currentTeleportCoroutines[gameObjectToTeleport] = StartCoroutine(Teleport(gameObjectToTeleport, true));
			}
			else
			{
				if (currentDisolveCoroutines.ContainsKey(gameObjectToTeleport) && currentDisolveCoroutines[gameObjectToTeleport] != null)
					return;
				if (((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Creature))) != 0
					&& Utilities.GetComponentFromGameObject<ICreature>(gameObjectToTeleport) != null)
				{
					currentTeleportCoroutines[gameObjectToTeleport] = StartCoroutine(Teleport(gameObjectToTeleport, false));
					currentDisolveCoroutines[gameObjectToTeleport] = StartCoroutine(DissolveTeleportEffectForNonPlayer(gameObjectToTeleport));
				}
				else if (((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Gameobject))) != 0)
				{
					currentTeleportCoroutines[gameObjectToTeleport] = StartCoroutine(Teleport(gameObjectToTeleport, false));
					currentDisolveCoroutines[gameObjectToTeleport] = StartCoroutine(DissolveTeleportEffectForNonPlayer(gameObjectToTeleport));
				}
			}
		}
		private IEnumerator DissolveTeleportEffectForNonPlayer(GameObject gameObjectToTeleport)
		{
			List<Renderer> meshRenderers = gameObjectToTeleport.GetComponentsInChildren<Renderer>(true).ToList();
			Dissolve dissolve = Utilities.GetComponentFromGameObject<Dissolve>(gameObjectToTeleport);
			if (!dissolve)
			{
				dissolve = gameObjectToTeleport.AddComponent<Dissolve>();
				yield return null;
			}
			dissolve.meshRenderers = meshRenderers;
			dissolve.referenceMat = dissolvingRefMat;
			dissolve.InitializeMat();
			dissolve.StartDissolving(dissolvingTime);
			float timeElapsed = 0;
			if (dissolvingRefAudioSource)
			{
				AudioSourcesManager audioSourceManager = currentDisolveAudioSourcesManager.ContainsKey(gameObjectToTeleport) ? currentDisolveAudioSourcesManager[gameObjectToTeleport] : null;
				if (!audioSourceManager)
				{
					audioSourceManager = gameObjectToTeleport.AddComponent<AudioSourcesManager>();
					currentDisolveAudioSourcesManager[gameObjectToTeleport] = audioSourceManager;
				}
				AudioSourceObject audioSourceObject = new AudioSourceObject();
				audioSourceObject.AudioObject = new AudioObject(dissolvingRefAudioSource, dissolvingRefAudioSource.volume);
				audioSourceObject.AudioSource = dissolvingRefAudioSource;
				audioSourceManager.CreateNewAudioSourceAndPlay(audioSourceObject);
			}
			while (dissolve.CurrentDissolve < 0.95f && timeElapsed < spawnAfter)
			{
				timeElapsed += Time.deltaTime;
				yield return null;
			}
			dissolve.SetDissolve(1);
			dissolve.StartEmerging(dissolvingTime);
			while (dissolve.CurrentDissolve > -0.95f)
				yield return new WaitForSeconds(0.05f);
			currentDisolveCoroutines[gameObjectToTeleport] = null;
			//dissolve.ResetAllRenderers();
		}
		private IEnumerator Teleport(GameObject gameObjectToTeleport, bool isPlayer)
		{
			float timeElapsed = 0f;
			ICreature creature = Utilities.GetComponentFromGameObject<ICreature>(gameObjectToTeleport);
			while ((dimming?.BlackScreen.color.a < 1f && isPlayer) || timeElapsed < spawnAfter)
			{
				timeElapsed += Time.deltaTime;
				yield return null;
			}
			if (creature != null)
				creature.BlockMovement();
			if (isConnectedToMapData)
			{

				GameObject connectedLoc = MapData.Instance.GetLocationByIndex(connectedLocIndex).MapData;
				connectedLoc.SetActive(true);
				if (connectedLocIndex == (int)LocationIndex.TheLastLocation)
				{
					Location theLastLocation = MapData.Instance.GetLocationByIndex((int)LocationIndex.TheLastLocation);
					theLastLocation.EntryTeleportTrigger.TeleportPoint = TeleportPointToHere;
					theLastLocation.EntryTeleportTrigger.ConnectedLocIndex = ThisLocIndex;
				}
			}
			yield return new WaitForSeconds(0.05f);
			while (isConnectedToMapData && TeleportPoint == null)
				yield return null;
			gameObjectToTeleport.transform.position = TeleportPoint.position;
			gameObjectToTeleport.transform.localRotation = TeleportPoint.rotation;
			yield return new WaitForSeconds(0.05f);
			if (isPlayer)
				dimming?.DimmingDisable();
			if (creature != null)
				creature.UnblockMovement();
			if (isConnectedToMapData)
			{
				GameObject thisLoc = MapData.Instance.GetLocationByIndex(thisLocIndex).MapData;
				if (isPlayer)
					thisLoc.SetActive(false);
			}
		}
	}
}