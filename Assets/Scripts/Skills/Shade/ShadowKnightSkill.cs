using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Implements the logic for the "Shadow Knight" summoning skill.
/// This component would be attached to the Player GameObject.
/// It implements the ISkill interface so the PlayerSkillCaster can activate it.
/// </summary>
public class ShadowKnightSkill : NetworkBehaviour, ISkill
{
    [Header("Skill Configuration")]
    [Tooltip("The SkillData ScriptableObject for Shadow Knight. This is used to get cooldowns and other base stats.")]
    [SerializeField] private SkillData skillData;
    public SkillData SkillData => skillData; // Implementation of the interface property

    [Tooltip("The prefab for the Shadow Knight minion. Must have a NetworkObject.")]
    public GameObject minionPrefab;

    public void Activate(PlayerSkillInventory inventory)
    {
        // Activation is requested by the client's PlayerSkillCaster.
        // We must use a ServerRpc to ask the server to perform the summon.
        if (IsOwner)
        {
            // Pass the upgrade data to the server.
            if (inventory.AcquiredSkills.TryGetValue(SkillData, out var instanceData))
            {
                int amountLevel = instanceData.upgradeLevels["Amount"];
                int powerLevel = instanceData.upgradeLevels["Knight Power"];
                SummonKnightsServerRpc(amountLevel, powerLevel);
            }
        }
    }

    [ServerRpc]
    private void SummonKnightsServerRpc(int amountLevel, int powerLevel)
    {
        if (minionPrefab == null)
        {
            Debug.LogError("Minion prefab is not assigned for ShadowKnightSkill!");
            return;
        }

        // The base amount is 1, plus any points in the "Amount" upgrade path.
        int amountToSummon = 1 + amountLevel;

        Debug.Log($"Server is summoning {amountToSummon} Shadow Knight(s) with power level {powerLevel}.");

        for (int i = 0; i < amountToSummon; i++)
        {
            Vector3 spawnPos = transform.position + (Random.insideUnitSphere * 2f);
            spawnPos.z = 0;

            GameObject minionInstance = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            minionInstance.GetComponent<NetworkObject>().Spawn(true);

            // Initialize the minion with its upgraded stats.
            ShadowKnightMinion minionAI = minionInstance.GetComponent<ShadowKnightMinion>();
            if(minionAI != null)
            {
                minionAI.Initialize(powerLevel);
            }
        }
    }
}
