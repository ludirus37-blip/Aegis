using UnityEngine;

/// <summary>
/// Controls a simple melee enemy that moves towards the player.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class EnemyController : MonoBehaviour
{
    [Tooltip("Reference to the ScriptableObject defining this enemy's stats.")]
    public EnemyData enemyData;

    private Rigidbody2D rb;
    private Transform playerTarget;
    private Health health;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
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
            if (collision.gameObject.TryGetComponent<Health>(out var playerHealth))
            {
                playerHealth.TakeDamage(enemyData.damage);
            }
        }
    }
}
