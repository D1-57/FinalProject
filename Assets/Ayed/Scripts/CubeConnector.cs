// using UnityEngine;

// public class CubeConnector : MonoBehaviour
// {
//     [Header("Player References")]
//     public Transform player1;
//     public Transform player2;
//     public Transform player3;
    
//     [Header("Cube Settings")]
//     public float cubeSize = 2f;
//     public float floatHeight = 3f;
//     public float connectionStiffness = 10f;
//     public float floatForce = 12f;
//     public float damping = 0.5f;
    
//     private Rigidbody cubeRb;
//     private SpringJoint[] playerJoints;
//     private bool isFloating = true;
    
//     void Start()
//     {
//         // Ensure the cube has a Rigidbody
//         cubeRb = GetComponent<Rigidbody>();
//         if (cubeRb == null)
//         {
//             cubeRb = gameObject.AddComponent<Rigidbody>();
//         }
        
//         // Configure cube Rigidbody
//         cubeRb.mass = 2f;
//         cubeRb.linearDamping = 1f;
//         cubeRb.angularDamping = 1f;
        
//         // Create spring joints for each player
//         CreatePlayerConnections();
        
//         // Set up floating behavior
//         SetupFloatingCube();
//     }
    
//     void FixedUpdate()
//     {
//         // Maintain floating height
//         if (isFloating)
//         {
//             MaintainFloatHeight();
//         }
        
//         // Keep cube centered between players (optional - makes it more stable)
//         MaintainCenterPosition();
//     }
    
//     void CreatePlayerConnections()
//     {
//         playerJoints = new SpringJoint[3];
//         Transform[] players = { player1, player2, player3 };
        
//         for (int i = 0; i < players.Length; i++)
//         {
//             if (players[i] != null)
//             {
//                 // Add SpringJoint to player
//                 SpringJoint joint = players[i].gameObject.AddComponent<SpringJoint>();
//                 joint.connectedBody = cubeRb;
//                 joint.spring = connectionStiffness;
//                 joint.damper = damping;
//                 joint.autoConfigureConnectedAnchor = false;
//                 joint.anchor = Vector3.zero;
                
//                 // Position the connection point around the cube
//                 Vector3 connectionPoint = Vector3.zero;
//                 if (i == 0) connectionPoint = Vector3.left * cubeSize;
//                 else if (i == 1) connectionPoint = Vector3.right * cubeSize;
//                 else connectionPoint = Vector3.forward * cubeSize;
                
//                 joint.connectedAnchor = connectionPoint;
                
//                 playerJoints[i] = joint;
                
//                 // Configure player Rigidbody
//                 Rigidbody playerRb = players[i].GetComponent<Rigidbody>();
//                 if (playerRb != null)
//                 {
//                     playerRb.mass = 1f;
//                     playerRb.linearDamping = 2f;
//                     playerRb.angularDamping = 2f;
//                     playerRb.useGravity = false; // Players float with the cube
//                 }
//             }
//         }
//     }
    
//     void SetupFloatingCube()
//     {
//         // Position cube above players
//         Vector3 centerPosition = (player1.position + player2.position + player3.position) / 3f;
//         transform.position = centerPosition + Vector3.up * floatHeight;
        
//         // Configure cube physics
//         cubeRb.useGravity = false; // We'll handle gravity manually for floating
//     }
    
//     void MaintainFloatHeight()
//     {
//         // Simple floating effect using upward force
//         RaycastHit hit;
//         float targetHeight = floatHeight;
        
//         // Check current height above the average player position
//         Vector3 playersCenter = (player1.position + player2.position + player3.position) / 3f;
//         float currentHeight = transform.position.y - playersCenter.y;
        
//         // Apply upward force to maintain float height
//         if (currentHeight < targetHeight)
//         {
//             float forceMultiplier = (targetHeight - currentHeight) * floatForce;
//             cubeRb.AddForce(Vector3.up * forceMultiplier, ForceMode.Acceleration);
//         }
//     }
    
//     void MaintainCenterPosition()
//     {
//         // Gently pull cube toward center of players
//         Vector3 playersCenter = (player1.position + player2.position + player3.position) / 3f;
//         Vector3 directionToCenter = playersCenter - transform.position;
//         directionToCenter.y = 0; // Only horizontal correction
        
//         cubeRb.AddForce(directionToCenter * 2f, ForceMode.Acceleration);
//     }
    
//     // Visualize connections in editor
//     void OnDrawGizmos()
//     {
//         if (player1 != null && player2 != null && player3 != null)
//         {
//             Gizmos.color = Color.red;
//             if (player1 != null) Gizmos.DrawLine(transform.position, player1.position);
//             if (player2 != null) Gizmos.DrawLine(transform.position, player2.position);
//             if (player3 != null) Gizmos.DrawLine(transform.position, player3.position);
            
//             // Draw float height reference
//             Vector3 center = (player1.position + player2.position + player3.position) / 3f;
//             Gizmos.color = Color.blue;
//             Gizmos.DrawWireCube(center + Vector3.up * floatHeight, Vector3.one * cubeSize);
//         }
//     }
// }