using UnityEngine;

/// <summary>
/// Placeholder for Harbinger's unique hero skill.
/// This would create a persistent aura that heals nearby allies over time.
/// In a single-player context, it would heal the player.
/// </summary>
public class HealingAura : MonoBehaviour
{
    public float healPerSecond = 10f;
    public float auraRadius = 6f;

    void Update()
    {
        // This is a passive aura, so its logic would run continuously.
        ApplyHealing();
    }

    private void ApplyHealing()
    {
        // In a multiplayer game, this logic would be more complex.
        // It would need to find all allied players within the auraRadius.
        // For now, we can assume it just heals the player running this script.

        // Example logic:
        // Find player(s) within `auraRadius`.
        // Get their Health component.
        // Heal them for `healPerSecond * Time.deltaTime`.
        // (Note: A real implementation would likely use a fixed-tick timer instead of Update).
    }

    public void Activate()
    {
        // This might be used to turn the aura on or off, or perhaps to provide an initial burst of healing.
        Debug.Log("Harbinger's Healing Aura is active!");
    }
}
