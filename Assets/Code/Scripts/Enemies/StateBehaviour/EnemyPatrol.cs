using EnemyNS.Base.StateBehaviourNS;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyNS.Base.StateBehaviourNS.Patrol
{
    public class EnemyPatrol : StateBehaviour
    {
        [SerializeField]
        private List<Transform> patrolPoints;
        [SerializeField]
        private EnemyStateHandler enemyStateHandler;
        [SerializeField]
        private EnemyMovement enemyMovement;

        private int pointsIt = 0;

        private void OnEnable()
        {
            enemyStateHandler.OnPatrol += DoPatrol;
        }
        private void OnDisable()
        {
            enemyStateHandler.OnPatrol -= DoPatrol;
        }
        private void DoPatrol()
        {
            if (UtilitiesNS.Utilities.CheckIfAgentHasArrived(enemyMovement.Agent))
            {
                pointsIt = pointsIt >= patrolPoints.Count ? 0 : pointsIt;
                if (enemyMovement.Agent.CalculatePath(patrolPoints[pointsIt].position, enemyMovement.Agent.path))
                    enemyMovement.Agent.SetDestination(patrolPoints[pointsIt].position);
                pointsIt++;
            }
        }
    }
}
