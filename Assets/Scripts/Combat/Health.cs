using UnityEngine;
using System;
using Unity.Netcode;

public class Health : NetworkBehaviour
{
    // Using a NetworkVariable to synchronize health across the network.
    // Only the server can write to it.
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>();
    private float maxHealthValue;

    // Server-side event for when damage is taken. This will be used for effects.
    public event Action<float> OnDamaged;
    // Server-side event for when the entity dies.
    public event Action OnDeath;

    public override void OnNetworkSpawn()
    {
        // This is a simple way to ensure maxHealth is set on clients.
        // A more robust system might use an RPC to send the max health value.
        if (IsClient)
        {
            // Client doesn't know the max health yet, so we can't calculate a percentage.
            // A UI script would subscribe to currentHealth.OnValueChanged to update the health bar.
        }
    }

    /// <summary>
    /// Initializes the health component. Should only be called on the server.
    /// </summary>
    public void Initialize(float maxHealth)
    {
        if (!IsServer) return;

        maxHealthValue = maxHealth;
        currentHealth.Value = maxHealthValue;
    }

    /// <summary>
    /// Reduces the entity's health. Should only be called on the server.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (!IsServer) return;

        if (amount < 0 || currentHealth.Value <= 0) return;

        currentHealth.Value -= amount;

        // Invoke the server-side damage event for feedback systems.
        OnDamaged?.Invoke(amount);

        if (currentHealth.Value <= 0)
        {
            currentHealth.Value = 0;
            Die();
        }
    }

    private void Die()
    {
        // This method is only called on the server.
        OnDeath?.Invoke();

        // In a real game, object pooling would be used here instead of Destroy
        // to improve performance by recycling game objects.
        // Destroying a NetworkObject must be done by the server.
        Destroy(gameObject);
    }
}
