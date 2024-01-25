using CreatureNS;
using EnvironmentEffects.MatEffect.Dissolve;
using ExtensionMethods;
using GameObjectsControllingNS;
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
    public class TeleportedGameobject
    {
        public AudioSourcesManager AudioSourcesManager;
        public Dissolve Dissolve;
        public TeleportedGameobject(AudioSourcesManager audioSourcesManager, Dissolve dissolve)
        {
            AudioSourcesManager = audioSourcesManager;
            Dissolve = dissolve;
        }
    }
    public class ConnectorTeleportTrigger : TeleportTrigger
    {
        [SerializeField]
        private Transform nonPlayerGameObjectSpawnPointToHere;
        [Header("DissolveEffect"), SerializeField]
        private Material dissolvingRefMat;
        [SerializeField, Min(0)]
        private float dissolvingTime = 0.5f;
        [SerializeField]
        private AudioSourcesManager dissolveSounds;
        [SerializeField]
        private GameObject dissolveSoundGameObject;

        private Connector connector;
        private Dictionary<GameObject, Coroutine> currentTeleportCoroutines = new Dictionary<GameObject, Coroutine>();
        private Dictionary<GameObject, TeleportedGameobject> currentDisolveAudioSourcesManager = new Dictionary<GameObject, TeleportedGameobject>();

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
            if (layerObjectToTeleport.CheckIfLayerInLayerMask(other.gameObject.layer))
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
            if (isConnectedToMapData)
            {
                Location connectedLoc = MapData.Instance.GetLocationByIndex(connectedLocIndex);
                StartCoroutine(DisableLoc(connectedLoc.MapData));
            }
        }
        private void DefineObjectLogic(GameObject gameObjectToTeleport)
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
        private void LocationStatusControlling(bool isPlayer)
        {
            if (isConnectedToMapData)
            {
                Location connectedLoc = MapData.Instance.GetLocationByIndex(connectedLocIndex);
                connectedLoc.MapData.SetActive(true);
                if (connectedLocIndex == -2)
                {
                    connectedLoc.EntryTeleportTrigger.TeleportPoint = TeleportPointToHere;
                    connectedLoc.EntryTeleportTrigger.ConnectedLocIndex = ThisLocIndex;
                }
                //Disable not needed locations (only if a player on the current location)
                if (isPlayer)
                {
                    foreach (var location in MapData.Instance.ActiveLocations.Where(x => x.MapData.activeInHierarchy))
                    {
                        if (location.MapData != connectedLoc.MapData)
                            StartCoroutine(DisableLoc(location.MapData));
                    }
                    Location theFirstLocation = MapData.Instance.GetLocationByIndex((int)LocationIndex.TheFirstLocation);
                    Location theLastLocation = MapData.Instance.GetLocationByIndex((int)LocationIndex.TheLastLocation);
                    if (connectedLoc.MapData != theFirstLocation.MapData)
                        StartCoroutine(DisableLoc(theFirstLocation.MapData));
                    if (connectedLoc.MapData != theLastLocation.MapData)
                        StartCoroutine(DisableLoc(theLastLocation.MapData));
                }
            }
        }
        private IEnumerator DisableLoc(GameObject locToDisable)
        {
            yield return null;
            if (locToDisable != MapData.Instance.GetLocationByIndex(thisLocIndex).MapData)
                locToDisable.SetActive(false);
        }
        private IEnumerator TeleportNonPlayer(GameObject gameObjectToTeleport)
        {
            DefineGameobjectParent(gameObjectToTeleport);
            Dissolve dissolve = SetAndEnableDisolving(gameObjectToTeleport);
            while (dissolve.CurrentDissolve < 0.95f)
                yield return null;
            Location connectedLoc = MapData.Instance.GetLocationByIndex(connectedLocIndex);
            ConnectorTeleportTrigger connectedTrigger = (ConnectorTeleportTrigger)connectedLoc.EntryTeleportTrigger;
            Transform teleportToPoint = connectedTrigger == null ? TeleportPoint : connectedTrigger.nonPlayerGameObjectSpawnPointToHere;
            TeleportGameobjectObject(gameObjectToTeleport, teleportToPoint);
            dissolve.SetDissolve(1);
            dissolve.StartEmerging(dissolvingTime);
            while (dissolve.CurrentDissolve > -0.95f)
                yield return new WaitForSeconds(0.05f);
            currentTeleportCoroutines[gameObjectToTeleport] = null;
        }
        private void DefineGameobjectParent(GameObject gameObjectToTeleport)
        {
            GameobjectRoot gameobjectRoot = Utilities.GetComponentFromGameObject<GameobjectRoot>(gameObjectToTeleport, includeSiblings: false);
            if (gameobjectRoot)
                gameobjectRoot.gameObject.transform.parent = TeleportPoint.root;
            else
                gameObjectToTeleport.transform.parent = TeleportPoint.root;
        }
        private Dissolve SetAndEnableDisolving(GameObject gameObjectToTeleport)
        {
            Dissolve dissolve;
            AudioSourcesManager audioSourceManager;
            if (currentDisolveAudioSourcesManager.ContainsKey(gameObjectToTeleport))
            {
                dissolve = currentDisolveAudioSourcesManager[gameObjectToTeleport].Dissolve;
                audioSourceManager = currentDisolveAudioSourcesManager[gameObjectToTeleport].AudioSourcesManager;
            }
            else
            {
                audioSourceManager = gameObjectToTeleport.AddComponent<AudioSourcesManager>();
                List<Renderer> meshRenderers = Utilities.FindAllComponentsInGameObject<Renderer>(gameObjectToTeleport, includeInactive: false)
                    .Where(x => x.GetType() != typeof(ParticleSystemRenderer)).ToList(); //HARDCODE, need to fix
                dissolve = gameObjectToTeleport.AddComponent<Dissolve>();
                dissolve.meshRenderers = meshRenderers;
                currentDisolveAudioSourcesManager.Add(gameObjectToTeleport, new TeleportedGameobject(audioSourceManager, dissolve));
            }
            dissolve.referenceMat = dissolvingRefMat;
            dissolve.InitializeMat();
            dissolve.StartDissolving(dissolvingTime);
            if (dissolveSounds)
            {
                dissolveSoundGameObject.transform.position = gameObjectToTeleport.transform.position;
                dissolveSoundGameObject.transform.rotation = gameObjectToTeleport.transform.rotation;
                audioSourceManager.CreateNewAudioSourceAndPlay(dissolveSounds.GetRandomSound());
            }
            return dissolve;
        }
        private void TeleportGameobjectObject(GameObject gameObjectToTeleport, Transform teleportToPoint)
        {
            ICreature creature = Utilities.GetComponentFromGameObject<ICreature>(gameObjectToTeleport);
            if (creature != null)
                creature.SetPositionAndRotation(teleportToPoint.position, teleportToPoint.rotation);
            else
            {
                gameObjectToTeleport.transform.position = teleportToPoint.position;
                gameObjectToTeleport.transform.rotation = teleportToPoint.rotation;
            }
        }
    }
}