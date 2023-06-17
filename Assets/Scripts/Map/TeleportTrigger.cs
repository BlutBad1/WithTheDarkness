using HudNS;
using MyConstants;
using PlayerScriptsNS;
using SoundNS;
using System.Collections;
using UnityEngine;

namespace LocationManagementNS
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField]
        GameObject player;
        [SerializeField]
        public Transform teleportPointToHere;
        [HideInInspector]
        public Transform teleportPoint; // position of the next spawn point of a next location 
        [SerializeField, Range(0, float.MaxValue)]
        float spawnAfter = 2;
        public BlackScreenDimming dimming;
        public AudioManager audioManager;
        public bool IsConnectedToMapData = true;
        //thisLocIndex = -1, it means it's the first location. 
        //thisLocIndex = -2, it means it's the last location. 
        //connectedLocIndex = -2, it's connected to the last location. 
        [HideInInspector]
        public int thisLocIndex = -1, connectedLocIndex = -2;
        public Coroutine CurrentTeleportCoroutine;
        private void Awake()
        {
            thisLocIndex = -1;
            connectedLocIndex = -2;
        }
        private void Start()
        {
            if (IsConnectedToMapData)
            {
                if (!dimming)
                    dimming = GameObject.Find(HUDConstants.BLACK_SCREEN_DIMMING).GetComponent<BlackScreenDimming>();
                if (dimming)
                    dimming.fadeSpeed = 0.5f;
                if (!audioManager)
                    audioManager = GameObject.Find(CommonConstants.MAIN_AUDIOMANAGER).GetComponent<AudioManager>();
            }
            if (!player)
                player = GameObject.Find(CommonConstants.PLAYER);
        }
        public void StartTeleporting()
        {
            if (CurrentTeleportCoroutine == null)
            {
                dimming?.DimmingEnable();
                audioManager?.CreateAndPlay(MainAudioManagerConstants.TRANSITION);
                CurrentTeleportCoroutine = StartCoroutine(Teleport());
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == CommonConstants.PLAYER)
            {
                if (!player)
                    player = other.gameObject;
                StartTeleporting();
            }
        }
        IEnumerator Teleport()
        {
            float timeElapsed = 0f;
            while (dimming?.blackScreen.color.a < 1f || timeElapsed < spawnAfter)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            if (player.TryGetComponent(out InputManager inputManager))
                inputManager.IsMovingEnable = false;
            yield return new WaitForSeconds(0.05f);
            while (IsConnectedToMapData && teleportPoint == null)
                yield return null;
            player.transform.position = teleportPoint.position;
            player.transform.localRotation = teleportPoint.rotation;
            yield return new WaitForSeconds(0.05f);
            dimming?.DimmingDisable();
            CurrentTeleportCoroutine = null;
            if (inputManager != null)
                inputManager.IsMovingEnable = true;
            if (IsConnectedToMapData)
            {
                GameObject connectedLoc = connectedLocIndex == -1 ? MapData.instance.TheFirstLocation.MapData
                         : connectedLocIndex == -2 ? MapData.instance.TheLastLocation.MapData : MapData.instance.LocationsArr[connectedLocIndex].MapData;
                connectedLoc.SetActive(true);
                GameObject thisLoc = thisLocIndex == -1 ? MapData.instance.TheFirstLocation.MapData
                    : thisLocIndex == -2 ? MapData.instance.TheLastLocation.MapData : MapData.instance.LocationsArr[thisLocIndex].MapData;
                thisLoc.SetActive(false);
            }
        }
    }
}