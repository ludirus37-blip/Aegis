using UnityEngine;
using Unity.Netcode;
using System.Collections;

/// <summary>
/// Manages the spawning of enemies in the game scene.
/// This script must be run on the server.
/// </summary>
public class EnemySpawner : NetworkBehaviour
{
    [Header("Spawning Configuration")]
    [Tooltip("A list of enemy prefabs to be spawned. These prefabs MUST have a NetworkObject component.")]
    public GameObject[] enemyPrefabs;
    [Tooltip("The time in seconds between each spawn wave.")]
    public float timeBetweenWaves = 5f;
    [Tooltip("The radius around the player where enemies should spawn.")]
    public float spawnRadius = 20f;

    private Transform playerTransform;

    public override void OnNetworkSpawn()
    {
        // Only the server should run the spawner logic.
        if (!IsServer)
        {
            enabled = false;
            return;
        }

        // Find the player to spawn enemies around them.
        // In a real multiplayer game, this would need to handle multiple players.
        // For now, we'll just find the first one.
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

        // Start the spawning coroutine.
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

        // Instantiate the enemy and spawn it on the network.
        GameObject enemyInstance = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        enemyInstance.GetComponent<NetworkObject>().Spawn(true);

        Debug.Log($"Spawned a networked enemy at {spawnPosition}");
    }
}
