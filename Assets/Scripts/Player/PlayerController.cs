using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Handles player movement and ensures that only the owner of this object can control it.
/// This script requires a NetworkObject component on the same GameObject.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : NetworkBehaviour
{
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    /// <summary>
    /// We override OnNetworkSpawn to safely get components after the network object is ready.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerController could not find the PlayerStats component!", this);
            enabled = false;
        }
    }

    void Update()
    {
        // This is the most important check for a NetworkBehaviour.
        // We must ensure that only the client who owns this object can process input.
        if (!IsOwner) return;

        // Input handling remains the same. This would be linked to a virtual joystick on mobile.
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Movement logic should also only be executed by the owner.
        // The position will be synced to other clients via the NetworkTransform component.
        if (!IsOwner) return;

        if (playerStats != null)
        {
            rb.MovePosition(rb.position + moveInput.normalized * playerStats.moveSpeed * Time.fixedDeltaTime);
        }
    }
}
