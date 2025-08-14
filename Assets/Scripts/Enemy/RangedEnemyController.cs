using UnityEngine;

/// <summary>
/// Controls a ranged enemy. This AI will attempt to stay at a specific distance
/// from the player and fire projectiles.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class RangedEnemyController : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Data asset defining this enemy's stats.")]
    public EnemyData enemyData;
    [Tooltip("The projectile prefab this enemy fires.")]
    public GameObject projectilePrefab;
    [Tooltip("The transform point from which projectiles are fired.")]
    public Transform firePoint;

    [Header("AI Behavior")]
    [Tooltip("The ideal distance to keep from the player.")]
    public float idealRange = 10f;
    [Tooltip("How close the enemy will get before backing away.")]
    public float minRange = 8f;
    [Tooltip("The time in seconds between each shot.")]
    public float fireRate = 2f;

    private Transform playerTarget;
    private Rigidbody2D rb;
    private float fireTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Debug.LogWarning("RangedEnemy could not find Player. Disabling AI.", this);
            enabled = false;
        }

        fireTimer = fireRate;
    }

    void FixedUpdate()
    {
        if (playerTarget == null || enemyData == null) return;

        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
        Vector2 directionToPlayer = (playerTarget.position - transform.position).normalized;

        Vector2 moveDirection = Vector2.zero;

        if (distanceToPlayer < minRange)
        {
            // Too close, move away
            moveDirection = -directionToPlayer;
        }
        else if (distanceToPlayer > idealRange)
        {
            // Too far, move closer
            moveDirection = directionToPlayer;
        }
        // If in the sweet spot, don't move.

        rb.MovePosition(rb.position + moveDirection * enemyData.moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleShooting()
    {
        fireTimer -= Time.fixedDeltaTime;
        if (fireTimer <= 0)
        {
            fireTimer = fireRate;
            Fire();
        }
    }

    private void Fire()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Aim at the player
        Vector2 direction = (playerTarget.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        // Instantiate the projectile
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
