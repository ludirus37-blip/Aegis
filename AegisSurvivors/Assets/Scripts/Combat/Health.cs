using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    private float _maxHealth;
    private float _currentHealth;

    // Public properties to safely access health values
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    // Events for other systems to subscribe to
    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged; // Sends currentHealth, maxHealth

    /// <summary>
    /// Initializes the health component with a maximum value.
    /// This should be called by the script that sets up the character (e.g., from a ScriptableObject).
    /// </summary>
    /// <param name="maxHealthValue">The maximum health for this entity.</param>
    public void Initialize(float maxHealthValue)
    {
        _maxHealth = maxHealthValue;
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    /// <summary>
    /// Reduces the entity's health by a given amount.
    /// </summary>
    /// <param name="amount">The amount of damage to take.</param>
    public void TakeDamage(float amount)
    {
        // Ignore negative damage or damage taken after death
        if (amount < 0 || _currentHealth <= 0) return;

        _currentHealth -= amount;
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        // Notify any listeners that this entity has died.
        OnDeath?.Invoke();

        // In a real game, object pooling would be used here instead of Destroy
        // to improve performance by recycling game objects.
        Destroy(gameObject);
    }
}
