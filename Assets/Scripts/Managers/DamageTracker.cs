using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class to track damage dealt by various skills throughout a run.
/// This provides a centralized place to record and retrieve performance statistics.
/// </summary>
public static class DamageTracker
{
    // A dictionary to store the total damage dealt by each skill, keyed by skill ID or name.
    private static Dictionary<string, float> damageBySkill = new Dictionary<string, float>();

    /// <summary>
    /// Call this method whenever a skill deals damage to an enemy.
    /// </summary>
    /// <param name="skillId">The unique identifier of the skill that dealt the damage.</param>
    /// <param name="amount">The amount of damage dealt.</param>
    public static void RecordDamage(string skillId, float amount)
    {
        if (damageBySkill.ContainsKey(skillId))
        {
            damageBySkill[skillId] += amount;
        }
        else
        {
            damageBySkill[skillId] = amount;
        }
    }

    /// <summary>
    /// Retrieves the current damage statistics.
    /// </summary>
    /// <returns>A dictionary containing the total damage for each skill.</returns>
    public static Dictionary<string, float> GetDamageStats()
    {
        // Return a copy to prevent external modification.
        return new Dictionary<string, float>(damageBySkill);
    }

    /// <summary>
    /// Resets all damage tracking data. Should be called at the start of a new run.
    /// </summary>
    public static void Reset()
    {
        damageBySkill.Clear();
        Debug.Log("Damage tracker has been reset.");
    }
}
