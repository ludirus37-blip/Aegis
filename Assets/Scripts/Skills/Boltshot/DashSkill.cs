using UnityEngine;

/// <summary>
/// Placeholder for Boltshot's unique hero skill.
/// This would execute a quick dash in the direction the player is moving.
/// </summary>
public class DashSkill : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;

    /// <summary>
    /// This method would be called to activate the dash.
    /// </summary>
    public void Activate()
    {
        Debug.Log("Boltshot uses Dash! (Movement and VFX logic would execute here)");

        // Example logic:
        // 1. Get the player's current movement direction.
        // 2. Quickly move the player character forward by `dashDistance` over `dashDuration`.
        // 3. Potentially make the player invulnerable during the dash.
        // 4. Instantiate a trail or dash visual effect.
    }
}
