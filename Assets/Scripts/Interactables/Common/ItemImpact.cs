using UnityEngine;

namespace InteractableNS.Common
{


    [RequireComponent(typeof(ItemDamagable), typeof(Rigidbody))]
    public class ItemImpact : MonoBehaviour
    {
        ItemDamagable itemDamagable;
        Rigidbody rigidbody;
        public float ImpactForce = 1f;
        void Start()
        {
            if (!itemDamagable)
                itemDamagable = GetComponent<ItemDamagable>();

            if (!rigidbody)
                rigidbody = GetComponent<Rigidbody>();
            itemDamagable.OnTakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(float force, Vector3 hit)
        {
            Vector3 moveDirection = transform.position - hit;
            rigidbody.AddForce(moveDirection.normalized * force * ImpactForce, ForceMode.Impulse);
        }
    }
}