using UnityEngine;

/// <summary>
/// Placeholder for Flare's unique hero skill.
/// This would unleash a wave of fire, damaging all nearby enemies.
/// The actual logic, damage values, and visual effects would be implemented here.
/// </summary>
public class FireNovaSkill : MonoBehaviour
{
    public float radius = 5f;
    public float damage = 40f;
    public float dotDamage = 5f;
    public float dotDuration = 3f;

    /// <summary>
    /// This method would be called to activate the skill.
    /// </summary>
    public void Activate()
    {
        Debug.Log("Flare uses Fire Nova! (VFX and damage logic would execute here)");

        // Example logic:
        // 1. Find all enemies within the radius.
        // 2. Apply initial `damage` to them.
        // 3. Apply a damage-over-time effect using `dotDamage` and `dotDuration`.
        // 4. Instantiate a fire nova visual effect prefab.
    }
}
