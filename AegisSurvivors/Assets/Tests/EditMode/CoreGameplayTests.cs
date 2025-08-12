using NUnit.Framework;
using UnityEngine;

public class CoreGameplayTests
{
    // Test fixture for the Health component
    private Health healthComponent;
    private GameObject testGameObject;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and add the component to it before each test
        testGameObject = new GameObject();
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the GameObject after each test
        Object.DestroyImmediate(testGameObject);
    }

    [Test]
    public void Health_TakeDamage_ReducesCurrentHealth()
    {
        healthComponent = testGameObject.AddComponent<Health>();
        healthComponent.Initialize(100f);

        healthComponent.TakeDamage(30f);

        Assert.AreEqual(70f, healthComponent.CurrentHealth);
    }

    [Test]
    public void Health_TakeFatalDamage_TriggersOnDeathEvent()
    {
        healthComponent = testGameObject.AddComponent<Health>();
        healthComponent.Initialize(50f);
        bool wasDeathEventFired = false;
        healthComponent.OnDeath += () => { wasDeathEventFired = true; };

        healthComponent.TakeDamage(100f);

        Assert.IsTrue(wasDeathEventFired);
        Assert.AreEqual(0, healthComponent.CurrentHealth);
    }

    [Test]
    public void Health_CurrentHealth_DoesNotGoBelowZero()
    {
        healthComponent = testGameObject.AddComponent<Health>();
        healthComponent.Initialize(20f);

        healthComponent.TakeDamage(100f);

        Assert.AreEqual(0, healthComponent.CurrentHealth);
    }

    [Test]
    public void LevelingSystem_AddExperience_IncreasesCurrentExperience()
    {
        var levelingSystem = testGameObject.AddComponent<LevelingSystem>();

        levelingSystem.AddExperience(50);

        Assert.AreEqual(50, levelingSystem.CurrentExperience);
        Assert.AreEqual(1, levelingSystem.CurrentLevel);
    }

    [Test]
    public void LevelingSystem_AddEnoughExperience_LevelsUpOnce()
    {
        var levelingSystem = testGameObject.AddComponent<LevelingSystem>(); // Starts at level 1, 100 XP to next
        int levelUpCount = 0;
        levelingSystem.OnLevelUp += (level) => {
            levelUpCount++;
            Assert.AreEqual(2, level);
        };

        levelingSystem.AddExperience(100);

        Assert.AreEqual(1, levelUpCount);
        Assert.AreEqual(2, levelingSystem.CurrentLevel);
        Assert.AreEqual(0, levelingSystem.CurrentExperience);
        Assert.Greater(levelingSystem.ExperienceToNextLevel, 100);
    }

    [Test]
    public void LevelingSystem_AddExperience_LevelsUpMultipleTimes()
    {
        var levelingSystem = testGameObject.AddComponent<LevelingSystem>();
        int levelUpCount = 0;
        levelingSystem.OnLevelUp += (level) => levelUpCount++;

        // XP for Lvl 1 -> 2 is 100. XP for Lvl 2 -> 3 is ~282. Total ~382.
        // Let's add enough XP for two level ups.
        levelingSystem.AddExperience(400);

        Assert.AreEqual(2, levelUpCount);
        Assert.AreEqual(3, levelingSystem.CurrentLevel);
        Assert.AreEqual(400 - 100 - 282, levelingSystem.CurrentExperience); // 18
    }
}
