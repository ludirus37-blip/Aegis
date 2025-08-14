using UnityEngine;
// using TMPro; // Would be used for displaying the text

/// <summary>
/// Controls the behavior of a single damage number popup.
/// It moves up, fades out, and then destroys itself.
/// </summary>
public class DamageNumber : MonoBehaviour
{
    // [Tooltip("The TextMeshPro component used to display the damage number.")]
    // public TextMeshPro textMesh;

    [Tooltip("How fast the number moves upwards.")]
    public float moveSpeed = 2f;
    [Tooltip("How long the number stays visible before fading.")]
    public float lifetime = 1f;
    [Tooltip("How fast the number fades out.")]
    public float fadeSpeed = 2f;

    private float lifeTimer;
    private Color initialColor;

    void Awake()
    {
        // In a real implementation, you would get the TextMeshPro component here.
        // initialColor = textMesh.color;
        lifeTimer = lifetime;
    }

    /// <summary>
    /// Sets the damage value to be displayed.
    /// </summary>
    public void SetValue(float damage)
    {
        // textMesh.text = damage.ToString("F0");
    }

    void Update()
    {
        // Move upwards
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Handle fading
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            // Fade out
            // initialColor.a -= fadeSpeed * Time.deltaTime;
            // textMesh.color = initialColor;

            // if (textMesh.color.a <= 0)
            // {
                // Destroy itself once fully faded.
                Destroy(gameObject);
            // }
        }
    }
}
