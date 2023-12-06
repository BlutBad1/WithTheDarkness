using CreatureNS;
using EnvironmentEffects.MatEffect.Dissolve;
using ExtensionMethods;
using LocationManagementNS;
using PlayerScriptsNS;
using SoundNS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilitiesNS;

namespace LocationConnector
{
    public class ConnectorTeleportTrigger : TeleportTrigger
    {
        public Transform NonPlayerGameObjectSpawnPointToHere;
        [Header("DissolveEffect")]
        public Material DissolvingRefMat;
        [Min(0)]
        public float DissolvingTime = 5f;
        public AudioSourcesManager DissolveSounds;
        private Connector connector;
        private Dictionary<GameObject, Coroutine> currentTeleportCoroutines = new Dictionary<GameObject, Coroutine>();
        private Dictionary<GameObject, AudioSourcesManager> currentDisolveAudioSourcesManager = new Dictionary<GameObject, AudioSourcesManager>();
        public override int ConnectedLocIndex
        {
            get => base.ConnectedLocIndex;
            set
            {
                base.ConnectedLocIndex = value;
                if (!connector)
                    connector = Connector.Instance;
                connector.ExitRoom.transform.position = TeleportPoint.position;
                connector.ExitRoom.transform.rotation = TeleportPoint.rotation;
            }
        }
        protected override void Start()
        {
            base.Start();
            connector = Connector.Instance;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (LayerObjectToTeleport.CheckIfLayerInLayerMask(other.gameObject.layer))
                DefineObjectLogic(other.attachedRigidbody == null ? other.gameObject : other.attachedRigidbody.gameObject);
        }
        public void EnableConnection()
        {
            connector.OriginGameObject.SetActive(true);
            connector.EnterRoom.transform.position = TeleportPointToHere.position;
            connector.EnterRoom.transform.rotation = TeleportPointToHere.rotation;
            connector.ExitRoom.transform.position = TeleportPoint.position;
            connector.ExitRoom.transform.rotation = TeleportPoint.rotation;
        }
        public void DisableConnection()
        {
            connector.CloseDoors();
            connector.OriginGameObject.SetActive(false);
            if (IsConnectedToMapData)
            {
                GameObject connectedLoc = MapData.Instance.GetLocationByIndex(connectedLocIndex).MapData;
                connectedLoc.SetActive(false);
            }
        }
        public void DefineObjectLogic(GameObject gameObjectToTeleport)
        {
            DefineNextLoc();
            ICreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<ICreature>(gameObjectToTeleport);
            LocationStatusControlling(creature is PlayerCreature);
            if (creature is PlayerCreature && (((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Player))) != 0))
                EnableConnection();
            else
            {
                if (currentTeleportCoroutines.ContainsKey(gameObjectToTeleport) && currentTeleportCoroutines[gameObjectToTeleport] != null)
                    return;
                if (creature == null && (((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Gameobject))) != 0))
                    currentTeleportCoroutines[gameObjectToTeleport] = StartCoroutine(TeleportNonPlayer(gameObjectToTeleport));
                else if (((int)ObjectToTeleport & (1 << Utilities.GetIndexOfElementInEnum(TypeObjectToTeleport.Creature))) != 0)
                    currentTeleportCoroutines[gameObjectToTeleport] = StartCoroutine(TeleportNonPlayer(gameObjectToTeleport));
            }
        }
        protected void LocationStatusControlling(bool isPlayer)
        {
            if (IsConnectedToMapData)
            {
                Location connectedLoc = MapData.Instance.GetLocationByIndex(connectedLocIndex);
                connectedLoc.MapData.SetActive(true);
                if (connectedLocIndex == -2)
                {
                    connectedLoc.EntryTeleportTrigger.TeleportPoint = TeleportPointToHere;
                    connectedLoc.EntryTeleportTrigger.ConnectedLocIndex = ThisLocIndex;
                }
                //Disable not needed locations (only if a player on the current locations)
                if (isPlayer)
                {
                    foreach (var location in MapData.Instance.ActiveLocations.Where(x => x.MapData.activeInHierarchy))
                    {
                        if (location.EntryTeleportTrigger.ThisLocIndex != connectedLocIndex && location.EntryTeleportTrigger.ThisLocIndex != thisLocIndex)
                            location.MapData.SetActive(false);
                    }
                    if (connectedLocIndex != -1 && thisLocIndex != -1)
                        MapData.Instance.TheFirstLocation.MapData.SetActive(false);
                    if (connectedLocIndex != -2 && thisLocIndex != -2)
                        MapData.Instance.TheLastLocation.MapData.SetActive(false);
                }
            }
        }
        protected IEnumerator TeleportNonPlayer(GameObject gameObjectToTeleport)
        {
            gameObjectToTeleport.transform.parent = TeleportPoint.root;
            List<Renderer> meshRenderers = Utilities.FindAllComponentsInChildren<Renderer>(gameObjectToTeleport, false);
            Dissolve dissolve = Utilities.GetComponentFromGameObject<Dissolve>(gameObjectToTeleport);
            if (!dissolve)
            {
                dissolve = gameObjectToTeleport.AddComponent<Dissolve>();
                yield return null;
            }
            dissolve.meshRenderers = meshRenderers;
            dissolve.referenceMat = DissolvingRefMat;
            dissolve.InitializeMat();
            dissolve.StartDissolving(DissolvingTime);
            if (DissolveSounds)
            {
                AudioSourcesManager audioSourceManager = currentDisolveAudioSourcesManager.ContainsKey(gameObjectToTeleport) ? currentDisolveAudioSourcesManager[gameObjectToTeleport] : null;
                if (!audioSourceManager)
                {
                    audioSourceManager = gameObjectToTeleport.GetComponent<AudioSourcesManager>() == null ? gameObjectToTeleport.AddComponent<AudioSourcesManager>() :
                         gameObjectToTeleport.GetComponent<AudioSourcesManager>();
                    currentDisolveAudioSourcesManager[gameObjectToTeleport] = audioSourceManager;
                }
                audioSourceManager.CreateNewAudioSourceAndPlay(DissolveSounds.GetRandomSound());
            }
            ICreature creature = Utilities.GetComponentFromGameObject<ICreature>(gameObjectToTeleport);
            while (dissolve.CurrentDissolve < 0.95f)
                yield return null;
            Location connectedLoc = MapData.Instance.GetLocationByIndex(connectedLocIndex);
            ConnectorTeleportTrigger connectedTrigger = (ConnectorTeleportTrigger)connectedLoc.EntryTeleportTrigger;
            Transform pointTo = connectedTrigger == null ? TeleportPoint : connectedTrigger.NonPlayerGameObjectSpawnPointToHere;
            if (creature != null)
                creature.SetPositionAndRotation(pointTo.position, pointTo.rotation);
            else
            {
                gameObjectToTeleport.transform.position = pointTo.position;
                gameObjectToTeleport.transform.rotation = pointTo.rotation;
            }
            dissolve.SetDissolve(1);
            dissolve.StartEmerging(DissolvingTime);
            while (dissolve.CurrentDissolve > -0.95f)
                yield return new WaitForSeconds(0.05f);
            currentTeleportCoroutines[gameObjectToTeleport] = null;
        }
    }
}