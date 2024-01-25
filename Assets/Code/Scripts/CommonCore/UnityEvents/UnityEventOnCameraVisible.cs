using UnityEngine;
using UnityEngine.Events;
using UtilitiesNS.RendererNS;

namespace UnityEventNS
{
    public class UnityEventOnCameraVisible : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] renderers;
        [SerializeField]
        private UnityEvent eventOnRenderersVisible;
        [SerializeField]
        private UnityEvent eventOnRenderersInvisible;

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }
        private void Update()
        {
            if (CheckRenderVisibility.IsSomeRendererVisibleWithinCameraBounds(renderers, mainCamera))
                eventOnRenderersVisible?.Invoke();
            else
                eventOnRenderersInvisible?.Invoke();
        }
    }
}