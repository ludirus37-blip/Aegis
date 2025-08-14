using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the spawning of enemies in the game scene.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Configuration")]
    [Tooltip("A list of enemy prefabs to be spawned.")]
    public GameObject[] enemyPrefabs;
    [Tooltip("The time in seconds between each spawn wave.")]
    public float timeBetweenWaves = 5f;
    [Tooltip("The radius around the player where enemies should spawn.")]
    public float spawnRadius = 20f;

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("EnemySpawner could not find the player! Disabling spawner.");
            enabled = false;
            return;
        }

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("No enemy prefabs assigned to the EnemySpawner!");
            enabled = false;
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (playerTransform == null) return;

        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = playerTransform.position + (Vector3)(spawnDirection * spawnRadius);

        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }
}
