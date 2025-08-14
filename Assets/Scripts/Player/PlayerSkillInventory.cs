using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

/// <summary>
/// Represents the state of a single skill instance that the player has acquired.
/// Tracks the current level of each upgrade path for that skill.
/// </summary>
public class SkillInstanceData
{
    public SkillData SkillData { get; private set; }
    // Dictionary to store the current level of each upgrade path, keyed by the path name.
    public Dictionary<string, int> upgradeLevels;

    public SkillInstanceData(SkillData skillData)
    {
        SkillData = skillData;
        upgradeLevels = new Dictionary<string, int>();
        if (skillData.upgradePaths != null)
        {
            foreach (var path in skillData.upgradePaths)
            {
                upgradeLevels[path.pathName] = 0; // Initialize all paths at level 0
            }
        }
    }

    /// <summary>
    /// Gets the total number of upgrade points invested in this skill instance.
    /// </summary>
    public int GetTotalPointsInvested()
    {
        int total = 0;
        foreach (var level in upgradeLevels.Values)
        {
            total += level;
        }
        return total;
    }
}

/// <summary>
/// Manages the player's collection of skills acquired during a run.
/// This component is the source of truth for what skills the player owns and their current upgrade levels.
/// It should be placed on the player's NetworkObject.
/// </summary>
public class PlayerSkillInventory : NetworkBehaviour
{
    public readonly Dictionary<SkillData, SkillInstanceData> AcquiredSkills = new Dictionary<SkillData, SkillInstanceData>();

    // Note: For these changes to be visible on other clients, this dictionary would need to be synchronized
    // using a NetworkList<SkillState> and custom serialization. For now, this logic is client-authoritative
    // and would be validated by the server.

    /// <summary>
    /// Adds a new skill to the inventory. Should only be called on the owner's client.
    /// </summary>
    public void AddSkill(SkillData skillData)
    {
        if (!IsOwner) return;

        if (!AcquiredSkills.ContainsKey(skillData))
        {
            AcquiredSkills[skillData] = new SkillInstanceData(skillData);
            Debug.Log($"Acquired new skill: {skillData.skillName}");
            // TODO: Fire an event here to notify other systems (like the UI or SkillCaster)
        }
    }

    /// <summary>
    /// Adds an upgrade point to a specific path for an existing skill.
    /// </summary>
    /// <returns>True if the upgrade was successful.</returns>
    public bool UpgradeSkill(SkillData skillData, string upgradePathName)
    {
        if (!IsOwner) return false;

        if (AcquiredSkills.TryGetValue(skillData, out SkillInstanceData instance))
        {
            // Check if total upgrades are less than the max (7)
            if (instance.GetTotalPointsInvested() >= 7)
            {
                Debug.LogWarning($"Cannot upgrade {skillData.skillName}. Max total upgrades (7) reached.");
                return false;
            }

            if (instance.upgradeLevels.TryGetValue(upgradePathName, out int currentLevel))
            {
                // Find the max level for this path from the SkillData
                int maxLevelForPath = 0;
                foreach(var path in skillData.upgradePaths)
                {
                    if(path.pathName == upgradePathName)
                    {
                        maxLevelForPath = path.maxLevel;
                        break;
                    }
                }

                if (currentLevel < maxLevelForPath)
                {
                    instance.upgradeLevels[upgradePathName]++;
                    Debug.Log($"Upgraded {skillData.skillName} - {upgradePathName} to level {instance.upgradeLevels[upgradePathName]}");
                    // TODO: Fire an event here to notify other systems to update stats/behavior
                    return true;
                }
                else
                {
                    Debug.LogWarning($"Cannot upgrade {upgradePathName} for {skillData.skillName}. Path is already at max level ({maxLevelForPath}).");
                    return false;
                }
            }
        }
        return false;
    }
}
