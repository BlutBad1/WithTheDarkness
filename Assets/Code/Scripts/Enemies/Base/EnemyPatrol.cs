using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyNS.Base
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyPatrol : MonoBehaviour
    {
        public List<Transform> PatrolPoints;
        private EnemyMovement enemyMovement;
        private int pointsIt = 0;
        private void Start()
        {
            enemyMovement = GetComponent<EnemyMovement>();
            enemyMovement.OnPatrol += DoPatrol;
        }
        public void DoPatrol()
        {
            if (PatrolPoints.Count > 0 && enemyMovement.Agent.isOnNavMesh && !enemyMovement.Agent.pathPending)
            {
                if (enemyMovement.Agent.enabled && enemyMovement.Agent.isOnNavMesh && enemyMovement.Agent.remainingDistance <= enemyMovement.Agent.stoppingDistance)
                {
                    if (!enemyMovement.Agent.hasPath || enemyMovement.Agent.velocity.sqrMagnitude == 0f)
                    {
                        pointsIt = pointsIt >= PatrolPoints.Count ? 0 : pointsIt;
                        if (enemyMovement.Agent.CalculatePath(PatrolPoints[pointsIt].position, enemyMovement.Agent.path))
                            enemyMovement.Agent.SetDestination(PatrolPoints[pointsIt].position);
                        pointsIt++;
                    }
                }
            }
        }
        public Transform GetClosestTransform(IEnumerable transforms)
        {
            Transform closestTranform = null;
            foreach (Transform transform in transforms)
            {
                if (closestTranform == null || Vector3.Distance(gameObject.transform.position, transform.position) < Vector3.Distance(gameObject.transform.position, closestTranform.position))
                    closestTranform = transform;
            }
            return closestTranform;
        }
    }
}
