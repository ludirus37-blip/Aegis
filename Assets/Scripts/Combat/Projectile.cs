using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("The forward speed of the projectile.")]
    public float speed = 15f;

    [Tooltip("How long the projectile exists in seconds before being destroyed.")]
    public float lifetime = 3f;

    private float damage;
    private string skillId;
    private Rigidbody2D rb;

    /// <summary>
    /// Initializes the projectile with damage and the ID of the skill that fired it.
    /// </summary>
    public void Initialize(float projectileDamage, string firingSkillId)
    {
        damage = projectileDamage;
        skillId = firingSkillId;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = transform.up * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Health>(out var enemyHealth))
            {
                enemyHealth.TakeDamage(damage);

                if (!string.IsNullOrEmpty(skillId))
                {
                    DamageTracker.RecordDamage(skillId, damage);
                }
            }
            Destroy(gameObject);
        }
    }
}
