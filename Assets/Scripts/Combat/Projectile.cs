using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("The forward speed of the projectile.")]
    public float speed = 15f;

    [Tooltip("How long the projectile exists in seconds before being destroyed.")]
    public float lifetime = 3f;

    private float damage;
    private Rigidbody2D rb;

    /// <summary>
    /// Initializes the projectile with the damage it will deal.
    /// </summary>
    /// <param name="projectileDamage">The amount of damage.</param>
    public void Initialize(float projectileDamage)
    {
        damage = projectileDamage;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Set the projectile's velocity. It moves "up" relative to its own orientation.
        // This means it will fire in the direction it was spawned facing.
        rb.velocity = transform.up * speed;

        // Destroy the projectile after its lifetime expires to prevent it from flying forever.
        Destroy(gameObject, lifetime);
    }

    // This function is called when the Collider2D other enters the trigger.
    // Ensure the projectile's collider is set to "Is Trigger".
    void OnTriggerEnter2D(Collider2D other)
    {
        // For a prototype, checking tags is sufficient. In a larger project,
        // using layers for collision matrix optimization is recommended.
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Destroy the projectile once it hits an enemy.
            // Later, this could be replaced with an impact effect and object pooling.
            Destroy(gameObject);
        }
    }
}
