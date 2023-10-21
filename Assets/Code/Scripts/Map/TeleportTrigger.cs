using CreatureNS;
using EffectsNS.PlayerEffects;
using MyConstants;
using SoundNS;
using System.Collections;
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
        //thisLocIndex = -1, it means it's the first location. 
        //thisLocIndex = -2, it means it's the last location. 
        //connectedLocIndex = -2, it's connected to the last location. 
        private int thisLocIndex = -1;
        [HideInInspector]
        public int ConnectedLocIndex = -2;
        public Coroutine CurrentTeleportCoroutine;
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
            if (gameObjectToTeleport.name == CommonConstants.PLAYER &&
                ((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Player))) != 0
                && CurrentTeleportCoroutine == null)
            {
                dimming?.DimmingEnable();
                audioManager?.CreateAndPlay(MainAudioManagerConstants.TRANSITION);
                CurrentTeleportCoroutine = StartCoroutine(Teleport(gameObjectToTeleport, true));
            }
            else if (gameObjectToTeleport.name != CommonConstants.PLAYER)
            {
                if (((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Creature))) != 0
                    && ICreature.GetICreatureComponent(gameObjectToTeleport) != null)
                    StartCoroutine(Teleport(gameObjectToTeleport, false));
                else if (((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Gameobject))) != 0)
                    StartCoroutine(Teleport(gameObjectToTeleport, false));
            }
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
            {
                dimming?.DimmingDisable();
                CurrentTeleportCoroutine = null;
            }
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