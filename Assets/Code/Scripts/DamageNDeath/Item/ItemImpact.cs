using UnityEngine;

namespace InteractableNS.Common
{
    [RequireComponent(typeof(ItemDamagable))]
    public class ItemImpact : MonoBehaviour
    {
        public Rigidbody Rigidbody;
        public float ImpactForce = 1f;
        ItemDamagable itemDamagable;
        void Start()
        {
            if (!itemDamagable)
                itemDamagable = GetComponent<ItemDamagable>();
            if (!Rigidbody)
                Rigidbody = GetComponent<Rigidbody>();
            itemDamagable.OnTakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(float force, Vector3 hit)
        {
            Vector3 moveDirection = transform.position - hit;
            Rigidbody.AddForce(moveDirection.normalized * force * ImpactForce, ForceMode.Impulse);
        }
    }
}