using UnityEngine;
using UnityEngine.InputSystem;

public class PreSpawnedInputManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerAssignment
    {
        public PreSpawnedPlayerController playerController;
        public InputDevice device;
        public string controlScheme = "Gamepad";
    }
    
    [SerializeField] private PlayerAssignment[] playerAssignments = new PlayerAssignment[3];
    
    private void Start()
    {
        AssignInputDevices();
    }
    
    private void AssignInputDevices()
    {
        // Get all connected gamepads
        var gamepads = Gamepad.all;
        
        for (int i = 0; i < playerAssignments.Length; i++)
        {
            if (playerAssignments[i].playerController == null) continue;
            
            // Assign gamepad if available, otherwise use keyboard splits
            if (i < gamepads.Count)
            {
                AssignGamepadToPlayer(playerAssignments[i].playerController, gamepads[i]);
            }
            else
            {
                AssignKeyboardToPlayer(playerAssignments[i].playerController, i);
            }
        }
    }
    
    private void AssignGamepadToPlayer(PreSpawnedPlayerController player, Gamepad gamepad)
    {
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.SwitchCurrentControlScheme("Gamepad", gamepad);
            Debug.Log($"Assigned {gamepad.name} to {player.gameObject.name}");
        }
    }
    
    private void AssignKeyboardToPlayer(PreSpawnedPlayerController player, int playerIndex)
    {
        // Different keyboard regions for each player
        string controlScheme = playerIndex switch
        {
            0 => "KeyboardWASD",  // Player 1: WASD
            1 => "KeyboardArrows", // Player 2: Arrow Keys
            2 => "KeyboardIJKL",   // Player 3: IJKL
            _ => "Keyboard"
        };
        
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.SwitchCurrentControlScheme(controlScheme, Keyboard.current);
            Debug.Log($"Assigned {controlScheme} to {player.gameObject.name}");
        }
    }
    
    // Manual device assignment during gameplay
    public void ReassignDevice(int playerIndex, InputDevice newDevice)
    {
        if (playerIndex < playerAssignments.Length && playerAssignments[playerIndex].playerController != null)
        {
            PlayerInput playerInput = playerAssignments[playerIndex].playerController.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.SwitchCurrentControlScheme("Gamepad", newDevice);
                playerAssignments[playerIndex].device = newDevice;
            }
        }
    }
}