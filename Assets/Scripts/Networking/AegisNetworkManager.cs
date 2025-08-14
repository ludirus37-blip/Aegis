using Unity.Netcode;
using UnityEngine;

/// <summary>
/// A manager to handle starting the game as a host, client, or server.
/// Also includes the architectural setup for Host Migration.
/// This script would be attached to a GameObject alongside Unity's NetworkManager component.
/// </summary>
public class AegisNetworkManager : MonoBehaviour
{
    void Start()
    {
        // --- HOST MIGRATION SETUP ---
        // Subscribe to connection events. These are critical for managing state during host migration.
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        NetworkManager.Singleton.OnServerStopped += OnServerStopped;
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            NetworkManager.Singleton.OnServerStopped -= OnServerStopped;
        }
    }

    public void StartHost()
    {
        Debug.Log("Starting Host...");
        // To enable host migration, you would typically prepare the NetworkManager here
        // before starting. For example, by setting a flag or using a specific lobby service.
        // The core logic is handled by the NetworkManager itself when a host disconnects.
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        Debug.Log("Starting Client...");
        NetworkManager.Singleton.StartClient();
    }

    private void OnClientDisconnect(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}");
        // If the disconnected client was the host, the host migration process will begin automatically
        // if it has been configured correctly in the transport layer.
        if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.LocalClientId == clientId)
        {
            Debug.Log("Host has disconnected. Host migration should be in progress if enabled.");
        }
    }

    private void OnServerStopped(bool wasHost)
    {
        if (wasHost)
        {
            Debug.Log("Host has stopped. A new host will be chosen if host migration is enabled.");
            // Here you might need to handle cleanup or UI changes to reflect the host change.
        }
    }


    // Example of how to get connection status, could be used for UI feedback.
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host")) StartHost();
            if (GUILayout.Button("Client")) StartClient();
        }
        else
        {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            GUILayout.Label($"Mode: {mode}");
            GUILayout.Label($"ClientID: {NetworkManager.Singleton.LocalClientId}");
        }
        GUILayout.EndArea();
    }
}
