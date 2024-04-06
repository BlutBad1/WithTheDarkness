using UnityEngine;

namespace EntityNS.Attack
{
    public class GramophoneSight : EntitySight
    {
        [SerializeField]
        private SphereCollider sphereCollider;

        protected override void OnEnable()
        {
            base.OnEnable();
            entityBehaviour.OnTakeDamageWithDamageData += GainCreatureInSightOnDamage;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            entityBehaviour.OnTakeDamageWithDamageData -= GainCreatureInSightOnDamage;
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
            Vector3.Distance(transform.position, gameObject.transform.position) <= sphereCollider.radius * 2f;
    }
}