using UnityEngine;

/// <summary>
/// Handles player movement.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerController could not find the PlayerStats component!", this);
            enabled = false;
        }
    }

    void Update()
    {
        // Input handling for single-player.
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Movement logic for single-player.
        if (playerStats != null)
        {
            rb.MovePosition(rb.position + moveInput.normalized * playerStats.moveSpeed * Time.fixedDeltaTime);
        }
    }
}
