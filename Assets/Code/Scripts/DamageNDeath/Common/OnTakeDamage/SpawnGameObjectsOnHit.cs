using DamageableNS.OnActions;
using UnityEngine;
namespace DamageableNS.OnTakeDamage
{
    public class SpawnGameObjectsOnHit : DamageablesEvents
    {
        public GameObject[] GameObjectsToSpawn;
        protected override void Start()
        {
            base.Start();
            OnTakeDamageWithDataEvent += OnTakeDamage;
        }
        protected void OnTakeDamage(TakeDamageData takeDamageData)
        {
            for (int i = 0; i < GameObjectsToSpawn.Length; i++)
            {
                Vector3 projectedDirection = Vector3.up - Vector3.Dot(Vector3.up, takeDamageData.HitData.HitPoint.normal) * takeDamageData.HitData.HitPoint.normal;
                Quaternion targetRotation = Quaternion.LookRotation(takeDamageData.HitData.HitPoint.normal, projectedDirection.normalized);

                if (GameObjectsToSpawn[i].scene == null)
                    GameObjectsToSpawn[i] = GameObject.Instantiate(GameObjectsToSpawn[i], takeDamageData.HitData.HitPoint.point, targetRotation, takeDamageData.HitData.HitPoint.transform.parent);
                else
                {
                    GameObjectsToSpawn[i].transform.position = takeDamageData.HitData.HitPoint.point;
                    GameObjectsToSpawn[i].transform.rotation = targetRotation;
                    GameObjectsToSpawn[i].transform.parent = takeDamageData.HitData.HitPoint.transform.parent;
                }
            }
        }
    }
}