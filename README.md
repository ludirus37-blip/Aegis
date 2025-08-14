# Aegis Survivors

Aegis Survivors is a fast-paced, survivors-io style mobile game built in Unity. It features short, addictive runs, large enemy hordes, and a deep progression system. This project is designed to be data-driven and easily extensible.

## Project Status

The core single-player gameplay loop is implemented. This includes player movement, combat, an XP/leveling system, and a functional skill card upgrade mechanic. The foundation for LAN multiplayer, meta-progression, and multiple content types (heroes, enemies) is in place.

This project is currently configured for **Unity 6000.1.14f1**.

## Core Systems Overview

*   **Data-Driven Design:** Most game data (Heroes, Enemies, Cards) is managed through `ScriptableObjects`. You can create new ones in the Unity Editor to easily add new content. The master list of skill cards is also available in `skill_cards.json`.
*   **Gameplay Loop:** The game is managed by a series of singleton managers (`GameManager`, `SceneLoader`, `CardManager`, etc.). The `GameSceneInitializer` script is responsible for setting up the main game scene at runtime.
*   **Networking:** The project uses `Unity.Netcode.for.GameObjects`. The architecture is server-authoritative. Scripts have been written to support this model, but gameplay logic (like syncing abilities) still needs to be fully implemented.
*   **Testing:** The project is set up with the Unity Test Framework. Unit tests for core systems are located in the `Assets/Tests` folder.

## Getting Started

1.  **Clone the repository:**
    ```bash
    git clone <repository_url>
    ```
2.  **Open in Unity:** Open the project using **Unity Hub**. Ensure you have **Unity version 6000.1.14f1** installed.
3.  **Open the Main Menu Scene:** In the Project window, navigate to `Assets/Scenes/` and open `MainMenuScene.unity`.
4.  **Set up Build Settings:** Ensure the scenes `MainMenuScene` (index 0) and `GameScene` (index 1) are added to the `File > Build Settings` window.
5.  **Press Play:** You can press the Play button in the editor to run the main menu. The core gameplay loop will load when you start a game, but it requires further setup of prefabs in the editor.

For detailed instructions on building the project for Android and iOS, please see `BUILDING.md`. For details on the networking implementation, see `NETWORKING.md`.
