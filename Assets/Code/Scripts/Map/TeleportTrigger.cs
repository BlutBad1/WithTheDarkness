using EffectsNS.PlayerEffects;
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
        private GameObject player;
        [SerializeField]
        public Transform teleportPointToHere;
        [HideInInspector]
        public Transform teleportPoint; // position of the next spawn point of a next location 
        [SerializeField, Range(0, float.MaxValue)]
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
            if (!player)
                player = GameObject.Find(CommonConstants.PLAYER);
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
        public void StartTeleporting()
        {
            if (CurrentTeleportCoroutine == null)
            {
                dimming?.DimmingEnable();
                audioManager?.CreateAndPlay(MainAudioManagerConstants.TRANSITION);
                CurrentTeleportCoroutine = StartCoroutine(Teleport());
            }
        }
        private IEnumerator Teleport()
        {
            float timeElapsed = 0f;
            while (dimming?.BlackScreen.color.a < 1f || timeElapsed < spawnAfter)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            if (player.TryGetComponent(out InputManager inputManager))
                inputManager.SetMovingLock(true);
            if (IsConnectedToMapData)
            {
                GameObject connectedLoc = ConnectedLocIndex == -1 ? MapData.Instance.TheFirstLocation.MapData
                         : ConnectedLocIndex == -2 ? MapData.Instance.TheLastLocation.MapData : MapData.Instance.ActiveLocations[ConnectedLocIndex].MapData;
                connectedLoc.SetActive(true);
                if (ConnectedLocIndex == -2)
                {
                    MapData.Instance.TheLastLocation.EntryTeleportTrigger.teleportPoint = teleportPointToHere;
                    MapData.Instance.TheLastLocation.EntryTeleportTrigger.ConnectedLocIndex = ThisLocIndex;
                }
            }
            yield return new WaitForSeconds(0.05f);
            while (IsConnectedToMapData && teleportPoint == null)
                yield return null;
            player.transform.position = teleportPoint.position;
            player.transform.localRotation = teleportPoint.rotation;
            yield return new WaitForSeconds(0.05f);
            dimming?.DimmingDisable();
            CurrentTeleportCoroutine = null;
            if (inputManager != null)
                inputManager.SetMovingLock(false);
            if (IsConnectedToMapData)
            {
                GameObject thisLoc = ThisLocIndex == -1 ? MapData.Instance.TheFirstLocation.MapData
                    : ThisLocIndex == -2 ? MapData.Instance.TheLastLocation.MapData : MapData.Instance.ActiveLocations[ThisLocIndex].MapData;
                thisLoc.SetActive(false);
            }
        }
    }
}