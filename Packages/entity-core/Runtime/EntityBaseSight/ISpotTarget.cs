using UnityEngine;
namespace EntityNS.Base
{
    public interface ISpotTarget
    {
        public void HandleGainCreatureInSight(GameObject spottedTarget);
        public void HandleLoseCreatureFromSight(GameObject lostTarget);
    }
}