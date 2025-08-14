using UnityEngine;
using System.Collections.Generic;

// SkillInstanceData remains the same, as it's a plain C# class.
public class SkillInstanceData
{
    public SkillData SkillData { get; private set; }
    public Dictionary<string, int> upgradeLevels;

    public SkillInstanceData(SkillData skillData)
    {
        SkillData = skillData;
        upgradeLevels = new Dictionary<string, int>();
        if (skillData.upgradePaths != null)
        {
            foreach (var path in skillData.upgradePaths)
            {
                upgradeLevels[path.pathName] = 0;
            }
        }
    }

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
/// </summary>
public class PlayerSkillInventory : MonoBehaviour
{
    public readonly Dictionary<SkillData, SkillInstanceData> AcquiredSkills = new Dictionary<SkillData, SkillInstanceData>();

    /// <summary>
    /// Adds a new skill to the inventory.
    /// </summary>
    public void AddSkill(SkillData skillData)
    {
        if (!AcquiredSkills.ContainsKey(skillData))
        {
            AcquiredSkills[skillData] = new SkillInstanceData(skillData);
            Debug.Log($"Acquired new skill: {skillData.skillName}");
        }
    }

    /// <summary>
    /// Adds an upgrade point to a specific path for an existing skill.
    /// </summary>
    /// <returns>True if the upgrade was successful.</returns>
    public bool UpgradeSkill(SkillData skillData, string upgradePathName)
    {
        if (AcquiredSkills.TryGetValue(skillData, out SkillInstanceData instance))
        {
            if (instance.GetTotalPointsInvested() >= 7)
            {
                Debug.LogWarning($"Cannot upgrade {skillData.skillName}. Max total upgrades (7) reached.");
                return false;
            }

            if (instance.upgradeLevels.TryGetValue(upgradePathName, out int currentLevel))
            {
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
