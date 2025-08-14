using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Aegis Survivors/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    [Tooltip("The display name of the enemy.")]
    public string enemyName;
    [Tooltip("The main prefab for this enemy, containing visuals and AI components.")]
    public GameObject enemyPrefab;

    [Header("Stats")]
    [Tooltip("The enemy's maximum health.")]
    public float maxHealth = 20f;
    [Tooltip("The enemy's movement speed.")]
    public float moveSpeed = 3f;
    [Tooltip("The damage the enemy deals on contact.")]
    public float damage = 5f;
    [Tooltip("The amount of experience this enemy drops on death.")]
    public int experienceReward = 10;
}
