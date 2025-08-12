using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExperienceOrb : MonoBehaviour
{
    [Tooltip("The amount of experience this orb grants to the player.")]
    public int experienceValue = 10;

    // In a future step, a "magnet" effect could be implemented here,
    // making the orb move towards the player when they are in range.

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the orb has collided with the player.
        if (other.CompareTag("Player"))
        {
            LevelingSystem playerLevel = other.GetComponent<LevelingSystem>();
            if (playerLevel != null)
            {
                // Grant experience to the player.
                playerLevel.AddExperience(experienceValue);

                // For performance, this would be returned to an object pool in a full game.
                Destroy(gameObject);
            }
        }
    }
}
