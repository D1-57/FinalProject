// using UnityEngine;
// using System.Collections.Generic;

// public class CarryObject : MonoBehaviour
// {
//     [Header("Carry Settings")]
//     public float connectionDistance = 2f;
//     public float jointStiffness = 100f;
//     public float jointDamping = 10f;

//     [Header("Player References")]
//     public List<PlayerMovement> connectedPlayers = new List<PlayerMovement>();

//     private List<ConfigurableJoint> playerJoints = new List<ConfigurableJoint>();
//     private Rigidbody rb;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

//         // Set up central object physics
//         rb.mass = 20f;
//         rb.linearDamping = 2f;
//         rb.angularDamping = 3f;

//         // Auto-connect players that are assigned in inspector
//         AutoConnectPlayers();
//     }

//     private void AutoConnectPlayers()
//     {
//         // If players are assigned in inspector, connect them
//         foreach (PlayerMovement player in connectedPlayers)
//         {
//             if (player != null)
//             {
//                 // Get the player's rigidbody and connect
//                 Rigidbody playerRb = player.GetComponent<Rigidbody>();
//                 if (playerRb != null)
//                 {
//                     ConnectPlayer(playerRb, player.GetPlayerIndex());
//                 }
//             }
//         }

//         // Alternatively, find all players in scene and connect them
//         if (connectedPlayers.Count == 0)
//         {
//             PlayerMovement[] allPlayers = FindObjectsOfType<PlayerMovement>();
//             foreach (PlayerMovement player in allPlayers)
//             {
//                 Rigidbody playerRb = player.GetComponent<Rigidbody>();
//                 if (playerRb != null)
//                 {
//                     ConnectPlayer(playerRb, player.GetPlayerIndex());
//                     connectedPlayers.Add(player);
//                 }
//             }
//         }
//     }

//     public void ConnectPlayer(Rigidbody playerRb, int playerIndex)
//     {
//         // Check if player is already connected
//         if (playerJoints.Exists(joint => joint != null && joint.connectedBody == playerRb))
//             return;

//         ConfigurableJoint joint = playerRb.gameObject.AddComponent<ConfigurableJoint>();
//         joint.connectedBody = rb;

//         // Position player around the object based on player index
//         Vector3 connectionPoint = GetConnectionPointForPlayer(playerIndex);
//         joint.connectedAnchor = connectionPoint;
//         joint.anchor = Vector3.zero;

//         ConfigureJointForCarrying(joint);

//         playerJoints.Add(joint);

//         Debug.Log($"Player {playerIndex} connected to object! Total: {playerJoints.Count}");
//     }

//     private Vector3 GetConnectionPointForPlayer(int playerIndex)
//     {
//         // Position players at different sides based on index
//         switch (playerIndex)
//         {
//             case 0: return new Vector3(-connectionDistance, 0, 0); // Left player
//             case 1: return new Vector3(connectionDistance, 0, 0);  // Right player  
//             case 2: return new Vector3(0, 0, connectionDistance);  // Front player
//             default: return Vector3.forward * connectionDistance;
//         }
//     }

//     private void ConfigureJointForCarrying(ConfigurableJoint joint)
//     {
//         // Allow some movement for pushing effect
//         joint.xMotion = ConfigurableJointMotion.Limited;
//         joint.yMotion = ConfigurableJointMotion.Limited;
//         joint.zMotion = ConfigurableJointMotion.Limited;

//         // Limited rotation
//         joint.angularXMotion = ConfigurableJointMotion.Limited;
//         joint.angularYMotion = ConfigurableJointMotion.Limited;
//         joint.angularZMotion = ConfigurableJointMotion.Limited;

//         // Soft limits for pushing
//         SoftJointLimit limit = new SoftJointLimit();
//         limit.limit = 0.3f;
//         joint.linearLimit = limit;

//         // Spring configuration
//         JointDrive drive = new JointDrive();
//         drive.positionSpring = jointStiffness;
//         drive.positionDamper = jointDamping;
//         drive.maximumForce = 1000f;

//         joint.xDrive = drive;
//         joint.yDrive = drive;
//         joint.zDrive = drive;

//         // Prevent breaking
//         joint.breakForce = Mathf.Infinity;
//         joint.breakTorque = Mathf.Infinity;

//         // Enable collision
//         joint.enableCollision = true;
//     }

//     public void DisconnectPlayer(Rigidbody playerRb)
//     {
//         for (int i = playerJoints.Count - 1; i >= 0; i--)
//         {
//             if (playerJoints[i] != null && playerJoints[i].connectedBody == playerRb)
//             {
//                 Destroy(playerJoints[i]);
//                 playerJoints.RemoveAt(i);

//                 // Remove from connected players list
//                 PlayerMovement player = playerRb.GetComponent<PlayerMovement>();
//                 if (player != null)
//                 {
//                     connectedPlayers.Remove(player);
//                 }
//             }
//         }
//     }

//     // Optional: Visualize connection points in scene view
//     void OnDrawGizmos()
//     {
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(transform.position, 0.5f);

//         // Draw connection points
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position + new Vector3(-connectionDistance, 0, 0), 0.3f);
//         Gizmos.DrawWireSphere(transform.position + new Vector3(connectionDistance, 0, 0), 0.3f);
//         Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, connectionDistance), 0.3f);
//     }
// }