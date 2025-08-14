using UnityEngine;

/// <summary>
/// Manages the player's persistent currency.
/// Handles loading, saving, and modifying the currency balance.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int CurrentCurrency { get; private set; }

    private const string CurrencyPlayerPrefsKey = "PlayerCurrency";

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

        LoadCurrency();
    }

    private void LoadCurrency()
    {
        CurrentCurrency = PlayerPrefs.GetInt(CurrencyPlayerPrefsKey, 0);
        Debug.Log($"Loaded currency: {CurrentCurrency}");
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(CurrencyPlayerPrefsKey, CurrentCurrency);
        PlayerPrefs.Save();
        Debug.Log($"Saved currency: {CurrentCurrency}");
    }

    public void AddCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add a negative amount of currency.");
            return;
        }
        CurrentCurrency += amount;
        SaveCurrency();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot spend a negative amount of currency.");
            return false;
        }

        if (CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
            SaveCurrency();
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough currency to spend.");
            return false;
        }
    }
}
