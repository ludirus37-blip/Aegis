using UnityEngine;
using System.Collections.Generic;

// Using enums for card properties makes the code cleaner and less error-prone.
public enum CardRarity { Common, Rare, Epic, Legendary }
public enum CardType { Active, Passive, Consumable, Summon, Upgrade }

[CreateAssetMenu(fileName = "New CardData", menuName = "Aegis Survivors/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Card Identity")]
    public string cardId; // Unique identifier, e.g., "fireball_01"
    public string cardName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;

    [Header("Card Properties")]
    public CardRarity rarity;
    public CardType type;
    public List<string> tags; // e.g., "Fire", "Frost", "Bleed", "Hero_Sentinel"

    [Header("Gameplay Effects")]
    // A flexible list of key-value pairs to define what the card does.
    // Examples: {key: "moveSpeed_add", value: 1.5}, {key: "damage_multiplier", value: 0.1}
    public List<EffectValue> effectValues;
    public float cooldown; // Cooldown time in seconds for active skills.
    public float duration; // Duration in seconds for temporary effects.

    // A struct to hold effect data, making it easy to manage in the inspector.
    [System.Serializable]
    public struct EffectValue
    {
        public string key;
        public float value;
    }
}
