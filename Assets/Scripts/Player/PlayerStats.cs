using UnityEngine;

/// <summary>
/// Manages player stats.
/// </summary>
public class PlayerStats : MonoBehaviour
{
    [Tooltip("The base hero data asset. Used for initializing stats at the start of a run.")]
    public HeroData baseHeroData;

    // --- Core Combat Stats ---
    public float currentHealth;
    public float maxHealth;
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

    void Awake()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        if (baseHeroData == null)
        {
            Debug.LogError("baseHeroData is not assigned on PlayerStats! Cannot initialize.", this);
            return;
        }

        maxHealth = baseHeroData.maxHealth;
        currentHealth = maxHealth;
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
        // This logic now runs directly in single-player.
        switch (statKey)
        {
            case "maxHealth_add":
                maxHealth += value;
                currentHealth += value;
                break;
            case "maxHealth_mult":
                maxHealth *= (1 + value);
                currentHealth *= (1 + value);
                break;
            // ... other cases ...
            default:
                Debug.LogWarning($"Stat key '{statKey}' not handled by PlayerStats.ModifyStat. Value was {value}.");
                break;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Handle player death logic here...
            Debug.Log("Player has died.");
            Destroy(gameObject);
        }
    }
}
