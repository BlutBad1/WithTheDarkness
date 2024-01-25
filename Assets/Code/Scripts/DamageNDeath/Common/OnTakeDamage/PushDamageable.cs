using AYellowpaper;
using UnityEngine;

namespace DamageableNS.OnTakeDamage
{
    public class PushDamageable : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(IDamageable))]
        private MonoBehaviour damageable;
        [SerializeField]
        private Rigidbody damageableRigidbody;
        [SerializeField]
        private float pushForce = 1f;

        public float PushForce { get => pushForce; set => pushForce = value; }
        protected Rigidbody DamageableRigidbody { get => damageableRigidbody; set => damageableRigidbody = value; }
        protected IDamageable Damageable { get => (IDamageable)damageable; set => damageable = (MonoBehaviour)value; }

        private void Start()
        {
            if (Damageable == null)
                Damageable = GetComponent<IDamageable>();
            if (!DamageableRigidbody)
                DamageableRigidbody = GetComponent<Rigidbody>();
            if (Damageable != null)
                Damageable.OnTakeDamageWithDamageData += OnTakeDamage;
        }
        protected virtual void OnTakeDamage(TakeDamageData takeDamageData) =>
            PushRigidbody(DamageableRigidbody, takeDamageData);
        public void PushRigidbody(Rigidbody rigidbody, TakeDamageData takeDamageData)
        {
            if (takeDamageData.HitData != null)
            {
                Vector3 moveDirection = transform.position - takeDamageData.FromGameObject.transform.position;
                rigidbody.AddForce(moveDirection.normalized * PushForce * takeDamageData.Force, ForceMode.Impulse);
            }
        }
    }

}