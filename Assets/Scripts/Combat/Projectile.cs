using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : NetworkBehaviour
{
    [Tooltip("The forward speed of the projectile.")]
    public float speed = 15f;

    [Tooltip("How long the projectile exists in seconds before being destroyed.")]
    public float lifetime = 3f;

    private float damage;
    private string skillId; // To identify which skill fired this projectile
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
        // Damage logic should only execute on the server.
        if (!IsServer) return;

        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);

                // Record the damage dealt.
                if (!string.IsNullOrEmpty(skillId))
                {
                    DamageTracker.RecordDamage(skillId, damage);
                }
            }

            // Destroy the projectile on impact. This should also be handled on the server
            // and synced to clients via NetworkObject.Destroy().
            Destroy(gameObject);
        }
    }
}
