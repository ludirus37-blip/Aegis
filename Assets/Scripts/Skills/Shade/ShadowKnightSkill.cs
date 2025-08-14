using UnityEngine;

/// <summary>
/// Implements the logic for the "Shadow Knight" summoning skill.
/// This component would be attached to the Player GameObject.
/// It implements the ISkill interface so the PlayerSkillCaster can activate it.
/// </summary>
public class ShadowKnightSkill : MonoBehaviour, ISkill
{
    [Header("Skill Configuration")]
    [Tooltip("The SkillData ScriptableObject for Shadow Knight.")]
    [SerializeField] private SkillData skillData;
    public SkillData SkillData => skillData;

    [Tooltip("The prefab for the Shadow Knight minion.")]
    public GameObject minionPrefab;

    public void Activate(PlayerSkillInventory inventory)
    {
        if (minionPrefab == null)
        {
            Debug.LogError("Minion prefab is not assigned for ShadowKnightSkill!");
            return;
        }

        // --- Read Upgrade Data ---
        int amountToSummon = 1; // Base amount
        int powerLevel = 0;
        if (inventory.AcquiredSkills.TryGetValue(SkillData, out var instanceData))
        {
            amountToSummon += instanceData.upgradeLevels["Amount"];
            powerLevel = instanceData.upgradeLevels["Knight Power"];
        }

        Debug.Log($"Summoning {amountToSummon} Shadow Knight(s) with power level {powerLevel}.");

        for (int i = 0; i < amountToSummon; i++)
        {
            Vector3 spawnPos = transform.position + (Random.insideUnitSphere * 2f);
            spawnPos.z = 0;

            GameObject minionInstance = Instantiate(minionPrefab, spawnPos, Quaternion.identity);

            ShadowKnightMinion minionAI = minionInstance.GetComponent<ShadowKnightMinion>();
            if(minionAI != null)
            {
                minionAI.Initialize(powerLevel);
            }
        }
    }
}
