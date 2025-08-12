using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class EnemyController : MonoBehaviour
{
    [Tooltip("Reference to the ScriptableObject defining this enemy's stats.")]
    public EnemyData enemyData;
    [Tooltip("The prefab for the experience orb this enemy drops on death.")]
    public GameObject experienceOrbPrefab;

    private Rigidbody2D rb;
    private Transform playerTarget;
    private Health health;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        // Subscribe to the death event to handle logic when health reaches zero.
        health.OnDeath += HandleDeath;
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Enemy AI could not find a GameObject with the 'Player' tag. Disabling AI.", this);
            enabled = false;
            return;
        }

        if (enemyData != null)
        {
            health.Initialize(enemyData.maxHealth);
        }
        else
        {
            Debug.LogError("EnemyData has not been assigned in the inspector. Disabling AI.", this);
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (playerTarget != null && enemyData != null)
        {
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * enemyData.moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyData.damage);
            }
        }
    }

    private void HandleDeath()
    {
        // When the enemy dies, instantiate an experience orb.
        if (experienceOrbPrefab != null && enemyData != null)
        {
            GameObject orb = Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);

            // Set the orb's experience value based on the enemy's data.
            ExperienceOrb expOrb = orb.GetComponent<ExperienceOrb>();
            if (expOrb != null)
            {
                expOrb.experienceValue = enemyData.experienceReward;
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event when the enemy is destroyed to prevent memory leaks.
        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }
    }
}
