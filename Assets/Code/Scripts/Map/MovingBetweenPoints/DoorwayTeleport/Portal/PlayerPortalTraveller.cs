using PlayerScriptsNS;
using UnityEngine;

namespace PortalNS
{
    public class PlayerPortalTraveller : CreaturePortalTraveller
    {
        //public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
        //{
        //    PlayerCreature playerCreature = UtilitiesNS.Utilities.GetComponentFromGameObject<PlayerCreature>(gameObject);
        //    playerCreature.SetPositionAndRotationWithoutBlur(pos, rot);
        //}
        //public override void EnterPortalThreshold(Portal portal)
        //{
        //    PlayerCreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<PlayerCreature>(gameObject);
        //    creature.SetMotionBlurState(false);
        //    base.EnterPortalThreshold(portal);
        //}
        //public override void ExitPortalThreshold()
        //{
        //    PlayerCreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<PlayerCreature>(gameObject);
        //    creature.SetMotionBlurState(true);
        //    base.ExitPortalThreshold();
        //}
    }
}