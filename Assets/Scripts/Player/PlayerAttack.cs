using UnityEngine;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("Reference to the PlayerStats component, which provides dynamic stats like attack speed and damage.")]
    public PlayerStats playerStats;

    [Tooltip("The prefab for the projectile to be fired.")]
    public GameObject projectilePrefab;

    [Tooltip("The point from which projectiles are spawned. This will be rotated to aim at enemies.")]
    public Transform firePoint;

    [Tooltip("The range within which the player will auto-target enemies.")]
    public float targetingRange = 15f;

    private float attackTimer;

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
        attackTimer -= Time.deltaTime;

        Transform target = FindNearestEnemy();
        if (target != null)
        {
            AimAtTarget(target);

            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = 1f / playerStats.attackSpeed;
            }
        }
    }

    private Transform FindNearestEnemy()
    {
        // Note: FindGameObjectsWithTag can be inefficient. For a production game,
        // a more optimized enemy tracking system (e.g., a manager with a registered list) would be better.
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
        // The angle needs to be adjusted by -90 degrees because in Unity, 0 degrees on the Z axis points "up" (Vector2.up).
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Attack()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            // Use the dynamic attackDamage from PlayerStats.
            projectile.Initialize(playerStats.attackDamage);
        }
    }
}
