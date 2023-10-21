using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public delegate void DamageWIthData(TakeDamageData takeDamageData);
    public delegate void DamageWithoutData();
    public event DamageWIthData OnTakeDamageWithDamageData;
    public event DamageWithoutData OnTakeDamageWithoutDamageData;
    public event DamageWithoutData OnDeath;
    /// <summary>
    /// Current Health
    /// </summary>
    public float Health = 100;
    public bool InvokeOnTakeDamageAfterDead = true;
    [HideInInspector]
    public float OriginalHealth;
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
    protected void Start()
    {
        OriginalHealth = Health;
    }
    public virtual GameObject GetGameObject()
    {
        return gameObject;
    }
    public virtual void TakeDamage(TakeDamageData takeDamageData)
    {
        TakeDamage(takeDamageData.Damage);
        OnTakeDamageWithDamageData?.Invoke(takeDamageData);
    }
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        OnTakeDamageWithoutDamageData?.Invoke();
        if (Health <= 0)
        {
            IsDead = true;
            if (!InvokeOnTakeDamageAfterDead)
            {
                OnTakeDamageWithDamageData = null;
                OnTakeDamageWithoutDamageData = null;
            }
            OnDeath = null;
        }
    }
}
