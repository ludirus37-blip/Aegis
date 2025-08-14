using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("Assign the level-up UI panel here. It will be shown on level up.")]
    public GameObject cardSelectionUI;

    private LevelingSystem playerLevelingSystem;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Make the GameManager persistent across scenes if it's not already handled by a prefab loader.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Subscribe to events when this manager is first created.
        SubscribeToPlayerEvents();
    }

    private void OnEnable()
    {
        // Also subscribe when the object is re-enabled (e.g., after a scene load).
        SubscribeToPlayerEvents();
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks when the object is disabled or destroyed.
        if (playerLevelingSystem != null)
        {
            playerLevelingSystem.OnLevelUp -= HandleLevelUp;
        }
    }

    private void SubscribeToPlayerEvents()
    {
        // Find the player's LevelingSystem and subscribe to its OnLevelUp event.
        // This needs to be robust enough to handle scene loading.
        if (playerLevelingSystem == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerLevelingSystem = player.GetComponent<LevelingSystem>();
                if (playerLevelingSystem != null)
                {
                    playerLevelingSystem.OnLevelUp += HandleLevelUp;
                }
            }
        }
    }

    private void HandleLevelUp(int newLevel)
    {
        Debug.Log($"Player has reached Level {newLevel}! Pausing game for upgrade selection.");
        PauseGameAndShowCardSelection();
    }

    public void PauseGameAndShowCardSelection()
    {
        Time.timeScale = 0f;
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(true);
        }
    }

    public void ResumeGameAfterSelection()
    {
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Placeholder for the end-of-run logic.
    /// This would be called when the player dies or completes the level.
    /// </summary>
    /// <param name="goldCollectedThisRun">The amount of gold/currency gathered during the run.</param>
    public void EndOfRun(int goldCollectedThisRun)
    {
        Debug.Log($"Run has ended. Awarding {goldCollectedThisRun} currency.");

        // Use the CurrencyManager to add the collected gold to the player's persistent total.
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCurrency(goldCollectedThisRun);
        }

        // Load the main menu or a results screen.
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenuScene");
        }
    }
}
