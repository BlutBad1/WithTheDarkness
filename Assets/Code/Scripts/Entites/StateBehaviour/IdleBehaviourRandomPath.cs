using UnityEngine;
using UnityEngine.AI;
using Time = UnityEngine.Time;

namespace EntityNS.Base.StateBehaviourNS.Idle
{
    public class IdleBehaviourRandomPath : StateBehaviour
    {
        [SerializeField]
        private StateHandler stateHandler;
        [SerializeField]
        private float moveRadius = 5f;
        [SerializeField]
        private float pickTime = 3f;

        private float time = 0;

        private void OnEnable()
        {
            stateHandler.OnIdle += PickRandomPointAndComeToThere;
        }
        private void OnDisable()
        {
            stateHandler.OnIdle -= PickRandomPointAndComeToThere;
        }
        private void Update()
        {
            time += Time.deltaTime;
        }
        private void PickRandomPointAndComeToThere()
        {
            if (stateHandler.IsArrived())
            {
                if (time > pickTime)
                {
                    time = 0;
                    Vector3 randomPoint = Random.insideUnitSphere * moveRadius;
                    randomPoint += stateHandler.Destination;
                    NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, moveRadius, NavMesh.AllAreas);
                    stateHandler.Destination = hit.position;
                }
            }
            else
                time = 0;
        }
    }
}
