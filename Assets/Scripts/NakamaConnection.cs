using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;

public class NakamaConnection : MonoBehaviour
{
    private string scheme = "http";
    private string host = "129.152.5.101";
    private int port = 7350;
    private string serverKey = "defaultkey";

    private IClient client;
    private ISession session;
    private ISocket socket;

    private string ticket;

    async void Start()
    {
        client = new Client(scheme, host, port, serverKey, UnityWebRequestAdapter.Instance);
        session = await client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
        socket = client.NewSocket(true);
        await socket.ConnectAsync(session, true);

        socket.ReceivedMatchmakerMatched += OnReceivedMatchmakerMatched;

        Debug.Log(session);
        Debug.Log(socket);
    }

    public async void FindMatch()
    {
        Debug.Log("FindingMatch");

        var matchmakingTicket = await socket.AddMatchmakerAsync("*", 2, 2);

        Debug.Log("Matchmaking ticket: " + matchmakingTicket);

        ticket = matchmakingTicket.Ticket;
    }

    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmakerMatched)
    {
        var match = await socket.JoinMatchAsync(matchmakerMatched);

        Debug.Log("Our session ID: " + match.Self.SessionId);
        Debug.Log("Our match: " + match);


        foreach (var user in match.Presences)
        {
            Debug.Log("Connected User Session ID: " + user.SessionId);
        }
    }
}
