using AYellowpaper;
using UnityEngine;

namespace DamageableNS.OnTakeDamage
{
    public class PushDamageable : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(IDamageable))]
        private MonoBehaviour damageable;
        public Rigidbody Rigidbody;
        public float PushForce = 1f;
        public IDamageable Damageable { get => (IDamageable)damageable; set => damageable = (MonoBehaviour)value; }
        void Start()
        {
            if (Damageable == null)
                Damageable = GetComponent<IDamageable>();
            if (!Rigidbody)
                Rigidbody = GetComponent<Rigidbody>();
            if (Damageable != null)
                Damageable.OnTakeDamageWithDamageData += OnTakeDamage;
        }
        protected virtual void OnTakeDamage(TakeDamageData takeDamageData)
        {
            PushRigidbody(Rigidbody, takeDamageData);
        }
        public void PushRigidbody(Rigidbody rigidbody, TakeDamageData takeDamageData)
        {
            if (takeDamageData.HitData !=null)
            {
                Vector3 moveDirection = transform.position - takeDamageData.FromGameObject.transform.position;
                rigidbody.AddForce(moveDirection.normalized * PushForce * takeDamageData.Force, ForceMode.Impulse);
            }
        }
    }

}