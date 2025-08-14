using UnityEngine;

/// <summary>
/// Manages the UI logic for the main menu screen.
/// This script would be attached to a GameObject in the MainMenu scene.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    // These methods are intended to be called by UI Button OnClick events.

    /// <summary>
    /// Loads the main gameplay scene.
    /// This would be linked to a "Start Game" or "Play" button.
    /// </summary>
    public void StartGame()
    {
        // We assume the main game scene is named "GameScene".
        // We use the SceneLoader singleton to handle the actual loading.
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("GameScene");
        }
        else
        {
            Debug.LogError("SceneLoader instance not found! Cannot start game.");
        }
    }

    /// <summary>
    /// Opens the options menu.
    /// This is a placeholder for now.
    /// </summary>
    public void OpenOptions()
    {
        // In a real game, this would activate an options panel UI.
        Debug.Log("Options button clicked. (UI Panel would open here)");
    }

    /// <summary>
    /// Quits the application.
    /// This would be linked to a "Quit" button.
    /// </summary>
    public void QuitGame()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.QuitGame();
        }
        else
        {
            Debug.LogError("SceneLoader instance not found! Cannot quit game.");
        }
    }
}
