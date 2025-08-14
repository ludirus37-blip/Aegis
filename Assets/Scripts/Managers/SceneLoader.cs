using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A centralized manager for loading scenes throughout the application.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // A static instance to allow easy access from other scripts (e.g., UI buttons).
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists.
        // This object will persist across scene loads.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads a scene by its build name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load (must be in Build Settings).</param>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be null or empty.");
            return;
        }

        Debug.Log($"Loading scene: {sceneName}...");
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }
}
