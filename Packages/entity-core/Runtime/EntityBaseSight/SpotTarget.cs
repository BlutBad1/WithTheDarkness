using UnityEngine;

namespace EntityNS.Base
{
    public abstract class SpotTarget : MonoBehaviour, ISpotTarget
    {
        public delegate void TargetSpotEvent(GameObject target);
        public TargetSpotEvent OnTargetSpot;
        public TargetSpotEvent OnTargetLost;

        public abstract void HandleGainCreatureInSight(GameObject spottedTarget);
        public abstract void HandleLoseCreatureFromSight(GameObject lostTarget);
    }
}
