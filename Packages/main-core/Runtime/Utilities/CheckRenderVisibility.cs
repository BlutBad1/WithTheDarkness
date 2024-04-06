using UnityEngine;

namespace UtilitiesNS.RendererNS
{
    public class CheckRenderVisibility
    {
        public static bool IsSomeRendererVisibleWithinCameraBounds(Renderer[] rendereres, Camera camera)
        {
            foreach (var renderer in rendereres)
            {
                if (IsRendererVisibleWithinCameraBounds(renderer, camera))
                    return true;
            }
            return false;
        }
        public static bool IsRendererVisibleWithinCameraBounds(Renderer renderer, Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
                return true;
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
