using UnityEngine;

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
    public float critDamageMultiplier; // e.g., 2.0 for 200% damage
    public float areaSizeMultiplier = 1f;
    public int projectilePierce = 0;

    // --- Utility Stats ---
    public float pickupRadius;
    public float cooldownReductionMultiplier = 0f; // as a percentage, e.g., 0.1 for 10%
    public float luck = 0f; // Can influence RNG-based events
    public float xpGainMultiplier = 1f;

    void Awake()
    {
        InitializeStats();
    }

    public void InitializeStats()
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

        // Initialize other stats to their default values
        critChance = 0.05f;
        critDamageMultiplier = 2.0f;
        areaSizeMultiplier = 1f;
        projectilePierce = 0;
        pickupRadius = 2.0f;
        cooldownReductionMultiplier = 0f;
        luck = 0f;
        xpGainMultiplier = 1f;
    }

    /// <summary>
    /// Modifies a player stat based on a key and value from a CardData effect.
    /// This is the central hub for applying all passive card upgrades.
    /// </summary>
    public void ModifyStat(string statKey, float value)
    {
        switch (statKey)
        {
            // Health & Defense
            case "max_health_add":
                maxHealth += value;
                currentHealth += value; // Optionally increase current health too
                break;
            case "max_health_mult":
                maxHealth *= (1 + value);
                currentHealth *= (1 + value);
                break;
            case "defense_add":
                defense += value;
                break;

            // Movement
            case "move_speed_add":
                moveSpeed += value;
                break;
            case "move_speed_mult":
                moveSpeed *= (1 + value);
                break;

            // Attack
            case "damage_add":
                attackDamage += value;
                break;
            case "damage_mult_all":
                attackDamage *= (1 + value);
                break;
            case "attack_speed_mult":
                attackSpeed *= (1 + value);
                break;
            case "crit_chance_add":
                critChance += value;
                break;
            case "crit_damage_mult_add":
                critDamageMultiplier += value;
                break;

            // Projectile / Area
            case "area_size_mult":
                areaSizeMultiplier *= (1 + value);
                break;
            case "projectile_pierce_add":
                projectilePierce += (int)value;
                break;

            // Utility
            case "pickup_radius_add":
                pickupRadius += value;
                break;
            case "cooldown_reduction_mult":
                cooldownReductionMultiplier += value; // Additive percentage
                break;
            case "luck_add":
                luck += value;
                break;
            case "xp_gain_mult":
                xpGainMultiplier *= (1 + value);
                break;

            default:
                Debug.LogWarning($"Stat key '{statKey}' not handled by PlayerStats.ModifyStat. Value was {value}.");
                break;
        }
    }
}
