using UnityEngine;
using Unity.Netcode;

/// <summary>
/// The AI and behavior for a summoned Shadow Knight minion.
/// This is a networked object controlled by the server.
/// </summary>
[RequireComponent(typeof(NetworkObject))]
public class ShadowKnightMinion : NetworkBehaviour
{
    [Header("Base Stats")]
    public float baseMoveSpeed = 4f;
    public float baseDamage = 10f;
    public float lifetime = 15f;

    // Runtime stats modified by player upgrades
    private float currentMoveSpeed;
    private float currentDamage;

    private Transform target;

    /// <summary>
    /// Initializes the minion with stats based on the player's upgrade level.
    /// This should be called by the server immediately after spawning.
    /// </summary>
    /// <param name="powerLevel">The number of points invested in the 'Knight Power' upgrade path.</param>
    public void Initialize(int powerLevel)
    {
        // Example upgrade logic: each point in "Knight Power" increases damage and speed by 15%.
        float powerMultiplier = 1f + (powerLevel * 0.15f);
        currentMoveSpeed = baseMoveSpeed * powerMultiplier;
        currentDamage = baseDamage * powerMultiplier;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Invoke(nameof(Despawn), lifetime);
        }
    }

    void Update()
    {
        if (!IsServer) return;

        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindNearestEnemy();
            if (target == null) return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, currentMoveSpeed * Time.deltaTime);
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistanceSqr = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (var enemy in enemies)
        {
            float distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Health>(out var enemyHealth))
            {
                enemyHealth.TakeDamage(currentDamage);
            }
        }
    }

    private void Despawn()
    {
        if (!IsServer) return;
        GetComponent<NetworkObject>().Despawn();
    }
}
