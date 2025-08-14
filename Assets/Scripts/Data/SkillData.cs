using UnityEngine;
using System.Collections.Generic;

// Enums to define the new skill taxonomy
public enum SkillType { Spell, Summoning, Blunt, Ranged, Aura, Utility }
public enum Affinity { None, Fire, Frost, Lightning, Shadow, Holy, Physical, Poison }

/// <summary>
/// A data structure to define a single path in a skill's upgrade tree.
/// </summary>
[System.Serializable]
public struct UpgradePath
{
    [Tooltip("The name of the upgrade path, e.g., 'Damage' or 'Cooldown'.")]
    public string pathName;
    [Tooltip("The maximum number of points that can be invested in this path.")]
    public int maxLevel;
    [Tooltip("A description of what this path upgrades.")]
    public string description;
}

[CreateAssetMenu(fileName = "New SkillData", menuName = "Aegis Survivors/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Skill Identity")]
    [Tooltip("Unique identifier for this skill, e.g., 'fireball_01'.")]
    public string skillId;
    [Tooltip("The display name of the skill.")]
    public string skillName;
    [Tooltip("The description of what the skill does, shown to the player.")]
    [TextArea(3, 5)]
    public string description;
    [Tooltip("The icon image for this skill.")]
    public Sprite icon;

    [Header("Skill Taxonomy")]
    [Tooltip("The functional type of the skill.")]
    public SkillType type;
    [Tooltip("The elemental or damage affinity of the skill.")]
    public Affinity affinity;

    [Header("Meta")]
    [Tooltip("Is this skill available in the initial pool or must it be unlocked?")]
    public bool isUnlocked = true;

    [Header("Upgrade System")]
    [Tooltip("The defined upgrade paths for this skill. A skill can have a maximum of 7 total upgrade points across all paths.")]
    public List<UpgradePath> upgradePaths;

    [Header("Gameplay Effects")]
    [Tooltip("The initial effects of the skill at level 1.")]
    public List<EffectValue> baseEffects;
    [Tooltip("The effects gained for each point invested in an upgrade path. The key should match an Upgrade Path's effectKey.")]
    public List<EffectValue> perUpgradeEffects;

    [System.Serializable]
    public struct EffectValue
    {
        public string key;
        public float value;
    }
}
