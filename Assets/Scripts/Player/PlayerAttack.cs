using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class PlayerAttack : NetworkBehaviour
{
    [Tooltip("Reference to the PlayerStats component, which provides dynamic stats like attack speed and damage.")]
    public PlayerStats playerStats;

    [Tooltip("The prefab for the projectile to be fired. This should have a Projectile component.")]
    public GameObject projectilePrefab;

    [Tooltip("The point from which projectiles are spawned. This will be rotated to aim at enemies.")]
    public Transform firePoint;

    [Tooltip("The range within which the player will auto-target enemies.")]
    public float targetingRange = 15f;

    private float attackTimer;

    // A hardcoded ID for the player's basic attack for damage tracking.
    private const string BasicAttackSkillId = "Basic Attack";

    void Start()
    {
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats not found on the Player. Disabling PlayerAttack component.", this);
                enabled = false;
                return;
            }
        }
        attackTimer = 0f;
    }

    void Update()
    {
        // Only the owner of the object should be able to attack.
        if (!IsOwner) return;

        attackTimer -= Time.deltaTime;

        Transform target = FindNearestEnemy();
        if (target != null)
        {
            AimAtTarget(target);

            if (attackTimer <= 0f)
            {
                // Request the server to fire the attack
                FireServerRpc();
                attackTimer = 1f / playerStats.attackSpeed;
            }
        }
    }

    [ServerRpc]
    private void FireServerRpc()
    {
        Attack();
    }

    private void Attack()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Spawn the projectile on the network
        projectileGO.GetComponent<NetworkObject>().Spawn(true);

        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            // Initialize with damage and the ID for tracking.
            projectile.Initialize(playerStats.attackDamage, BasicAttackSkillId);
        }
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float minDistanceSqr = targetingRange * targetingRange;

        foreach (GameObject enemy in enemies)
        {
            float distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distanceSqr < minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                nearestEnemy = enemy.transform;
            }
        }
        return nearestEnemy;
    }

    private void AimAtTarget(Transform target)
    {
        if (firePoint == null) return;
        Vector2 direction = (target.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
