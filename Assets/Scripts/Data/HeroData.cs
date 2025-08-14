using UnityEngine;

[CreateAssetMenu(fileName = "New HeroData", menuName = "Aegis Survivors/Hero Data")]
public class HeroData : ScriptableObject
{
    [Header("Info")]
    [Tooltip("The display name of the hero.")]
    public string heroName;
    [Tooltip("A short description or flavor text for the hero.")]
    public string description;
    [Tooltip("The main prefab for this hero, containing all visuals and necessary components.")]
    public GameObject heroPrefab;

    [Header("Meta")]
    [Tooltip("The cost in persistent currency to unlock this hero.")]
    public int unlockCost = 100;
    [Tooltip("Is this hero unlocked from the start of the game?")]
    public bool unlockedByDefault = false;

    [Header("Base Stats")]
    [Tooltip("The hero's starting maximum health.")]
    public float maxHealth = 150f;
    [Tooltip("The hero's base movement speed.")]
    public float moveSpeed = 4.5f;
    [Tooltip("The hero's starting defense value.")]
    public float defense = 10f;
    [Tooltip("The hero's base attack damage.")]
    public float attackDamage = 10f;
    [Tooltip("The hero's base attacks per second.")]
    public float attackSpeed = 1f;
}
