using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Manages player stats and synchronizes critical values over the network.
/// </summary>
public class PlayerStats : NetworkBehaviour
{
    [Tooltip("The base hero data asset. Used for initializing stats at the start of a run.")]
    public HeroData baseHeroData;

    // --- Core Combat Stats ---
    // We use NetworkVariable for stats that need to be visible to all clients (e.g., for UI).
    // The server has write permission, and clients have read permission by default.
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>();
    public NetworkVariable<float> maxHealth = new NetworkVariable<float>();

    // These stats are modified by cards. For now, we assume they are only relevant on the server
    // where combat calculations happen, but they could be made NetworkVariables if needed.
    public float defense;
    public float moveSpeed;

    // --- Attack Stats ---
    public float attackDamage;
    public float attackSpeed; // Attacks per second
    public float critChance;
    public float critDamageMultiplier;
    public float areaSizeMultiplier = 1f;
    public int projectilePierce = 0;

    // --- Utility Stats ---
    public float pickupRadius;
    public float cooldownReductionMultiplier = 0f;
    public float luck = 0f;
    public float xpGainMultiplier = 1f;

    public override void OnNetworkSpawn()
    {
        // Initialization should only happen on the server, as it's the authority.
        if (IsServer)
        {
            InitializeStats();
        }
    }

    private void InitializeStats()
    {
        if (baseHeroData == null)
        {
            Debug.LogError("baseHeroData is not assigned on PlayerStats! Cannot initialize.", this);
            return;
        }

        maxHealth.Value = baseHeroData.maxHealth;
        currentHealth.Value = maxHealth.Value;
        defense = baseHeroData.defense;
        moveSpeed = baseHeroData.moveSpeed;
        attackDamage = baseHeroData.attackDamage;
        attackSpeed = baseHeroData.attackSpeed;

        critChance = 0.05f;
        critDamageMultiplier = 2.0f;
        areaSizeMultiplier = 1f;
        projectilePierce = 0;
        pickupRadius = 2.0f;
        cooldownReductionMultiplier = 0f;
        luck = 0f;
        xpGainMultiplier = 1f;
    }

    public void ModifyStat(string statKey, float value)
    {
        // Stat modifications should only be processed on the server.
        if (!IsServer) return;

        switch (statKey)
        {
            case "maxHealth_add":
                maxHealth.Value += value;
                currentHealth.Value += value;
                break;
            case "maxHealth_mult":
                maxHealth.Value *= (1 + value);
                currentHealth.Value *= (1 + value);
                break;
            // ... other cases ...
            default:
                Debug.LogWarning($"Stat key '{statKey}' not handled by PlayerStats.ModifyStat. Value was {value}.");
                break;
        }
    }

    // Example of taking damage, which would be called by the server.
    public void TakeDamage(float amount)
    {
        if (!IsServer) return;

        currentHealth.Value -= amount;
        if (currentHealth.Value <= 0)
        {
            currentHealth.Value = 0;
            // Handle player death logic here...
        }
    }
}
