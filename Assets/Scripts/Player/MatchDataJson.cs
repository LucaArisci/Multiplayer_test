using System.Collections.Generic;
using Nakama.TinyJson;
using UnityEngine;

/// <summary>
/// A static class that creates JSON string network messages.
/// </summary>
public static class MatchDataJson
{
    /// <summary>
    /// Creates a network message containing velocity and position.
    /// </summary>
    /// <param name="velocity">The velocity to send.</param>
    /// <param name="position">The position to send.</param>
    /// <returns>A JSONified string containing velocity and position data.</returns>
    public static string VelocityAndPosition(Vector3 velocity, Vector3 position)
    {
        var values = new Dictionary<string, string>
        {
            { "velocity.x", velocity.x.ToString() },
            { "velocity.y", velocity.y.ToString() },
            { "velocity.z", velocity.z.ToString() },
            { "position.x", position.x.ToString() },
            { "position.y", position.y.ToString() },
            { "position.z", position.z.ToString() }
        };

        return values.ToJson();
    }

    /// <summary>
    /// Creates a network message containing player input.
    /// </summary>
    /// <param name="horizontalInput">The current horizontal input.</param>
    /// <param name="verticalInput">The current vertical input.</param>
    /// <returns>A JSONified string containing player input.</returns>
    public static string Input(float horizontalInput, float verticalInput)
    {
        var values = new Dictionary<string, string>
        {
            { "horizontalInput", horizontalInput.ToString() },
            { "verticalInput", verticalInput.ToString() },
        };

        return values.ToJson();
    }
}
