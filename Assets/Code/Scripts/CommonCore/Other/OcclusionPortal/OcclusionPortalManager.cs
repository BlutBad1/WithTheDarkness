using UnityEngine;
namespace OptimizationNS
{
    public class OcclusionPortalManager : MonoBehaviour
    {
        public OcclusionPortal OcclusionPortal;
        private void Start()
        {
            if (!OcclusionPortal)
                OcclusionPortal = GetComponent<OcclusionPortal>();
        }
        public virtual void OpenPortal() =>
             OcclusionPortal.open = true;
        public virtual void ClosePortal() =>
             OcclusionPortal.open = false;
        public virtual void ChangePortalStateToOpposite() =>
             OcclusionPortal.open = !OcclusionPortal.open;
    }
}