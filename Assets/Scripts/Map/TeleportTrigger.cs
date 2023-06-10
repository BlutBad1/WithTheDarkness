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
        [SerializeField]
        float spawnAfter = 2;
        float timeElapsed = 0;
        public GameObject dimming = null;
        public GameObject audioManager;
        bool isActivated = false;
        //thisLocIndex = -1, it means it's the first location. 
        //thisLocIndex = -2, it means it's the last location. 
        //connectedLocIndex = -2, it's connected to the last location. 
        [HideInInspector]
        public int thisLocIndex = -1, connectedLocIndex = -2;
        private void Awake()
        {
            thisLocIndex = -1;
            connectedLocIndex = -2;
        }
        private void Start()
        {
            if (!dimming)
                dimming = GameObject.Find(HUDConstants.BLACK_SCREEN_DIMMING);
            if (dimming)
                dimming.GetComponent<BlackScreenDimming>().fadeSpeed = 0.5f;
            if (!audioManager)
                audioManager = GameObject.Find(CommonConstants.MAIN_AUDIOMANAGER);
            if (!player)
                player = GameObject.Find(CommonConstants.PLAYER);
        }
        private void Update()
        {
            if (isActivated)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= spawnAfter)
                {
                    StartCoroutine(Teleport());
                    dimming?.GetComponent<BlackScreenDimming>().DimmingDisable();
                    timeElapsed = 0;
                    isActivated = false;
                }
            }
        }
        public void StartTeleporting()
        {
            isActivated = true;
            dimming?.GetComponent<BlackScreenDimming>().DimmingEnable();
            audioManager?.GetComponent<AudioManager>().CreateAndPlay(MainAudioManagerConstants.TRANSITION);
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
            if (player.TryGetComponent(out InputManager inputManager))
                inputManager.IsMovingEnable = false;
            yield return new WaitForSeconds(0.05f);
            player.transform.position = teleportPoint.position;
            player.transform.localRotation = teleportPoint.rotation;
            yield return new WaitForSeconds(0.05f);
            if (inputManager != null)
                inputManager.IsMovingEnable = true;
            GameObject connectedLoc = connectedLocIndex == -1 ? MapData.instance.TheFirstLocation.MapData
                     : connectedLocIndex == -2 ? MapData.instance.TheLastLocation.MapData : MapData.instance.LocationsArr[connectedLocIndex].MapData;
            connectedLoc.SetActive(true);
            GameObject thisLoc = thisLocIndex == -1 ? MapData.instance.TheFirstLocation.MapData
                : thisLocIndex == -2 ? MapData.instance.TheLastLocation.MapData : MapData.instance.LocationsArr[thisLocIndex].MapData;
            thisLoc.SetActive(false);
        }
    }
}