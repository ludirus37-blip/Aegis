using UnityEngine;
using System.Collections;

/// <summary>
/// Handles visual feedback for an enemy, such as flashing red and spawning damage numbers.
/// This component should be on the same GameObject as the enemy's Health script.
/// </summary>
[RequireComponent(typeof(Health))]
public class EnemyFeedback : MonoBehaviour
{
    [Header("Assets")]
    [Tooltip("The prefab for the damage number popup.")]
    public GameObject damageNumberPrefab;
    [Tooltip("The SpriteRenderer for this enemy.")]
    public SpriteRenderer spriteRenderer;

    [Header("Feedback Configuration")]
    [Tooltip("The color the sprite flashes when hit.")]
    public Color flashColor = Color.red;
    [Tooltip("How long the flash effect lasts.")]
    public float flashDuration = 0.1f;

    private Health health;
    private Color originalColor;

    void Awake()
    {
        health = GetComponent<Health>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void OnEnable()
    {
        // Subscribe to the damage event
        health.OnDamaged += HandleDamageTaken;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        health.OnDamaged -= HandleDamageTaken;
    }

    private void HandleDamageTaken(float damageAmount)
    {
        // This logic runs on the server where the event is invoked.
        // We need to tell the clients to play the effect using a ClientRpc.
        ShowFeedbackClientRpc(damageAmount);
    }

    [ClientRpc]
    private void ShowFeedbackClientRpc(float damageAmount)
    {
        // This method will be executed on all clients, including the host.

        // 1. Spawn the damage number
        if (damageNumberPrefab != null)
        {
            GameObject numberInstance = Instantiate(damageNumberPrefab, transform.position, Quaternion.identity);
            numberInstance.GetComponent<DamageNumber>()?.SetValue(damageAmount);
        }

        // 2. Trigger the color flash
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}
