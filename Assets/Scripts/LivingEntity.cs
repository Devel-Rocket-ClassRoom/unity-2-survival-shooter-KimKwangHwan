using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float Health { get; private set; }
    public bool IsDead { get { return Health <= 0; } }

    public UnityEvent OnDead;

    protected virtual void OnEnable()
    {
        Health = startingHealth;
    }

    public void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Health -= damage;
        if (IsDead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        OnDead?.Invoke();
    }
}
