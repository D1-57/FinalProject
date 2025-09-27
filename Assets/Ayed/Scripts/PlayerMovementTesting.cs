using UnityEngine;

public class PlayerMovementTesting : MonoBehaviour
{



    public enum PlayerType { LeftOnly, RightOnly, ForwardBackward }
    public PlayerType playerType;
    public float moveSpeed = 5f;
    public int gamepadNumber = 1;

    // Keyboard controls for testing
    public KeyCode keyboardPositiveKey = KeyCode.None;
    public KeyCode keyboardNegativeKey = KeyCode.None;

    void Update()
    {
        float inputValue = 0f;

        // Try gamepad first, then keyboard fallback
        if (gamepadNumber <= 4) // Only try if gamepad number is valid
        {
            inputValue = GetGamepadInput();
        }

        // If no gamepad input, try keyboard
        if (Mathf.Abs(inputValue) < 0.1f)
        {
            inputValue = GetKeyboardInput();
        }

        // Apply movement based on player type
        switch (playerType)
        {
            case PlayerType.LeftOnly:
                if (inputValue < -0.1f) HandleLeftOnlyMovement(Mathf.Abs(inputValue));
                break;
            case PlayerType.RightOnly:
                if (inputValue > 0.1f) HandleRightOnlyMovement(inputValue);
                break;
            case PlayerType.ForwardBackward:
                HandleForwardBackwardMovement(inputValue);
                break;
        }
    }

    float GetGamepadInput()
    {
        string horizontalAxis = "J" + gamepadNumber + "Horizontal";
        string verticalAxis = "J" + gamepadNumber + "Vertical";

        return playerType == PlayerType.ForwardBackward ?
            Input.GetAxis(verticalAxis) : Input.GetAxis(horizontalAxis);
    }

    float GetKeyboardInput()
    {
        float inputValue = 0f;

        if (Input.GetKey(keyboardPositiveKey))
            inputValue = 1f;
        else if (Input.GetKey(keyboardNegativeKey))
            inputValue = -1f;

        return inputValue;
    }

    void HandleLeftOnlyMovement(float intensity)
    {
        Vector3 movement = -transform.right * moveSpeed * intensity * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    void HandleRightOnlyMovement(float intensity)
    {
        Vector3 movement = transform.right * moveSpeed * intensity * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    void HandleForwardBackwardMovement(float inputValue)
    {
        if (Mathf.Abs(inputValue) > 0.1f)
        {
            Vector3 movement = (inputValue > 0 ? transform.forward : -transform.forward)
                             * moveSpeed * Mathf.Abs(inputValue) * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
    }
}
