using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))] // Ensure the PlayerStats component is present.
public class PlayerController : MonoBehaviour
{
    // The PlayerStats component will now be the source for all runtime stats.
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
        // Input handling remains the same. This would be linked to a virtual joystick on mobile.
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Use the moveSpeed from PlayerStats, which can be modified by cards during the run.
        // This makes the stat system dynamic.
        if (playerStats != null)
        {
            rb.MovePosition(rb.position + moveInput.normalized * playerStats.moveSpeed * Time.fixedDeltaTime);
        }
    }
}
