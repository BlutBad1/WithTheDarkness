using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
   

    public int Damage = 10;
    public float AttackDelay = 0.5f;
    public float AttackDistance = 2f;
    public float AttackRadius = 2f;
    public delegate void AttackEvent(IDamageable Target);
    public AttackEvent OnAttack;
    protected IDamageable damageable;
    protected Coroutine attackCoroutine;


    protected virtual IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        while (damageable != null)
        {

            OnAttack?.Invoke(damageable);
            damageable.TakeDamage(Damage);
            yield return Wait;
        }

    }

}
