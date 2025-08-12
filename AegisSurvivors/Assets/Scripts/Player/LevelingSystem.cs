using UnityEngine;
using System;

public class LevelingSystem : MonoBehaviour
{
    [Header("Level Stats")]
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _currentExperience = 0;
    [SerializeField] private int _experienceToNextLevel = 100;

    // Public properties for other systems to read data without modifying it.
    public int CurrentLevel => _currentLevel;
    public int CurrentExperience => _currentExperience;
    public int ExperienceToNextLevel => _experienceToNextLevel;

    // Event to notify other systems (like the GameManager or UI) when a level-up occurs.
    public event Action<int> OnLevelUp;
    public event Action<int, int> OnExperienceChanged; // Sends currentXP, nextLevelXP

    /// <summary>
    /// Adds experience points and checks if a level-up should occur.
    /// </summary>
    /// <param name="amount">The amount of experience to add.</param>
    public void AddExperience(int amount)
    {
        _currentExperience += amount;
        OnExperienceChanged?.Invoke(_currentExperience, _experienceToNextLevel);
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        // Use a while loop in case the player gains enough XP for multiple levels at once.
        while (_currentExperience >= _experienceToNextLevel)
        {
            _currentExperience -= _experienceToNextLevel;
            _currentLevel++;

            _experienceToNextLevel = CalculateNextLevelXp();

            // Notify listeners that a level up has happened.
            OnLevelUp?.Invoke(_currentLevel);
            // Update XP bar UI after level up
            OnExperienceChanged?.Invoke(_currentExperience, _experienceToNextLevel);
        }
    }

    private int CalculateNextLevelXp()
    {
        // A simple formula to make leveling up require more XP over time.
        // This can be easily adjusted to change the pacing of the game.
        return Mathf.FloorToInt(100f * Mathf.Pow(_currentLevel, 1.5f));
    }
}
