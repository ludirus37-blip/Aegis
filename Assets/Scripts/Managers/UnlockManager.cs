using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the unlockable content in the game, such as heroes.
/// Handles saving and loading unlock states.
/// </summary>
public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance { get; private set; }

    private const string UnlockPlayerPrefsKeyPrefix = "Unlock_";

    // A simple cache to hold the unlocked status of items during the session.
    private HashSet<string> unlockedItemsCache;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        unlockedItemsCache = new HashSet<string>();
    }

    /// <summary>
    /// Checks if a specific item (e.g., a hero) is unlocked.
    /// </summary>
    /// <param name="itemId">The unique ID of the item (e.g., heroName).</param>
    /// <returns>True if the item is unlocked.</returns>
    public bool IsUnlocked(string itemId)
    {
        if (unlockedItemsCache.Contains(itemId))
        {
            return true;
        }

        // Check PlayerPrefs if not in cache. 1 for unlocked, 0 for locked.
        bool isUnlocked = PlayerPrefs.GetInt(UnlockPlayerPrefsKeyPrefix + itemId, 0) == 1;
        if (isUnlocked)
        {
            unlockedItemsCache.Add(itemId);
        }
        return isUnlocked;
    }

    /// <summary>
    /// Unlocks a specific item and saves its state.
    /// </summary>
    /// <param name="itemId">The unique ID of the item to unlock.</param>
    public void UnlockItem(string itemId)
    {
        if (IsUnlocked(itemId)) return;

        PlayerPrefs.SetInt(UnlockPlayerPrefsKeyPrefix + itemId, 1);
        PlayerPrefs.Save();
        unlockedItemsCache.Add(itemId);
        Debug.Log($"Item unlocked and saved: {itemId}");
    }

    /// <summary>
    /// Attempts to purchase and unlock a hero.
    /// </summary>
    /// <param name="heroData">The data of the hero to purchase.</param>
    /// <returns>True if the purchase and unlock were successful.</returns>
    public bool PurchaseHero(HeroData heroData)
    {
        if (heroData == null) return false;
        if (IsUnlocked(heroData.heroName))
        {
            Debug.LogWarning($"Attempted to purchase hero '{heroData.heroName}' who is already unlocked.");
            return false;
        }

        if (CurrencyManager.Instance.SpendCurrency(heroData.unlockCost))
        {
            UnlockItem(heroData.heroName);
            return true;
        }
        else
        {
            Debug.Log("Purchase failed: Not enough currency.");
            return false;
        }
    }
}
