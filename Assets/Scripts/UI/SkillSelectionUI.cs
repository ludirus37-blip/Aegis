using UnityEngine;
using System.Collections.Generic;

public class SkillSelectionUI : MonoBehaviour
{
    [Tooltip("Assign the UI panel GameObjects that represent the skill choices.")]
    public GameObject[] skillOptionSlots;

    public void DisplaySkillOptions()
    {
        // Get 3 random skill options from the SkillManager.
        List<SkillData> options = SkillManager.Instance.GetSkillOptions(3);

        if (options.Count == 0)
        {
            Debug.LogWarning("No skill options were available to display.");
            GameManager.Instance.ResumeGameAfterSelection();
            return;
        }

        for (int i = 0; i < skillOptionSlots.Length; i++)
        {
            if (i < options.Count)
            {
                Debug.Log($"Presenting option {i + 1}: {options[i].skillName}");
                // This is where you would hook up the button's OnClick event.
                // The logic will need to be more complex now.
                // If it's a new skill, it's added to inventory.
                // If it's an upgrade, it should trigger the SkillUpgradeUI.
                var skillButton = skillOptionSlots[i].AddComponent<SkillButtonSimulator>();
                skillButton.Initialize(options[i], this);

                skillOptionSlots[i].SetActive(true);
            }
            else
            {
                skillOptionSlots[i].SetActive(false);
            }
        }
    }

    // This method would be called by the UI button's OnClick event.
    public void OnSkillSelected(SkillData selectedSkill)
    {
        Debug.Log($"Skill '{selectedSkill.skillName}' was selected.");

        // TODO: Add logic to check if this is a new skill or an upgrade.
        // If it's an upgrade, activate the SkillUpgradeUI panel.
        // If it's a new skill, add it to the inventory and resume the game.

        SkillManager.Instance.ApplySkillChoice(selectedSkill);
        GameManager.Instance.ResumeGameAfterSelection();
    }
}

// This helper class simulates a UI button's behavior.
public class SkillButtonSimulator : MonoBehaviour
{
    private SkillData assignedSkill;
    private SkillSelectionUI uiController;

    public void Initialize(SkillData skill, SkillSelectionUI controller)
    {
        assignedSkill = skill;
        uiController = controller;
    }

    public void SimulateClick()
    {
        uiController.OnSkillSelected(assignedSkill);
    }
}
