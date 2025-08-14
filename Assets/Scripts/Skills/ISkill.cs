/// <summary>
/// A common interface for all activateable skills.
/// This allows the PlayerSkillCaster to activate any skill in a generic way.
/// </summary>
public interface ISkill
{
    /// <summary>
    /// A reference to the ScriptableObject that defines this skill's properties.
    /// </summary>
    SkillData SkillData { get; }

    /// <summary>
    /// Activates the skill's primary effect.
    /// </summary>
    /// <param name="inventory">A reference to the player's skill inventory to get upgrade data.</param>
    void Activate(PlayerSkillInventory inventory);
}
