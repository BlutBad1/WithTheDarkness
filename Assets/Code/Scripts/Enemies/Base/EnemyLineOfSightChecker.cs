using EnemyNS.Base;
using SerializableTypes;
using System.Collections;
using UnityEngine;
namespace EnemyNS.Attack
{
    public class EnemyLineOfSightChecker : MonoBehaviour
    {
        public Enemy Enemy;
        public GameObject GameObjectForRay;
        [Min(1)]
        public int GapBetweenRay = 10;
        [Range(0f, 360)]
        public int FieldOfView = 90;
        public float ViewDistance = 10f;
        public int YDeviation = 10;
        public Vector3 OriginOffset;
        [Tooltip("Which layers enemy can see.")]
        public LayerMask LayersForProcessing;
        public LayerMask RayCastIgnore;
        Vector3[] RaycastsDirections;
        //!!!!DELEGATES!!!!
        public delegate void GainSightEvent(GameObject gameObject);
        public delegate void LoseSightEvent(GameObject gameObject);
        public event GainSightEvent OnGainSight;
        public event LoseSightEvent OnLoseSight;
        private SerializableDictionary<GameObject, Coroutine> seenObjectsInSight;
        private Quaternion currentRayGameObjectRotation;
        private void Start()
        {
            seenObjectsInSight = new SerializableDictionary<GameObject, Coroutine>();
            if (!GameObjectForRay)
                GameObjectForRay = gameObject;
            SetRaycastsDeviation();
            OnGainSight += Enemy.HandleGainCreatureInSight;
            OnLoseSight += Enemy.HandleLoseCreatureFromSight;
            Enemy.OnDead += OnDead;
        }
        private void Update()
        {
            for (int i = 0; i < RaycastsDirections.Length; i++)
            {
                if (Physics.Raycast(GameObjectForRay.transform.position + OriginOffset, RaycastsDirections[i] * ViewDistance, out RaycastHit Hit, ViewDistance, ~RayCastIgnore))
                {
                    if (CheckIfNonEqualOrParent(Hit.collider.gameObject, gameObject) && (LayersForProcessing | (1 << Hit.collider.gameObject.layer)) == LayersForProcessing)
                    {
                        if (!seenObjectsInSight.ContainsKey(Hit.collider.gameObject) || seenObjectsInSight[Hit.collider.gameObject] == null)
                        {
                            OnGainSight?.Invoke(Hit.collider.gameObject);
                            seenObjectsInSight[Hit.collider.gameObject] = StartCoroutine(checkGameObjectWhileInSight(Hit.collider.gameObject));
                        }
                    }
                }
            }
        }
        private void FixedUpdate()
        {
            if (currentRayGameObjectRotation != GameObjectForRay.transform.rotation)
            {
                SetRaycastsDeviation();
                currentRayGameObjectRotation = GameObjectForRay.transform.rotation;
            }
        }
        private void OnDrawGizmos()
        {
            if (RaycastsDirections != null)
            {
                Gizmos.color = Color.cyan;
                for (int i = 0; i < RaycastsDirections.Length; i++)
                    Gizmos.DrawRay(GameObjectForRay.transform.position + OriginOffset, RaycastsDirections[i] * ViewDistance);
            }
        }
        [ContextMenu("GenerateNewRayCasts")]
        public void SetRaycastsDeviation()
        {
            //1 ray on zero, half rays to right and another half to left
            int amoutOfRaycasts = (int)(FieldOfView / GapBetweenRay);
            amoutOfRaycasts = amoutOfRaycasts % 2 == 0 ? amoutOfRaycasts + 1 : amoutOfRaycasts;
            if (RaycastsDirections == null)
                RaycastsDirections = new Vector3[amoutOfRaycasts];
            int deviation = 0;
            for (int i = 0; i < RaycastsDirections.Length; i++)
            {
                RaycastsDirections[i] = Quaternion.AngleAxis(deviation, GameObjectForRay.transform.up) * Quaternion.AngleAxis(YDeviation, -GameObjectForRay.transform.right) *
                    GameObjectForRay.transform.forward;
                if (i <= (int)RaycastsDirections.Length / 2)
                    deviation += GapBetweenRay;
                else if (i == ((int)RaycastsDirections.Length / 2) + 1)
                    deviation = -GapBetweenRay;
                else
                    deviation -= GapBetweenRay;
            }
        }
        private void OnDead()
        {
            this.enabled = false;
        }
        private bool CheckIfNonEqualOrParent(GameObject gameObjectThatHasParent, GameObject gameObject)
        {
            if (gameObjectThatHasParent == gameObject)
                return false;
            Transform currentParent = gameObjectThatHasParent.transform.parent;
            while (currentParent != null)
            {
                if (currentParent.gameObject == gameObject)
                    return false;
                currentParent = currentParent.transform.parent;
            }
            return true;
        }
        private IEnumerator checkGameObjectWhileInSight(GameObject gameObject)
        {
            WaitForSeconds Wait = new WaitForSeconds(0.1f);
            while (checkGameObjectInSight(gameObject))
                yield return Wait;
            OnLoseSight?.Invoke(gameObject);
            seenObjectsInSight.Remove(gameObject);
        }
        private bool checkGameObjectInSight(GameObject gameObject)
        {
            Vector3 Direction = (gameObject.transform.position - (GameObjectForRay.transform.position + OriginOffset)).normalized;
            float DotProduct = Vector3.Dot(GameObjectForRay.transform.forward, Direction);
            if (DotProduct >= Mathf.Cos(FieldOfView))
            {
                RaycastHit Hit;
                if (Physics.Raycast(GameObjectForRay.transform.position + OriginOffset, Direction, out Hit, ViewDistance, ~RayCastIgnore))
                {
                    if ((LayersForProcessing | (1 << Hit.collider.gameObject.layer)) == LayersForProcessing)
                        return true;
                }
            }
            return false;
        }
    }
}
