using PlayerScriptsNS;
using PortalNS;
using UnityEngine;

namespace LocationConnector
{
    public class HiddenTeleportBetweenTwoRooms : HiddenCreatureTeleport
    {
        public HiddenTeleportBetweenTwoRooms ConnectedHiddenTeleport;
        public Renderer[] PlaceRenders;
        public Renderer[] SecondPlaceRenders;
        private bool isTeleportingActive = false;
        private Camera mainCamera;
        private void Start()
        {
            mainCamera = MainCamera.Instance.GetComponent<Camera>();
        }
        private void Update()
        {
            if (isTeleportingActive)
                IsRenderVisible();
        }
        public void SetTeleportingActive(bool active) => isTeleportingActive = active;
        public void IsRenderVisible()
        {
            if (!IsSomeRendererVisible(PlaceRenders))
                TeleportObject(UtilitiesNS.Utilities.GetClosestComponent<PlayerCreature>(transform.position).GetCreatureGameObject(), true);
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (CheckIfGameObjectMatchesConditions(other.gameObject))
            {
                ConnectedHiddenTeleport.SetTeleportingActive(false);
                isTeleportingActive = false;
                TeleportObject(other.gameObject);
                ConnectedHiddenTeleport.SetTeleportingActive(false);
            }
        }
        public void TeleportObject(GameObject go, bool setConnectedTeleportActive)
        {
            if (isTeleportingActive && IsSomeRendererVisible(SecondPlaceRenders))
            {
                isTeleportingActive = false;
                TeleportObject(go);
                ConnectedHiddenTeleport.SetTeleportingActive(setConnectedTeleportActive);
            }
        }
        protected bool IsSomeRendererVisible(Renderer[] rendereres)
        {
            foreach (var renderer in rendereres)
            {
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
                if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
                    return true;
            }
            return false;
        }
    }
}
