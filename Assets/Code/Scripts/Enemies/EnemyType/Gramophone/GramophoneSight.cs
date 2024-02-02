using UnityEngine;

namespace EnemyNS.Attack
{
    public class GramophoneSight : EnemySight
    {
        [SerializeField]
        private SphereCollider sphereCollider;

        protected override void OnEnable()
        {
            base.OnEnable();
            enemy.OnTakeDamageWithDamageData += GainCreatureInSightOnDamage;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            enemy.OnTakeDamageWithDamageData -= GainCreatureInSightOnDamage;
        }
        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            HandleGameObject(other.gameObject);
        }
        private void GainCreatureInSightOnDamage(DamageableNS.TakeDamageData takeDamageData)
        {
            HandleGameObject(takeDamageData.FromGameObject);
        }
        protected override bool CheckGameObjectInSight(GameObject gameObject) =>
            Vector3.Distance(transform.position, gameObject.transform.position) <= sphereCollider.radius * 1.1f;
    }
}