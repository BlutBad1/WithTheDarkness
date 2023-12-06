using CreatureNS;
using UnityEngine;
namespace PortalNS
{
    public class CreaturePortalTraveller : PortalTraveller
    {
        public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
        {
            ICreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<ICreature>(gameObject);
            creature.SetPositionAndRotation(pos, rot);
        }
    }
}