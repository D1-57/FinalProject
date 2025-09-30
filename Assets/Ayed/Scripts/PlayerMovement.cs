//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public enum PlayerType { LeftOnly, RightOnly, ForwardBackward }
//    public PlayerType playerType;
//    public float moveSpeed = 5f;
//    public int gamepadNumber = 1;

//    private Rigidbody rb;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    void Update()
//    {
//        string horizontalAxis = "J" + gamepadNumber + "Horizontal";
//        string verticalAxis = "J" + gamepadNumber + "Vertical";

//        float horizontal = Input.GetAxis(horizontalAxis);
//        float vertical = Input.GetAxis(verticalAxis);

//        Vector3 movement = Vector3.zero;

//        switch (playerType)
//        {
//            case PlayerType.LeftOnly:
//                movement = HandleLeftOnlyMovement(horizontal);
//                break;
//            case PlayerType.RightOnly:
//                movement = HandleRightOnlyMovement(horizontal);
//                break;
//            case PlayerType.ForwardBackward:
//                movement = HandleForwardBackwardMovement(vertical);
//                break;
//        }

//        // Apply movement using Rigidbody for physics
//        if (movement != Vector3.zero)
//        {
//            rb.linearVelocity = movement * moveSpeed;
//        }
//        else
//        {
//            rb.linearVelocity = Vector3.zero; // Stop when not moving
//        }
//    }

//    Vector3 HandleLeftOnlyMovement(float horizontal)
//    {
//        if (horizontal < -0.1f)
//        {
//            return Vector3.left * Mathf.Abs(horizontal);
//        }
//        return Vector3.zero;
//    }

//    Vector3 HandleRightOnlyMovement(float horizontal)
//    {
//        if (horizontal > 0.1f)
//        {
//            return Vector3.right * horizontal;
//        }
//        return Vector3.zero;
//    }

//    Vector3 HandleForwardBackwardMovement(float vertical)
//    {
//        if (vertical > 0.1f)
//        {
//            return Vector3.forward * vertical;
//        }
//        else if (vertical < -0.1f)
//        {
//            return Vector3.back * Mathf.Abs(vertical);
//        }
//        return Vector3.zero;
//    }
////}
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public enum PlayerType { LeftOnly, RightOnly, ForwardBackward }
//     public PlayerType playerType;
//     public float moveSpeed = 5f;
//     public int gamepadNumber = 1;

//     void Update()
//     {
//         string horizontalAxis = "J" + gamepadNumber + "Horizontal";
//         string verticalAxis = "J" + gamepadNumber + "Vertical";

//         float horizontal = Input.GetAxis(horizontalAxis);
//         float vertical = Input.GetAxis(verticalAxis);

//         switch (playerType)
//         {
//             case PlayerType.LeftOnly:
//                 HandleLeftOnlyMovement(horizontal);
//                 break;
//             case PlayerType.RightOnly:
//                 HandleRightOnlyMovement(horizontal);
//                 break;
//             case PlayerType.ForwardBackward:
//                 HandleForwardBackwardMovement(vertical);
//                 break;
//         }
//     }

//     void HandleLeftOnlyMovement(float horizontal)
//     {
//         // Player 1 - moves only left relative to facing direction
//         if (horizontal < -0.1f)
//         {
//             Vector3 movement = -transform.right * moveSpeed * Mathf.Abs(horizontal) * Time.deltaTime;
//             transform.Translate(movement, Space.World);
//         }
//     }

//     void HandleRightOnlyMovement(float horizontal)
//     {
//         // Player 2 - moves only right relative to facing direction
//         if (horizontal > 0.1f)
//         {
//             Vector3 movement = transform.right * moveSpeed * horizontal * Time.deltaTime;
//             transform.Translate(movement, Space.World);
//         }
//     }

//     void HandleForwardBackwardMovement(float vertical)
//     {
//         // Player 3 - moves forward/backward relative to facing direction
//         if (vertical > 0.1f) // Forward
//         {
//             Vector3 movement = transform.forward * moveSpeed * vertical * Time.deltaTime;
//             transform.Translate(movement, Space.World);
//         }
//         else if (vertical < -0.1f) // Backward
//         {
//             Vector3 movement = -transform.forward * moveSpeed * Mathf.Abs(vertical) * Time.deltaTime;
//             transform.Translate(movement, Space.World);
//         }
//     }
// }
















using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerType { LeftOnly, RightOnly, ForwardBackward }
    public PlayerType playerType;
    public float moveSpeed = 5f;
    public int gamepadNumber = 1;
    
    private Animator animator;
    private bool isMoving = false;

    void Start()
    {
        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();
        
        // If there's no Animator on this object, try to find it in children
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        
        // Log warning if no animator found
        if (animator == null)
        {
            Debug.LogWarning("No Animator component found on " + gameObject.name);
        }
    }

    void Update()
    {
        string horizontalAxis = "J" + gamepadNumber + "Horizontal";
        string verticalAxis = "J" + gamepadNumber + "Vertical";

        float horizontal = Input.GetAxis(horizontalAxis);
        float vertical = Input.GetAxis(verticalAxis);

        bool wasMoving = isMoving;
        isMoving = false;

        switch (playerType)
        {
            case PlayerType.LeftOnly:
                HandleLeftOnlyMovement(horizontal);
                break;
            case PlayerType.RightOnly:
                HandleRightOnlyMovement(horizontal);
                break;
            case PlayerType.ForwardBackward:
                HandleForwardBackwardMovement(vertical);
                break;
        }

        // Update animator if movement state changed
        if (wasMoving != isMoving && animator != null)
        {
            animator.SetBool("IsWalking", isMoving);
        }
    }

    void HandleLeftOnlyMovement(float horizontal)
    {
        // Player 1 - moves only left relative to facing direction
        if (horizontal < -0.1f)
        {
            Vector3 movement = -transform.right * moveSpeed * Mathf.Abs(horizontal) * Time.deltaTime;
            transform.Translate(movement, Space.World);
            isMoving = true;
        }
    }

    void HandleRightOnlyMovement(float horizontal)
    {
        // Player 2 - moves only right relative to facing direction
        if (horizontal > 0.1f)
        {
            Vector3 movement = transform.right * moveSpeed * horizontal * Time.deltaTime;
            transform.Translate(movement, Space.World);
            isMoving = true;
        }
    }

    void HandleForwardBackwardMovement(float vertical)
    {
        // Player 3 - moves forward/backward relative to facing direction
        if (vertical > 0.1f) // Forward
        {
            Vector3 movement = transform.forward * moveSpeed * vertical * Time.deltaTime;
            transform.Translate(movement, Space.World);
            isMoving = true;
        }
        else if (vertical < -0.1f) // Backward
        {
            Vector3 movement = -transform.forward * moveSpeed * Mathf.Abs(vertical) * Time.deltaTime;
            transform.Translate(movement, Space.World);
            isMoving = true;
        }
    }
}





































//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public enum PlayerType { LeftOnly, RightOnly, ForwardBackward }
//    public PlayerType playerType;
//    public float moveForce = 10f; // Changed from moveSpeed to force
//    public float maxSpeed = 5f;
//    public int gamepadNumber = 1;

//    private Rigidbody rb;
//    private CarryObject connectedObject;
//    private bool isCarrying = false;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        // Ensure Rigidbody is set up properly
//        rb.linearDamping = 1f;
//        rb.angularDamping = 2f;
//        rb.mass = 1f;
//    }

//    void Update()
//    {
//        string horizontalAxis = "J" + gamepadNumber + "Horizontal";
//        string verticalAxis = "J" + gamepadNumber + "Vertical";

//        float horizontal = Input.GetAxis(horizontalAxis);
//        float vertical = Input.GetAxis(verticalAxis);

//        switch (playerType)
//        {
//            case PlayerType.LeftOnly:
//                HandleLeftOnlyMovement(horizontal);
//                break;
//            case PlayerType.RightOnly:
//                HandleRightOnlyMovement(horizontal);
//                break;
//            case PlayerType.ForwardBackward:
//                HandleForwardBackwardMovement(vertical);
//                break;
//        }
//    }

//    void HandleLeftOnlyMovement(float horizontal)
//    {
//        // Player 1 - moves only left relative to facing direction
//        if (horizontal < -0.1f && rb.linearVelocity.magnitude < maxSpeed)
//        {
//            Vector3 forceDirection = -transform.right * moveForce * Mathf.Abs(horizontal);
//            rb.AddForce(forceDirection);
//        }
//    }

//    void HandleRightOnlyMovement(float horizontal)
//    {
//        // Player 2 - moves only right relative to facing direction
//        if (horizontal > 0.1f && rb.linearVelocity.magnitude < maxSpeed)
//        {
//            Vector3 forceDirection = transform.right * moveForce * horizontal;
//            rb.AddForce(forceDirection);
//        }
//    }

//    void HandleForwardBackwardMovement(float vertical)
//    {
//        // Player 3 - moves forward/backward relative to facing direction
//        if (Mathf.Abs(vertical) > 0.1f && rb.linearVelocity.magnitude < maxSpeed)
//        {
//            Vector3 forceDirection = transform.forward * moveForce * vertical;
//            rb.AddForce(forceDirection);
//        }
//    }

//    // Auto-connect when touching the carry object
//    void OnCollisionEnter(Collision collision)
//    {
//        CarryObject carryObj = collision.gameObject.GetComponent<CarryObject>();
//        if (carryObj != null && !isCarrying)
//        {
//            ConnectToObject(carryObj);
//        }
//    }

//    public void ConnectToObject(CarryObject carryObj)
//    {
//        connectedObject = carryObj;
//        carryObj.ConnectPlayer(rb);
//        isCarrying = true;

//        // Adjust physics when carrying
//        rb.linearDamping = 2f;
//        rb.angularDamping = 3f;
//    }

//    public void DisconnectFromObject()
//    {
//        if (connectedObject != null)
//        {
//            connectedObject.DisconnectPlayer(rb);
//            connectedObject = null;
//            isCarrying = false;

//            // Reset physics
//            rb.linearDamping = 1f;
//            rb.angularDamping = 2f;
//        }
//    }

//    // Optional: Add a key to disconnect (for testing)
//    void LateUpdate()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && isCarrying)
//        {
//            DisconnectFromObject();
//        }
//    }
//}
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public enum PlayerType { LeftOnly, RightOnly, ForwardBackward }
//     public PlayerType playerType;
//     public float moveForce = 10f;
//     public float maxSpeed = 5f;
//     public int gamepadNumber = 1;
//     public CarryObject carryObject; // Assign in inspector

//     private Rigidbody rb;
//     private bool isCarrying = false;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         // Ensure Rigidbody is set up properly
//         rb.linearDamping = 1f;
//         rb.angularDamping = 2f;
//         rb.mass = 1f;

//         // Auto-connect to the carry object on start
//         if (carryObject != null)
//         {
//             ConnectToObject(carryObject);
//         }
//         else
//         {
//             // Try to find the carry object automatically
//             carryObject = FindObjectOfType<CarryObject>();
//             if (carryObject != null)
//             {
//                 ConnectToObject(carryObject);
//             }
//         }
//     }

//     void Update()
//     {
//         string horizontalAxis = "J" + gamepadNumber + "Horizontal";
//         string verticalAxis = "J" + gamepadNumber + "Vertical";

//         float horizontal = Input.GetAxis(horizontalAxis);
//         float vertical = Input.GetAxis(verticalAxis);

//         switch (playerType)
//         {
//             case PlayerType.LeftOnly:
//                 HandleLeftOnlyMovement(horizontal);
//                 break;
//             case PlayerType.RightOnly:
//                 HandleRightOnlyMovement(horizontal);
//                 break;
//             case PlayerType.ForwardBackward:
//                 HandleForwardBackwardMovement(vertical);
//                 break;
//         }
//     }

//     void HandleLeftOnlyMovement(float horizontal)
//     {
//         if (isCarrying && horizontal < -0.1f && rb.linearVelocity.magnitude < maxSpeed)
//         {
//             Vector3 forceDirection = -transform.right * moveForce * Mathf.Abs(horizontal);
//             rb.AddForce(forceDirection);
//         }
//     }

//     void HandleRightOnlyMovement(float horizontal)
//     {
//         if (isCarrying && horizontal > 0.1f && rb.linearVelocity.magnitude < maxSpeed)
//         {
//             Vector3 forceDirection = transform.right * moveForce * horizontal;
//             rb.AddForce(forceDirection);
//         }
//     }

//     void HandleForwardBackwardMovement(float vertical)
//     {
//         if (isCarrying && Mathf.Abs(vertical) > 0.1f && rb.linearVelocity.magnitude < maxSpeed)
//         {
//             Vector3 forceDirection = transform.forward * moveForce * vertical;
//             rb.AddForce(forceDirection);
//         }
//     }

//     public void ConnectToObject(CarryObject carryObj)
//     {
//         carryObject = carryObj;
//         carryObj.ConnectPlayer(rb, GetPlayerIndex());
//         isCarrying = true;

//         // Adjust physics when carrying
//         rb.linearDamping = 2f;
//         rb.angularDamping = 3f;
//     }

//     public void DisconnectFromObject()
//     {
//         if (carryObject != null && isCarrying)
//         {
//             carryObject.DisconnectPlayer(rb);
//             carryObject = null;
//             isCarrying = false;

//             // Reset physics
//             rb.linearDamping = 1f;
//             rb.angularDamping = 2f;
//         }
//     }

//     public int GetPlayerIndex()
//     {
//         // Return index based on player type for positioning
//         switch (playerType)
//         {
//             case PlayerType.LeftOnly: return 0;
//             case PlayerType.RightOnly: return 1;
//             case PlayerType.ForwardBackward: return 2;
//             default: return 0;
//         }
//     }

//     // Optional: Visual debug to show connection status
//     void OnGUI()
//     {
//         if (isCarrying)
//         {
//             GUI.Label(new Rect(10, 10 + (gamepadNumber * 30), 300, 30),
//                      $"Player {gamepadNumber}: Connected to object");
//         }
//     }
// }