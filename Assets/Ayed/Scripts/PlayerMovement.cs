using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerType { LeftOnly, RightOnly, ForwardBackward }
    public PlayerType playerType;
    public float moveSpeed = 5f;
    public int gamepadNumber = 1;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        string horizontalAxis = "J" + gamepadNumber + "Horizontal";
        string verticalAxis = "J" + gamepadNumber + "Vertical";
        
        float horizontal = Input.GetAxis(horizontalAxis);
        float vertical = Input.GetAxis(verticalAxis);
        
        Vector3 movement = Vector3.zero;
        
        switch (playerType)
        {
            case PlayerType.LeftOnly:
                movement = HandleLeftOnlyMovement(horizontal);
                break;
            case PlayerType.RightOnly:
                movement = HandleRightOnlyMovement(horizontal);
                break;
            case PlayerType.ForwardBackward:
                movement = HandleForwardBackwardMovement(vertical);
                break;
        }
        
        // Apply movement using Rigidbody for physics
        if (movement != Vector3.zero)
        {
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero; // Stop when not moving
        }
    }
    
    Vector3 HandleLeftOnlyMovement(float horizontal)
    {
        if (horizontal < -0.1f)
        {
            return Vector3.left * Mathf.Abs(horizontal);
        }
        return Vector3.zero;
    }
    
    Vector3 HandleRightOnlyMovement(float horizontal)
    {
        if (horizontal > 0.1f)
        {
            return Vector3.right * horizontal;
        }
        return Vector3.zero;
    }
    
    Vector3 HandleForwardBackwardMovement(float vertical)
    {
        if (vertical > 0.1f)
        {
            return Vector3.forward * vertical;
        }
        else if (vertical < -0.1f)
        {
            return Vector3.back * Mathf.Abs(vertical);
        }
        return Vector3.zero;
    }
}