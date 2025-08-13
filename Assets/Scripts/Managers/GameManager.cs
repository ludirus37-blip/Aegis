using UnityEngine;

public class GameManager : MonoBehaviour
{
    // A simple singleton pattern to allow easy access from other scripts.
    public static GameManager Instance { get; private set; }

    [Tooltip("Assign the level-up UI panel here. It will be shown on level up.")]
    public GameObject cardSelectionUI;

    private LevelingSystem playerLevelingSystem;

    void Awake()
    {
        // Enforce the singleton pattern.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Find the player's LevelingSystem and subscribe to its OnLevelUp event.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerLevelingSystem = player.GetComponent<LevelingSystem>();
            if (playerLevelingSystem != null)
            {
                playerLevelingSystem.OnLevelUp += HandleLevelUp;
            }
        }
        else
        {
            Debug.LogError("GameManager could not find Player. Make sure the player GameObject has the 'Player' tag.");
        }

        // Ensure the card selection UI is hidden at the start of the game.
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(false);
        }
    }

    private void HandleLevelUp(int newLevel)
    {
        Debug.Log($"Player has reached Level {newLevel}! Pausing game for upgrade selection.");
        PauseGameAndShowCardSelection();
    }

    public void PauseGameAndShowCardSelection()
    {
        Time.timeScale = 0f; // This pauses all physics and frame-based updates.
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Card Selection UI is not assigned in the GameManager inspector.");
        }
    }

    // This method would be called by a button on the card selection UI.
    public void ResumeGameAfterSelection()
    {
        if (cardSelectionUI != null)
        {
            cardSelectionUI.SetActive(false);
        }
        Time.timeScale = 1f; // Resumes the game.
    }

    void OnDestroy()
    {
        // Always unsubscribe from events when the object is destroyed to prevent memory leaks.
        if (playerLevelingSystem != null)
        {
            playerLevelingSystem.OnLevelUp -= HandleLevelUp;
        }
    }
}
