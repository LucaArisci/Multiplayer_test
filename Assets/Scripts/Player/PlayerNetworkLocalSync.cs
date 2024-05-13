using UnityEngine;

/// <summary>
/// Syncs the local player's state across the network by sending frequent network packets containing relevent information such as velocity, position and inputs.
/// </summary>
public class PlayerNetworkLocalSync : MonoBehaviour
{
    /// <summary>
    /// How often to send the player's velocity and position across the network, in seconds.
    /// </summary>
    public float StateFrequency = 0.1f;

    private GameManager gameManager;
    private PlayerInputController playerInputController;
    private Rigidbody playerRigidbody;
    private Transform playerTransform;
    private float stateSyncTimer;

    /// <summary>
    /// Called by Unity when this GameObject starts.
    /// </summary>
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerInputController = GetComponent<PlayerInputController>();
        playerRigidbody = GetComponentInChildren<Rigidbody>();
        playerTransform = playerRigidbody.GetComponent<Transform>();
    }

    /// <summary>
    /// Called by Unity every frame after all Update calls have been made.
    /// </summary>
    private void LateUpdate()
    {
        // Send the players current velocity and position every StateFrequency seconds.
        if (stateSyncTimer <= 0)
        {
            // Send a network packet containing the player's velocity and position.
            gameManager.SendMatchState(
                OpCodes.VelocityAndPosition,
                MatchDataJson.VelocityAndPosition(playerRigidbody.velocity, playerTransform.position));

            stateSyncTimer = StateFrequency;
        }

        stateSyncTimer -= Time.deltaTime;

        // If the players input hasn't changed, return early.
        if (!playerInputController.InputChanged)
        {
            return;
        }

        // Send network packet with the player's current input.
        gameManager.SendMatchState(
            OpCodes.Input,
            MatchDataJson.Input(playerInputController.HorizontalInput, playerInputController.VerticalInput)
        );
    }
}
