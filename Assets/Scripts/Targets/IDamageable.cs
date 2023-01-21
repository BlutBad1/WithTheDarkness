using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public bool doesHaveKnockout { get; set; }
    public void TakeDamage(int damage, RaycastHit hit);
    public void TakeDamage(int damage);
    Transform GetTransform();
}
