using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float currentHealth;
    private float maxHealthValue;

    public event Action<float> OnDamaged;
    public event Action OnDeath;

    /// <summary>
    /// Initializes the health component.
    /// </summary>
    public void Initialize(float maxHealth)
    {
        maxHealthValue = maxHealth;
        currentHealth = maxHealthValue;
    }

    /// <summary>
    /// Reduces the entity's health.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (amount < 0 || currentHealth <= 0) return;

        currentHealth -= amount;

        OnDamaged?.Invoke(amount);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
