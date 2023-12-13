using DamageableNS.OnTakeDamage;
using UnityEngine;

namespace InteractableNS.Common
{
    [RequireComponent(typeof(ItemDamagable))]
    public class ItemImpact : PushDamageable
    {
    }
}