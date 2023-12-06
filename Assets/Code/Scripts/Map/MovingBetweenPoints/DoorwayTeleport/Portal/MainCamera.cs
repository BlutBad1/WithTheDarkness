using UnityEngine;
namespace PortalNS
{
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera Instance;
        Portal[] portals;
        void Awake()
        {
            portals = FindObjectsOfType<Portal>();
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }
        void OnPreCull()
        {
            for (int i = 0; i < portals.Length; i++)
            {
                portals[i].PortalLight();
                portals[i].PrePortalRender();
                portals[i].Render();
                portals[i].PostPortalRender();
            }
        }
    }
}