using AYellowpaper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DamageableNS.OnTakeDamage
{
    public class RagdollTakeDamage : MonoBehaviour
    {
        public List<InterfaceReference<IDamageable, MonoBehaviour>> Damageables;
        public PushDamageable PushDamageable;
        public UnityEvent EventOnTakeDamage;
        private void Start()
        {
            foreach (var damageable in Damageables)
                damageable.Value.OnTakeDamageWithDamageData += OnTakeDamage;
        }
        protected void OnTakeDamage(TakeDamageData takeDamageData)
        {
            EventOnTakeDamage?.Invoke();
            Rigidbody rigidbody = takeDamageData.DamagedObject.GetGameObject().GetComponent<Rigidbody>();
            PushDamageable?.PushRigidbody(rigidbody, takeDamageData);
        }
    }
}