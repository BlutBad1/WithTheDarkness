using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public delegate void TakeDamageEvent(float damage, float force, Vector3 hit);
    public delegate void DeathEvent();
    public event TakeDamageEvent OnTakeDamage;
    public event DeathEvent OnDeath;
    /// <summary>
    /// Current Health
    /// </summary>
    public float Health = 100;
    [HideInInspector]
    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; if (isDead) OnDeath?.Invoke(); }
    }

    /// <summary>
    /// Health on Start
    /// </summary>
    [HideInInspector]
    public float OriginalHealth;
    private void Start()
    {
        OriginalHealth = Health;
    }
    public virtual Transform GetTransform()
    {
        return transform;
    }
    public virtual void TakeDamage(float damage, float force, Vector3 hit)
    {
        TakeDamage(damage);
        OnTakeDamage?.Invoke(damage, force, hit);
    }
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            IsDead = true;
            OnTakeDamage = null;
            OnDeath = null;
        }
    }
}
