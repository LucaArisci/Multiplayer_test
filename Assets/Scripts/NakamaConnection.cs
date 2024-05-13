using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Nakama;

public class NakamaConnection : MonoBehaviour
{
    private string Scheme = "http";
    private string Host = "129.152.5.101";
    private int Port = 7350;
    private string ServerKey = "defaultkey";

    //private const string SessionPrefName = "nakama.session";
    private const string DeviceIdentifierPrefName = "nakama.deviceUniqueIdentifier";

    public IClient Client;
    public ISession Session;
    public ISocket Socket;

    private string currentMatchmakingTicket;
    private string MatchId;

    //async void Start()
    //{
    //    Client = new Client(Scheme, Host, Port, ServerKey, UnityWebRequestAdapter.Instance);
    //    Session = await Client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
    //    Socket = Client.NewSocket(true);
    //    await Socket.ConnectAsync(Session, true);

    //    Socket.ReceivedMatchmakerMatched += OnReceivedMatchmakerMatched;

    //    Debug.Log(Session);
    //    Debug.Log(Socket);
    //}

    public async Task Connect()
    {
        Client = new Nakama.Client(Scheme, Host, Port, ServerKey, UnityWebRequestAdapter.Instance);
       
        string deviceId;

        // If we've already stored a device identifier in PlayerPrefs then use that.
        //if (PlayerPrefs.HasKey(DeviceIdentifierPrefName))
        //{
        //    deviceId = PlayerPrefs.GetString(DeviceIdentifierPrefName);
        //}
        //else
        //{
            // If we've reach this point, get the device's unique identifier or generate a unique one.
            deviceId = SystemInfo.deviceUniqueIdentifier;
            if (deviceId == SystemInfo.unsupportedIdentifier)
            {
                deviceId = System.Guid.NewGuid().ToString();
            }

            //// Store the device identifier to ensure we use the same one each time from now on.
            //PlayerPrefs.SetString(DeviceIdentifierPrefName, deviceId);
        //}

        // Use Nakama Device authentication to create a new session using the device identifier.
        Session = await Client.AuthenticateDeviceAsync(deviceId);

        //// Store the auth token that comes back so that we can restore the session later if necessary.
        //PlayerPrefs.SetString(SessionPrefName, Session.AuthToken);

        // Open a new Socket for realtime communication.
        Socket = Client.NewSocket();
        await Socket.ConnectAsync(Session, true);

        Debug.Log(Session);
        Debug.Log(Socket);
    }

    public async Task FindMatch(int minPlayers = 2)
    {
        var matchmakerTicket = await Socket.AddMatchmakerAsync("*", minPlayers, minPlayers);

        Debug.Log("Matchmaking ticket: " + matchmakerTicket);

        currentMatchmakingTicket = matchmakerTicket.Ticket;
    }

    public async Task CancelMatchmaking()
    {
        await Socket.RemoveMatchmakerAsync(currentMatchmakingTicket);
    }

    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmakerMatched)
    {
        var match = await Socket.JoinMatchAsync(matchmakerMatched);

        Debug.Log("Our session ID: " + match.Self.SessionId);
        Debug.Log("Our match: " + match);

        MatchId = match.Id;

        foreach (var user in match.Presences)
        {
            Debug.Log("Connected User Session ID: " + user.SessionId);
        }
    }
}
