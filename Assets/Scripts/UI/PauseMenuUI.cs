using UnityEngine;
using System.Text;

/// <summary>
/// Manages the pause menu UI. Handles pausing/resuming the game
/// and displaying run statistics like damage dealt per skill.
/// </summary>
public class PauseMenuUI : MonoBehaviour
{
    [Tooltip("The parent GameObject for the pause menu UI panel.")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;

    void Update()
    {
        // A simple way to toggle the pause menu with the Escape key.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Pause the game and show the menu
            Time.timeScale = 0f;
            pauseMenuPanel.SetActive(true);
            DisplayDamageStats();
        }
        else
        {
            // Resume the game and hide the menu
            Time.timeScale = 1f;
            pauseMenuPanel.SetActive(false);
        }
    }

    private void DisplayDamageStats()
    {
        // In a real UI, you would populate a text field.
        // For now, we will build a string and log it to the console.
        var stats = DamageTracker.GetDamageStats();

        StringBuilder statsText = new StringBuilder();
        statsText.AppendLine("--- Damage Stats ---");

        if (stats.Count == 0)
        {
            statsText.AppendLine("No damage recorded yet.");
        }
        else
        {
            foreach (var entry in stats)
            {
                statsText.AppendLine($"{entry.Key}: {entry.Value:F0} damage");
            }
        }

        Debug.Log(statsText.ToString());
    }
}
