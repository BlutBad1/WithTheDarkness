using UnityEngine;
namespace LocationManagementNS
{
    public enum TypeObjectToTeleport
    {
        Player, Creature, Gameobject
    }
    public enum LocationIndex
    {
        TheFirstLocation = -1, TheLastLocation = -2
    }
    public abstract class TeleportTrigger : MonoBehaviour
    {
        [EnumMask]
        public TypeObjectToTeleport ObjectToTeleport;
        [SerializeField]
        protected LayerMask layerObjectToTeleport;
        [SerializeField]
        protected Transform teleportPointToHere;
        [SerializeField]
        protected bool isConnectedToMapData = true;
        [SerializeField]
        protected bool defineNextLocation = true;

        protected LocationsSpawnController spawnController;
        protected int thisLocIndex = (int)LocationIndex.TheFirstLocation;
        protected int connectedLocIndex = (int)LocationIndex.TheLastLocation;
        protected bool isLocIndexSet = false;
        private Transform teleportPoint; // position of the next spawn point of a next location 
        private NextTeleportPointDefinition nextTeleportPointDefinition;

        public Transform TeleportPointToHere { get => teleportPointToHere; set => teleportPointToHere = value; }
        public virtual int ThisLocIndex
        {
            get { return thisLocIndex; }
            set { thisLocIndex = isLocIndexSet ? thisLocIndex : value; isLocIndexSet = true; }
        }
        public virtual int ConnectedLocIndex { get => connectedLocIndex; set => connectedLocIndex = value; }
        public Transform TeleportPoint { get => teleportPoint; set => teleportPoint = value; }

        protected virtual void Start()
        {
            if (isConnectedToMapData)
                nextTeleportPointDefinition = gameObject.AddComponent<NextTeleportPointDefinition>();
            spawnController = LocationsSpawnController.Instance;
        }
        protected void DefineNextLoc()
        {
            if (defineNextLocation && isConnectedToMapData)
                nextTeleportPointDefinition.DefineNextLoc();
        }
    }
}