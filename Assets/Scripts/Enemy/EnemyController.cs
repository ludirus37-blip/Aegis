using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Controls a simple melee enemy that moves towards the player.
/// This component should be on a prefab with NetworkObject and NetworkTransform components.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class EnemyController : NetworkBehaviour
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

    public override void OnNetworkSpawn()
    {
        // Only run AI logic on the server. Clients will receive position updates via NetworkTransform.
        if (!IsServer)
        {
            enabled = false;
            return;
        }

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
        // This code now only runs on the server.
        if (playerTarget != null && enemyData != null)
        {
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * enemyData.moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Collision logic should also be server-only to ensure authoritative damage.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // In a real game, damage would be handled through a server RPC or by checking
            // the Health component on the other object, which would also be network-aware.
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Assuming Health component handles damage in a networked way.
                // playerHealth.TakeDamage(enemyData.damage);
            }
        }
    }
}
