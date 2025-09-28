using UnityEngine;
using UnityEngine.InputSystem;

public class PreSpawnedPlayerController : MonoBehaviour
{

    public enum MovementConstraint
    {
        RightOnly,     // Can only move right (positive X)
        LeftOnly,      // Can only move left (negative X)  
        ForwardOnly,   // Can only move forward (positive Z)
        BackwardOnly   // Can only move backward (negative Z)
    }

    [Header("Player Settings")]
    [SerializeField] private int playerId = 0;
    [SerializeField] private MovementConstraint movementConstraint;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Color playerColor = Color.white;

    [Header("Input Settings")]
    [SerializeField] private string moveActionName = "Move";

    private Vector2 currentInput;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetupPlayerVisuals();
    }

    void Update()
    {
        // Get input from the assigned gamepad/keyboard
        if (playerInput != null)
        {
            currentInput = playerInput.actions[moveActionName].ReadValue<Vector2>();
            currentInput = ApplyMovementConstraints(currentInput);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            Vector2 movement = currentInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
        else
        {
            // Fallback for non-physics movement
            transform.Translate(currentInput * moveSpeed * Time.deltaTime);
        }
    }

    private Vector2 ApplyMovementConstraints(Vector3 input)
    {
        return movementConstraint switch
        {
            MovementConstraint.RightOnly => new Vector2(Mathf.Clamp01(input.x), 0f),
            MovementConstraint.LeftOnly => new Vector2(Mathf.Clamp(input.x, -1f, 0f), 0f),
            MovementConstraint.ForwardOnly => new Vector2(0f, input.y),
            _ => input
        };
    }

    private void SetupPlayerVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = playerColor;
        }

        // Name the GameObject appropriately
        gameObject.name = $"Player {playerId + 1} ({movementConstraint})";
    }

    // Public methods to modify constraints at runtime
    public void SetMovementConstraint(MovementConstraint newConstraint)
    {
        movementConstraint = newConstraint;
        gameObject.name = $"Player {playerId + 1} ({newConstraint})";
    }

    public void SetPlayerId(int id)
    {
        playerId = id;
    }

    public void SetColor(Color newColor)
    {
        playerColor = newColor;
        if (spriteRenderer != null)
            spriteRenderer.color = newColor;
    }

    // Gizmos for visual debugging
    void OnDrawGizmos()
    {
        Gizmos.color = playerColor;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 1.2f);

        // Draw movement direction arrow
        Vector3 direction = movementConstraint switch
        {
            MovementConstraint.RightOnly => Vector3.right,
            MovementConstraint.LeftOnly => Vector3.left,
            MovementConstraint.ForwardOnly => Vector3.up,
            _ => Vector3.zero
        };

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, direction * 1.5f);
    }
    
}