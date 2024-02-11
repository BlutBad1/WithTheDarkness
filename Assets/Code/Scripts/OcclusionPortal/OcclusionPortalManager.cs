using UnityEngine;
using UnityEngine.Serialization;

namespace OptimizationNS
{
    public class OcclusionPortalManager : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("OcclusionPortal")]
		private OcclusionPortal occlusionPortal;

        private void Start()
        {
            if (!occlusionPortal)
                occlusionPortal = GetComponent<OcclusionPortal>();
        }
        public virtual void OpenPortal() =>
             occlusionPortal.open = true;
        public virtual void ClosePortal() =>
             occlusionPortal.open = false;
        public virtual void ChangePortalStateToOpposite() =>
             occlusionPortal.open = !occlusionPortal.open;
    }
}