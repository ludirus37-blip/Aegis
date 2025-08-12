using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Aegis Survivors/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public string enemyName;
    public GameObject enemyPrefab;

    [Header("Stats")]
    public float maxHealth = 20f;
    public float moveSpeed = 3f;
    public float damage = 5f;
    public int experienceReward = 10;
}
