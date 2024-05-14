using Nakama;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NakamaConnection NakamaConnection;
    public GameObject MainMenu;
    public GameObject SpawnPoints;
    public GameObject NetworkLocalPlayerPrefab;
    public GameObject NetworkRemotePlayerPrefab;

    private IUserPresence localUser;
    private IDictionary<string, GameObject> players;
    private IMatch currentMatch;

    private async void Start()
    {
        players = new Dictionary<string, GameObject>();

        var mainThread = UnityMainThreadDispatcher.Instance();

        await NakamaConnection.Connect();

        MainMenu.GetComponent<MainMenu>().EnableFindMatchButton();

        NakamaConnection.Socket.ReceivedMatchmakerMatched += m => mainThread.Enqueue(() => OnReceivedMatchmakerMatched(m));
        NakamaConnection.Socket.ReceivedMatchPresence += m => mainThread.Enqueue(() => OnReceivedMatchPresence(m));
    }

    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matched)
    {
        // Cache a reference to the local user.
        localUser = matched.Self.Presence;

        // Join the match.
        var match = await NakamaConnection.Socket.JoinMatchAsync(matched);

        // Disable the main menu UI and enable the in-game UI.
        // In a larger game we would probably transition to a totally different scene.
        MainMenu.GetComponent<MainMenu>().DeactivateMenu();

        // Spawn a player instance for each connected user.
        foreach (var user in match.Presences)
        {
            SpawnPlayer(match.Id, user);
        }

        // Cache a reference to the current match.
        currentMatch = match;
    }

    private void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
    {
        // For each new user that joins, spawn a player for them.
        foreach (var user in matchPresenceEvent.Joins)
        {
            SpawnPlayer(matchPresenceEvent.MatchId, user);
        }

        // For each player that leaves, despawn their player.
        foreach (var user in matchPresenceEvent.Leaves)
        {
            if (players.ContainsKey(user.SessionId))
            {
                Destroy(players[user.SessionId]);
                players.Remove(user.SessionId);
            }
        }
    }

    private void SpawnPlayer(string matchId, IUserPresence user, int spawnIndex = -1)
    {
        // If the player has already been spawned, return early.
        if (players.ContainsKey(user.SessionId))
        {
            return;
        }

        // If the spawnIndex is -1 then pick a spawn point at random, otherwise spawn the player at the specified spawn point.
        var spawnPoint = spawnIndex == -1 ?
            SpawnPoints.transform.GetChild(Random.Range(0, SpawnPoints.transform.childCount - 1)) :
            SpawnPoints.transform.GetChild(spawnIndex);

        // Set a variable to check if the player is the local player or not based on session ID.
        var isLocal = user.SessionId == localUser.SessionId;

        // Choose the appropriate player prefab based on if it's the local player or not.
        var playerPrefab = isLocal ? NetworkLocalPlayerPrefab : NetworkRemotePlayerPrefab;

        // Spawn the new player.
        var player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Setup the appropriate network data values if this is a remote player.
        if (!isLocal)
        {
            player.GetComponent<PlayerNetworkRemoteSync>().NetworkData = new RemotePlayerNetworkData
            {
                MatchId = matchId,
                User = user
            };
        }

        // Add the player to the players array.
        players.Add(user.SessionId, player);
    }

    public async Task SendMatchStateAsync(long opCode, string state)
    {
        await NakamaConnection.Socket.SendMatchStateAsync(currentMatch.Id, opCode, state);
    }

    /// <summary>
    /// Sends a match state message across the network.
    /// </summary>
    /// 
    /// <param name="opCode">The operation code.</param>
    /// <param name="state">The stringified JSON state data.</param>
    public void SendMatchState(long opCode, string state)
    {
        NakamaConnection.Socket.SendMatchStateAsync(currentMatch.Id, opCode, state);
    }
}
