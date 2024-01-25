using PlayerScriptsNS;
using PortalNS;
using UnityEngine;
using UtilitiesNS.RendererNS;

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
            mainCamera = Camera.main;
        }
        private void Update()
        {
            if (isTeleportingActive)
                IsRenderVisible();
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
        public void SetTeleportingActive(bool active) => isTeleportingActive = active;
        public void IsRenderVisible()
        {
            if (!CheckRenderVisibility.IsSomeRendererVisibleWithinCameraBounds(PlaceRenders, mainCamera))
                TeleportObject(UtilitiesNS.Utilities.GetClosestComponent<PlayerCreature>(transform.position).GetCreatureGameObject(), true);
        }
        public void TeleportObject(GameObject go, bool setConnectedTeleportActive)
        {
            if (isTeleportingActive && CheckRenderVisibility.IsSomeRendererVisibleWithinCameraBounds(SecondPlaceRenders, mainCamera))
            {
                isTeleportingActive = false;
                TeleportObject(go);
                ConnectedHiddenTeleport.SetTeleportingActive(setConnectedTeleportActive);
            }
        }
    }
}
