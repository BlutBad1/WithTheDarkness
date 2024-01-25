using UnityEngine;

namespace UtilitiesNS.RendererNS
{
    public class CheckRenderVisibility
    {
        public static bool IsSomeRendererVisibleWithinCameraBounds(Renderer[] rendereres, Camera mainCamera)
        {
            foreach (var renderer in rendereres)
            {
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
                if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
                    return true;
            }
            return false;
        }
        public static bool IsSomeRendererVisible(Renderer[] rendereres)
        {
            foreach (var renderer in rendereres)
            {
                if (renderer.isVisible)
                    return true;
            }
            return false;
        }
    }
}
