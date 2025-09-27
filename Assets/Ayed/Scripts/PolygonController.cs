using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PolygonController : MonoBehaviour
{
    
    [System.Serializable]
    public class PlayerAnchor
    {
        public PreSpawnedPlayerController playerController;
        public Vector3 localPosition; // Position relative to center (now Vector3)
        public bool isActive = true;
    }
    
    [Header("Polygon Settings")]
    [SerializeField] private List<PlayerAnchor> playerAnchors = new List<PlayerAnchor>();
    [SerializeField] private float polygonStiffness = 50f;
    [SerializeField] private float polygonDamping = 5f;
    [SerializeField] private float floatHeight = 2f;
    [SerializeField] private float floatStrength = 10f;
    [SerializeField] private float floatDamping = 2f;
    
    [Header("Physics Settings")]
    [SerializeField] private float mass = 1f;
    [SerializeField] private bool maintainShape = true;
    
    private Rigidbody rb; // Changed from Rigidbody2D to Rigidbody
    private Vector3 centerOfMass; // Changed from Vector2 to Vector3
    private List<Vector3> targetPositions = new List<Vector3>(); // Changed to Vector3
    private List<Vector3> currentVelocities = new List<Vector3>(); // Changed to Vector3
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false; // Disable gravity since we're floating
            rb.linearDamping = 0.5f; // Linear damping
            rb.angularDamping = 0.5f; // Angular damping
        }
        
        InitializePolygon();
    }
    
    void InitializePolygon()
    {
        // Set up initial positions and velocities
        targetPositions.Clear();
        currentVelocities.Clear();
        
        for (int i = 0; i < playerAnchors.Count; i++)
        {
            if (playerAnchors[i].playerController != null)
            {
                // Position player at their anchor point
                Vector3 worldPos = transform.position + playerAnchors[i].localPosition;
                playerAnchors[i].playerController.transform.position = worldPos;
                
                // Disable individual player physics
                Rigidbody playerRb = playerAnchors[i].playerController.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    playerRb.isKinematic = true;
                }
            }
            
            targetPositions.Add(playerAnchors[i].localPosition);
            currentVelocities.Add(Vector3.zero);
        }
    }
    
    void FixedUpdate()
    {
        UpdateCenterOfMass();
        ApplyFloatingForce();
        UpdatePlayerPositions();
        MaintainPolygonShape();
    }
    
    void UpdateCenterOfMass()
    {
        centerOfMass = Vector3.zero;
        int activePlayers = 0;
        
        for (int i = 0; i < playerAnchors.Count; i++)
        {
            if (playerAnchors[i].isActive && playerAnchors[i].playerController != null)
            {
                centerOfMass += playerAnchors[i].playerController.transform.position;
                activePlayers++;
            }
        }
        
        if (activePlayers > 0)
        {
            centerOfMass /= activePlayers;
        }
        else
        {
            centerOfMass = transform.position;
        }
    }
    
    void ApplyFloatingForce()
    {
        // Simple floating effect - adjust height based on distance from ground
        RaycastHit hit;
        if (Physics.Raycast(centerOfMass, Vector3.down, out hit, floatHeight * 2f))
        {
            float distanceToGround = hit.distance;
            float forceFactor = 1f - (distanceToGround / floatHeight);
            
            // Apply upward force based on distance to ground
            Vector3 floatForce = Vector3.up * forceFactor * floatStrength;
            rb.AddForce(floatForce);
            
            // Apply damping to prevent bouncing
            rb.AddForce(-rb.linearVelocity * floatDamping);
        }
        else
        {
            // If no ground detected, apply gentle downward force
            rb.AddForce(Vector3.down * floatStrength * 0.1f);
        }
    }
    
    void UpdatePlayerPositions()
    {
        for (int i = 0; i < playerAnchors.Count; i++)
        {
            if (!playerAnchors[i].isActive || playerAnchors[i].playerController == null) continue;
            
            // Get player's movement input (now Vector3)
            Vector3 playerMovement = GetPlayerMovementInput(playerAnchors[i].playerController);
            
            // Convert player input to force on the polygon
            Vector3 worldForce = playerMovement * polygonStiffness;
            rb.AddForce(worldForce);
            
            // Update target position based on player movement
            targetPositions[i] = playerAnchors[i].localPosition + playerMovement * 0.1f;
        }
    }
    
    void MaintainPolygonShape()
    {
        if (!maintainShape) return;
        
        for (int i = 0; i < playerAnchors.Count; i++)
        {
            if (!playerAnchors[i].isActive || playerAnchors[i].playerController == null) continue;
            
            Vector3 desiredWorldPos = transform.position + targetPositions[i];
            Vector3 currentWorldPos = playerAnchors[i].playerController.transform.position;
            
            // Use a temporary variable (now Vector3)
            Vector3 currentVelocity = currentVelocities[i];
            
            Vector3 newPosition = Vector3.SmoothDamp(
                currentWorldPos, 
                desiredWorldPos, 
                ref currentVelocity,  // Use the temporary variable
                0.1f, 
                Mathf.Infinity, 
                Time.fixedDeltaTime
            );
            
            // Update the array with the modified velocity
            currentVelocities[i] = currentVelocity;
            
            playerAnchors[i].playerController.transform.position = newPosition;
            
            // Apply spring force to maintain shape
            Vector3 displacement = desiredWorldPos - newPosition;
            Vector3 springForce = displacement * polygonStiffness;
            
            // Apply force at the player's position for better physics
            rb.AddForceAtPosition(springForce, newPosition);
        }
    }
    
    Vector3 GetPlayerMovementInput(PreSpawnedPlayerController player)
    {
        // This would connect to your existing player input system
        if (player.GetComponent<PlayerInput>() != null)
        {
            var moveAction = player.GetComponent<PlayerInput>().actions["Move"];
            if (moveAction != null)
            {
                Vector2 input = moveAction.ReadValue<Vector2>();
                // Convert 2D input to 3D movement
                return new Vector3(input.x, 0, input.y);
            }
        }
        
        return Vector3.zero;
    }
    
    Vector3 GetPlayerVelocity(int playerIndex)
    {
        if (playerIndex < currentVelocities.Count)
        {
            return currentVelocities[playerIndex];
        }
        return Vector3.zero;
    }
    
    void ApplyForceToPlayer(PreSpawnedPlayerController player, Vector3 force)
    {
        // This force primarily affects the main polygon rigidbody
        rb.AddForce(force);
    }
    
    // Public methods to manage the polygon
    public void AddPlayer(PreSpawnedPlayerController player, Vector3 localPosition)
    {
        PlayerAnchor newAnchor = new PlayerAnchor
        {
            playerController = player,
            localPosition = localPosition,
            isActive = true
        };
        
        playerAnchors.Add(newAnchor);
        targetPositions.Add(localPosition);
        currentVelocities.Add(Vector3.zero);
        
        InitializePolygon();
    }
    
    public void RemovePlayer(PreSpawnedPlayerController player)
    {
        int index = playerAnchors.FindIndex(anchor => anchor.playerController == player);
        if (index >= 0)
        {
            playerAnchors[index].isActive = false;
            // Optionally re-enable individual player physics
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.isKinematic = false;
            }
        }
    }
    
    // Visualization
    void OnDrawGizmos()
    {
        if (playerAnchors == null) return;
        
        Gizmos.color = Color.cyan;
        
        // Draw polygon shape
        for (int i = 0; i < playerAnchors.Count; i++)
        {
            if (playerAnchors[i].playerController == null || !playerAnchors[i].isActive) continue;
            
            Vector3 worldPos = transform.TransformPoint(playerAnchors[i].localPosition);
            Gizmos.DrawWireSphere(worldPos, 0.3f);
            
            // Draw lines between connected players
            int nextIndex = (i + 1) % playerAnchors.Count;
            if (playerAnchors[nextIndex].playerController != null && playerAnchors[nextIndex].isActive)
            {
                Vector3 nextWorldPos = transform.TransformPoint(playerAnchors[nextIndex].localPosition);
                Gizmos.DrawLine(worldPos, nextWorldPos);
            }
        }
        
        // Draw center of mass
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerOfMass, 0.2f);
        
        // Draw float height
        Gizmos.color = Color.green;
        Gizmos.DrawRay(centerOfMass, Vector3.down * floatHeight);
        
        // Draw forward direction
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(centerOfMass, transform.forward * 1f);
    }
}


