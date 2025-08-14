using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the player's acquired skills, their upgrade paths, and the logic for choosing new skills.
/// </summary>
public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    [Tooltip("A list of all possible skill data assets that can be drawn in the game.")]
    public List<SkillData> masterSkillList;

    // This will be replaced by the new PlayerSkillInventory system.
    // private List<SkillData> playerDeck;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// This method will be expanded to handle applying upgrades to the new PlayerSkillInventory.
    /// For now, it's a placeholder.
    /// </summary>
    public void ApplySkillChoice(SkillData skill)
    {
        // TODO: Integrate with PlayerSkillInventory
        // If the player doesn't have this skill, add it to their inventory.
        // If they do have it, this choice represents an upgrade point to be spent.
        Debug.Log($"Skill choice processed: {skill.skillName}");
    }

    /// <summary>
    /// Gets a specified number of random skill options.
    /// This logic will be updated to include both new skills and upgrades for existing skills.
    /// </summary>
    public List<SkillData> GetSkillOptions(int count)
    {
        List<SkillData> chosenSkills = new List<SkillData>();
        List<SkillData> drawPool = new List<SkillData>(masterSkillList); // Create a copy

        // TODO: Add logic to sometimes include upgrade options for already-owned skills.

        for (int i = 0; i < count; i++)
        {
            if (drawPool.Count == 0) break;

            int randomIndex = Random.Range(0, drawPool.Count);
            chosenSkills.Add(drawPool[randomIndex]);
            drawPool.RemoveAt(randomIndex);
        }

        return chosenSkills;
    }
}
