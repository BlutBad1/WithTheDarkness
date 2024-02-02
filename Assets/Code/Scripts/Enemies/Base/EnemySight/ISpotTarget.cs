using UnityEngine;
namespace EnemyNS.Base
{
    public interface ISpotTarget
    {
        public void HandleGainCreatureInSight(GameObject spottedTarget);
        public void HandleLoseCreatureFromSight(GameObject lostTarget);
    }
}