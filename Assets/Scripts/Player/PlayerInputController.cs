
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [HideInInspector] public float HorizontalInput;
    [HideInInspector] public float VerticalInput;
    [HideInInspector] public bool InputChanged;

    private Movement playerMovementController;

    /// <summary>
    /// Called by Unity when this GameObject starts.
    /// </summary>
    private void Start()
    {
        playerMovementController = GetComponentInChildren<Movement>();
    }

    /// <summary>
    /// Called by Unity every frame.
    /// </summary>
    private void Update()
    {
        // Get the current input states.
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        // Set a boolean (true/false) value to indicate if any input state has changed since the last frame.
        InputChanged = (horizontalInput != HorizontalInput || verticalInput != VerticalInput);

        // Cache the new input states in public variables that can be read elsewhere.
        HorizontalInput = horizontalInput;
        VerticalInput = verticalInput;

        // Set inputs on Player Controllers.
        playerMovementController.SetHorizontalMovement(HorizontalInput);
        playerMovementController.SetVerticalMovement(VerticalInput);
    }
}
