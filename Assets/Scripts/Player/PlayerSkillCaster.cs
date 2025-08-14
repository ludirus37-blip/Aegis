using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

/// <summary>
/// Automatically casts active skills from the player's inventory when their cooldowns are up.
/// </summary>
public class PlayerSkillCaster : NetworkBehaviour
{
    private PlayerSkillInventory skillInventory;

    // Tracks the remaining cooldown for each active skill component.
    private Dictionary<ISkill, float> skillCooldowns;

    void Awake()
    {
        skillInventory = GetComponent<PlayerSkillInventory>();
        skillCooldowns = new Dictionary<ISkill, float>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        // Find all components on this object that implement the ISkill interface.
        // This assumes that when a skill is "learned", its logic component is added to the player GameObject.
        ISkill[] skills = GetComponents<ISkill>();
        foreach (var skill in skills)
        {
            if (skill.SkillData != null)
            {
                // Initialize cooldown for each skill.
                skillCooldowns[skill] = skill.SkillData.baseEffects.Find(e => e.key == "cooldown").value;
            }
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        var skills = new List<ISkill>(skillCooldowns.Keys);

        foreach (var skill in skills)
        {
            skillCooldowns[skill] -= Time.deltaTime;

            if (skillCooldowns[skill] <= 0)
            {
                // Activate the skill, passing the inventory for it to read upgrade levels.
                skill.Activate(skillInventory);

                // Reset cooldown based on the skill's data.
                // TODO: This could be modified by cooldown reduction stats.
                float baseCooldown = skill.SkillData.baseEffects.Find(e => e.key == "cooldown").value;
                skillCooldowns[skill] = baseCooldown;
            }
        }
    }
}
