using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputControlChanger : MonoBehaviour
{
    public PlayerInput playerInput;

    [ContextMenu("Switch to Keyboard & Mouse")]
    private void SwitchToKeyboardMouse()
    {
        SwitchControlScheme("Keyboard&Mouse");
    }

    [ContextMenu("Switch to Gamepad")]
    private void SwitchToGamepad()
    {
        SwitchControlScheme("Gamepad");
    }

    private void SwitchControlScheme(string controlScheme)
    {
        if (playerInput != null)
        {
            if (playerInput.actions.FindActionMap("Player") != null)
            {
                playerInput.SwitchCurrentControlScheme(controlScheme);
                Debug.Log($"Switched to {controlScheme} scheme.");
            }
            else
            {
                Debug.LogWarning("No 'Player' action map found on PlayerInput.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerInput component not assigned.");
        }
    }

    private void Reset()
    {
        playerInput = GetComponent<PlayerInput>();
    }
}