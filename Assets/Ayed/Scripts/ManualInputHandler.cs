using UnityEngine;
using UnityEngine.InputSystem;

public class ManualInputHandler : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInputMap
    {
        public InputActionReference moveAction;
        public InputActionReference jumpAction;
        public InputActionReference actionAction;
    }
    
    [SerializeField] private PlayerInputMap[] playerInputs = new PlayerInputMap[3];
    
    private void Awake()
    {
        // Manually enable input actions for each player
        for (int i = 0; i < playerInputs.Length; i++)
        {
            if (playerInputs[i].moveAction != null)
                playerInputs[i].moveAction.action.Enable();
                
            if (playerInputs[i].jumpAction != null)
                playerInputs[i].jumpAction.action.Enable();
                
            if (playerInputs[i].actionAction != null)
                playerInputs[i].actionAction.action.Enable();
        }
    }
    
    public Vector2 GetPlayerMovement(int playerId)
    {
        if (playerId < playerInputs.Length && playerInputs[playerId].moveAction != null)
        {
            return playerInputs[playerId].moveAction.action.ReadValue<Vector2>();
        }
        return Vector2.zero;
    }
    
    public bool GetPlayerJump(int playerId)
    {
        if (playerId < playerInputs.Length && playerInputs[playerId].jumpAction != null)
        {
            return playerInputs[playerId].jumpAction.action.triggered;
        }
        return false;
    }
    
    public bool GetPlayerAction(int playerId)
    {
        if (playerId < playerInputs.Length && playerInputs[playerId].actionAction != null)
        {
            return playerInputs[playerId].actionAction.action.triggered;
        }
        return false;
    }
}