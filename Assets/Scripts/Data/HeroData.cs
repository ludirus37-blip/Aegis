using UnityEngine;

[CreateAssetMenu(fileName = "New HeroData", menuName = "Aegis Survivors/Hero Data")]
public class HeroData : ScriptableObject
{
    [Header("Info")]
    public string heroName;
    public string description;
    public GameObject heroPrefab;

    [Header("Base Stats")]
    public float maxHealth = 150f; // Sentinel is tanky
    public float moveSpeed = 4.5f; // Slightly slower
    public float defense = 10f;    // Higher defense
    public float attackDamage = 10f;
    public float attackSpeed = 1f; // Attacks per second
}
