using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Attack
{
    public class EnemyLineOfSightChecker : EnemySight
    {
        [SerializeField, FormerlySerializedAs("RayCastIgnore"), Tooltip("What layers an enemy wiil ignore (will see through).")]
        private LayerMask rayCastIgnore;
        [SerializeField, FormerlySerializedAs("GameObjectForRay")]
        private GameObject gameObjectForRay;
        [SerializeField, FormerlySerializedAs("GapBetweenRay"), Min(1)]
        private int gapBetweenRay = 10;
        [SerializeField, FormerlySerializedAs("FieldOfView"), Range(0f, 360)]
        private int fieldOfView = 90;
        [SerializeField, FormerlySerializedAs("ViewDistance")]
        private float viewDistance = 10f;
        [SerializeField, FormerlySerializedAs("YDeviation")]
        private int yDeviation = 10;
        [SerializeField, FormerlySerializedAs("OriginOffset")]
        private Vector3 originOffset;

        private Vector3[] RaycastsDirections;
        private Quaternion currentRayGameObjectRotation;

        private void Start()
        {
            if (!gameObjectForRay)
                gameObjectForRay = gameObject;
            SetRaycastsDeviation();
        }
        private void Update()
        {
            for (int i = 0; i < RaycastsDirections.Length; i++)
            {
                if (Physics.Raycast(gameObjectForRay.transform.position + originOffset, RaycastsDirections[i] * viewDistance, out RaycastHit Hit, viewDistance, ~rayCastIgnore))
                    HandleGameObject(Hit.collider.gameObject);
            }
        }
        private void FixedUpdate()
        {
            if (currentRayGameObjectRotation != gameObjectForRay.transform.rotation)
            {
                SetRaycastsDeviation();
                currentRayGameObjectRotation = gameObjectForRay.transform.rotation;
            }
        }
        private void OnDrawGizmos()
        {
            if (RaycastsDirections != null)
            {
                Gizmos.color = Color.cyan;
                for (int i = 0; i < RaycastsDirections.Length; i++)
                    Gizmos.DrawRay(gameObjectForRay.transform.position + originOffset, RaycastsDirections[i] * viewDistance);
            }
        }
        [ContextMenu("GenerateNewRayCasts")]
        public void SetRaycastsDeviation()
        {
            //1 ray on zero, half rays to right and another half to left
            int amoutOfRaycasts = (int)(fieldOfView / gapBetweenRay);
            amoutOfRaycasts = amoutOfRaycasts % 2 == 0 ? amoutOfRaycasts + 1 : amoutOfRaycasts;
            if (RaycastsDirections == null)
                RaycastsDirections = new Vector3[amoutOfRaycasts];
            int deviation = 0;
            for (int i = 0; i < RaycastsDirections.Length; i++)
            {
                RaycastsDirections[i] = Quaternion.AngleAxis(deviation, gameObjectForRay.transform.up) * Quaternion.AngleAxis(yDeviation, -gameObjectForRay.transform.right) *
                    gameObjectForRay.transform.forward;
                if (i <= (int)RaycastsDirections.Length / 2)
                    deviation += gapBetweenRay;
                else if (i == ((int)RaycastsDirections.Length / 2) + 1)
                    deviation = -gapBetweenRay;
                else
                    deviation -= gapBetweenRay;
            }
        }
        protected override bool CheckGameObjectInSight(GameObject gameObject)
        {
            Vector3 Direction = (gameObject.transform.position - (gameObjectForRay.transform.position + originOffset)).normalized;
            float DotProduct = Vector3.Dot(gameObjectForRay.transform.forward, Direction);
            if (DotProduct >= Mathf.Cos(fieldOfView))
            {
                if (Physics.Raycast(gameObjectForRay.transform.position + originOffset, Direction, out RaycastHit Hit, viewDistance, ~rayCastIgnore))
                {
                    if ((layersForProcessing | (1 << Hit.collider.gameObject.layer)) == layersForProcessing)
                        return true;
                }
            }
            return false;
        }
    }
}
