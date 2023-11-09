using CreatureNS;
using EffectsNS.PlayerEffects;
using EnvironmentEffects.MatEffect.Dissolve;
using MyConstants;
using SoundNS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UtilitiesNS;

namespace LocationManagementNS
{
    public enum TypeObjectToTeleport
    {
        Player, Creature, Gameobject
    }
    public class TeleportTrigger : MonoBehaviour
    {
        [EnumMask, HideInInspector]
        public TypeObjectToTeleport ObjectToTeleport;
        [HideInInspector]
        public LayerMask LayerObjectToTeleport;
        [HideInInspector]
        public Transform TeleportPointToHere;
        [HideInInspector]
        public Transform TeleportPoint; // position of the next spawn point of a next location 
        [SerializeField, Min(0)]
        private float spawnAfter = 2;
        public BlackScreenDimming dimming;
        public AudioManager audioManager;
        public bool IsConnectedToMapData = true;
        [Header("DissolvingEffect")]
        public Material DissolvingRefMat;
        public float DissolvingTime = 5f;
        public AudioSource DissolvingRefAudioSource;
        //thisLocIndex = -1, it means it's the first location. 
        //thisLocIndex = -2, it means it's the last location. 
        //connectedLocIndex = -2, it's connected to the last location. 
        private int thisLocIndex = -1;
        [HideInInspector]
        public int ConnectedLocIndex = -2;
        private Dictionary<GameObject, Coroutine> currentTeleportCoroutines = new Dictionary<GameObject, Coroutine>();
        private Dictionary<GameObject, Coroutine> currentDisolveCoroutines = new Dictionary<GameObject, Coroutine>();
        private Dictionary<GameObject, AudioSourcesManager> currentDisolveAudioSourcesManager = new Dictionary<GameObject, AudioSourcesManager>();
        private bool isLocIndexSet = false;
        public int ThisLocIndex
        {
            get { return thisLocIndex; }
            set { thisLocIndex = isLocIndexSet ? thisLocIndex : value; isLocIndexSet = true; }
        }
        private void Start()
        {
            if (IsConnectedToMapData)
            {
                if (!dimming)
                    dimming = GameObject.Find(HUDConstants.BLACK_SCREEN_DIMMING).GetComponent<BlackScreenDimming>();
                if (dimming)
                    dimming.FadeSpeed = 0.5f;
                if (!audioManager)
                    audioManager = GameObject.Find(CommonConstants.IMPORTANT_SOUNDS).GetComponent<AudioManager>();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == CommonConstants.PLAYER || LayerObjectToTeleport == (LayerObjectToTeleport | (1 << other.gameObject.layer)))
                StartTeleporting(other.gameObject);
        }
        public void StartTeleporting(GameObject gameObjectToTeleport)
        {
            if (currentTeleportCoroutines.ContainsKey(gameObjectToTeleport) && currentTeleportCoroutines[gameObjectToTeleport] != null)
            {
                StopCoroutine(currentTeleportCoroutines[gameObjectToTeleport]);
                currentTeleportCoroutines[gameObjectToTeleport] = null;
            }
            if (gameObjectToTeleport.name == CommonConstants.PLAYER &&
                ((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Player))) != 0)
            {
                dimming?.DimmingEnable();
                audioManager?.CreateAndPlay(MainAudioManagerConstants.TRANSITION);
                currentTeleportCoroutines[gameObjectToTeleport] = StartCoroutine(Teleport(gameObjectToTeleport, true));
            }
            else if (gameObjectToTeleport.name != CommonConstants.PLAYER)
            {
                if (currentDisolveCoroutines.ContainsKey(gameObjectToTeleport) && currentDisolveCoroutines[gameObjectToTeleport] != null)
                {
                    StopCoroutine(currentDisolveCoroutines[gameObjectToTeleport]);
                    currentDisolveCoroutines[gameObjectToTeleport] = null;
                }
                if (((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Creature))) != 0
                    && ICreature.GetICreatureComponent(gameObjectToTeleport) != null)
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
            MeshRenderer[] meshRenderers = gameObjectToTeleport.GetComponentsInChildren<MeshRenderer>(true);
            Dissolve dissolve = Utilities.GetComponentFromGameObject<Dissolve>(gameObjectToTeleport);
            if (!dissolve)
                dissolve = gameObjectToTeleport.AddComponent<Dissolve>();
            dissolve.meshRenderers = meshRenderers.ToList();
            dissolve.referenceMat = DissolvingRefMat;
            dissolve.InitializeMat();
            dissolve.StartDissolving(DissolvingTime);
            float timeElapsed = 0;
            if (DissolvingRefAudioSource)
            {
                AudioSourcesManager audioSourceManager = currentDisolveAudioSourcesManager.ContainsKey(gameObjectToTeleport) ? currentDisolveAudioSourcesManager[gameObjectToTeleport] : null;
                if (!audioSourceManager)
                {
                    audioSourceManager = gameObjectToTeleport.AddComponent<AudioSourcesManager>();
                    currentDisolveAudioSourcesManager[gameObjectToTeleport] = audioSourceManager;
                }
                AudioSourceObject audioSourceObject = new AudioSourceObject();
                audioSourceObject.AudioKind = SettingsNS.AudioSettings.AudioKind.SFX;
                audioSourceObject.AudioObject = new AudioObject(DissolvingRefAudioSource, DissolvingRefAudioSource.volume, SettingsNS.AudioSettings.AudioKind.SFX);
                audioSourceObject.AudioSource = DissolvingRefAudioSource;
                audioSourceManager.CreateNewAudioSourceAndPlay(audioSourceObject);
            }
            while (dissolve.CurrentDissolve < 0.95f && timeElapsed < spawnAfter)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            dissolve.SetDissolve(1);
            dissolve.StartEmerging(DissolvingTime);
            while (dissolve.CurrentDissolve > -0.95f)
                yield return new WaitForSeconds(0.05f);
            //dissolve.ResetAllRenderers();
        }
        private IEnumerator Teleport(GameObject gameObjectToTeleport, bool isPlayer)
        {
            float timeElapsed = 0f;
            ICreature creature = ICreature.GetICreatureComponent(gameObjectToTeleport);
            while ((dimming?.BlackScreen.color.a < 1f && isPlayer) || timeElapsed < spawnAfter)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            if (creature != null)
                creature.BlockMovement();
            if (IsConnectedToMapData)
            {
                GameObject connectedLoc = ConnectedLocIndex == -1 ? MapData.Instance.TheFirstLocation.MapData
                         : ConnectedLocIndex == -2 ? MapData.Instance.TheLastLocation.MapData : MapData.Instance.ActiveLocations[ConnectedLocIndex].MapData;
                connectedLoc.SetActive(true);
                if (ConnectedLocIndex == -2)
                {
                    MapData.Instance.TheLastLocation.EntryTeleportTrigger.TeleportPoint = TeleportPointToHere;
                    MapData.Instance.TheLastLocation.EntryTeleportTrigger.ConnectedLocIndex = ThisLocIndex;
                }
            }
            yield return new WaitForSeconds(0.05f);
            while (IsConnectedToMapData && TeleportPoint == null)
                yield return null;
            gameObjectToTeleport.transform.position = TeleportPoint.position;
            gameObjectToTeleport.transform.localRotation = TeleportPoint.rotation;
            yield return new WaitForSeconds(0.05f);
            if (isPlayer)
                dimming?.DimmingDisable();
            if (creature != null)
                creature.UnBlockMovement();
            if (IsConnectedToMapData)
            {
                GameObject thisLoc = ThisLocIndex == -1 ? MapData.Instance.TheFirstLocation.MapData
                    : ThisLocIndex == -2 ? MapData.Instance.TheLastLocation.MapData : MapData.Instance.ActiveLocations[ThisLocIndex].MapData;
                if (isPlayer)
                    thisLoc.SetActive(false);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(TeleportTrigger), true)]
    public class TeleportTrigger_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            TeleportTrigger script = (TeleportTrigger)target;
            // draw checkbox for the bool
            var property = serializedObject.FindProperty("ObjectToTeleport");
            EditorGUILayout.PropertyField(property, new GUIContent("ObjectToTeleport"), true);

            property = serializedObject.FindProperty("LayerObjectToTeleport");
            EditorGUILayout.PropertyField(property, new GUIContent("LayerObjectToTeleport"), true);

            property = serializedObject.FindProperty("TeleportPointToHere");
            EditorGUILayout.PropertyField(property, new GUIContent("TeleportPointToHere"), true);

            if (!script.IsConnectedToMapData)
            {
                property = serializedObject.FindProperty("TeleportPoint");
                EditorGUILayout.PropertyField(property, new GUIContent("TeleportPoint"), true);
            }
            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector(); // for other non-HideInInspector fields
        }
    }
#endif
}