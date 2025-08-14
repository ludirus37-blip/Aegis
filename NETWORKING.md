# Aegis Survivors - Networking Guide

This document provides an overview of the networking architecture for Aegis Survivors, which is built using the `Unity.Netcode.for.Gameobjects` package.

## Architecture: Authoritative Server

The networking model is **server-authoritative**. This means:
*   The **server** (or the player acting as the **host**, who is both a server and a client) is the source of truth for the game state.
*   Clients send their inputs to the server.
*   The server processes these inputs, updates the game state, and then synchronizes the result back to all clients.
*   This model prevents cheating, as clients cannot modify their own state directly (e.g., give themselves infinite health).

## Core Networking Scripts

*   **`AegisNetworkManager.cs` (`Assets/Scripts/Networking/`):**
    *   This is a helper script that provides a simple interface to the underlying `Unity.Netcode.NetworkManager`.
    *   It has public methods (`StartHost()`, `StartClient()`) that can be called from UI buttons to initiate a network session.
    *   It also contains the foundational callbacks (`OnClientDisconnectCallback`, etc.) needed to handle more advanced features like **Host Migration**.

*   **`LanDiscovery.cs` (`Assets/Scripts/Networking/`):**
    *   This script contains the architectural placeholder for discovering games on a Local Area Network (LAN).
    *   The GDD requires discovery via UDP broadcast. This script outlines how that would be achieved by broadcasting a message from the host and listening for it on clients.
    *   **Note:** The low-level UDP socket code is commented out and must be fully implemented and tested for this feature to be functional.

*   **Networked Behaviours:**
    *   **`PlayerController.cs`:** Modified to be a `NetworkBehaviour`. It includes an `if (!IsOwner)` check to ensure only the local player can control their character.
    *   **`PlayerStats.cs`:** Uses `NetworkVariable<T>` for critical stats like health. This means the server can change the value, and the change will automatically be replicated to all clients.
    *   **`EnemySpawner.cs` & `EnemyController.cs`:** These are now server-authoritative. The spawner will only run on the server, and it spawns networked enemy objects. The enemy AI also only runs on the server.
    *   Client representations of players and enemies are moved via a `NetworkTransform` component (which must be added to their prefabs in the editor), which synchronizes position and rotation from the server to clients.

## How to Test Networking

1.  **Create Prefabs:** In the Unity Editor, create prefabs for the Player and Enemies. Ensure each prefab has a `NetworkObject` component at its root. Add `NetworkTransform` to synchronize movement.
2.  **Configure Network Manager:** Create a `NetworkManager` GameObject and add Unity's `NetworkManager` component and our `AegisNetworkManager` script to it. Set the `UnityTransport` as the transport layer.
3.  **Build a Standalone Player:** Go to `File > Build Settings` and create a standalone build (e.g., for Windows/macOS).
4.  **Run Two Instances:**
    *   Run one instance of the game in the Unity Editor.
    *   Run the standalone build you just created.
5.  **Connect:**
    *   In one instance (e.g., the editor), click the "Host" button. This will start a game as both a server and a client.
    *   In the other instance, click the "Client" button. It should connect to the host on the local machine.

You should now see two players in the host's window, and you should only be able to control one of them in each window.
