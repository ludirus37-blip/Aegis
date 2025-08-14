using UnityEngine;

/// <summary>
/// This script is responsible for initializing the main gameplay scene.
/// It programmatically loads and instantiates all necessary prefabs from the Resources folder.
/// This ensures the scene works without manual setup in the editor.
/// In a real project, this would be placed on a single GameObject in the GameScene.
/// </summary>
public class GameSceneInitializer : MonoBehaviour
{
    // Prefabs will be loaded from "Assets/Resources/Prefabs/"
    private const string PLAYER_PREFAB_PATH = "Prefabs/Player";
    private const string GAME_MANAGER_PREFAB_PATH = "Prefabs/GameManager";
    private const string SCENE_LOADER_PREFAB_PATH = "Prefabs/SceneLoader";
    private const string SKILL_MANAGER_PREFAB_PATH = "Prefabs/SkillManager";
    private const string ENEMY_SPAWNER_PREFAB_PATH = "Prefabs/EnemySpawner";

    void Awake()
    {
        // --- Instantiate Managers ---
        InstantiateManager(SCENE_LOADER_PREFAB_PATH, () => SceneLoader.Instance == null);
        InstantiateManager(GAME_MANAGER_PREFAB_PATH, () => GameManager.Instance == null);
        InstantiateManager(SKILL_MANAGER_PREFAB_PATH, () => SkillManager.Instance == null);

        // --- Instantiate Gameplay Objects ---
        InstantiateFromResources(PLAYER_PREFAB_PATH, Vector3.zero, Quaternion.identity);
        InstantiateFromResources(ENEMY_SPAWNER_PREFAB_PATH);
    }

    /// <summary>
    /// Helper method to instantiate a manager only if its singleton instance doesn't already exist.
    /// </summary>
    private void InstantiateManager(string path, System.Func<bool> creationCondition)
    {
        if (creationCondition())
        {
            InstantiateFromResources(path);
        }
    }

    /// <summary>
    /// Loads a prefab from the Resources folder and instantiates it.
    /// </summary>
    private void InstantiateFromResources(string path, Vector3 position = default, Quaternion rotation = default)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null)
        {
            Instantiate(prefab, position, rotation);
        }
        else
        {
            Debug.LogError($"Failed to load prefab from Resources at path: '{path}'. Make sure the prefab exists in the 'Assets/Resources/{path}.prefab' file.");
        }
    }
}
