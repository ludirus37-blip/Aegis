using UnityEngine;

/// <summary>
/// The AI and behavior for a summoned Shadow Knight minion.
/// </summary>
public class ShadowKnightMinion : MonoBehaviour
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
    /// </summary>
    public void Initialize(int powerLevel)
    {
        float powerMultiplier = 1f + (powerLevel * 0.15f);
        currentMoveSpeed = baseMoveSpeed * powerMultiplier;
        currentDamage = baseDamage * powerMultiplier;
    }

    void Start()
    {
        // Despawn after the lifetime expires.
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
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
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Health>(out var enemyHealth))
            {
                enemyHealth.TakeDamage(currentDamage);
            }
        }
    }
}
