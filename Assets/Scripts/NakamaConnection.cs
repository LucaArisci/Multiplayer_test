using System.Threading.Tasks;
using UnityEngine;
using Nakama;

public class NakamaConnection : MonoBehaviour
{
    private string Scheme = "http";
    private string Host = "129.152.5.101";
    private int Port = 7350;
    private string ServerKey = "defaultkey";

    public IClient Client;
    public ISession Session;
    public ISocket Socket;

    private string currentMatchmakingTicket;

    public async Task Connect()
    {
        Client = new Nakama.Client(Scheme, Host, Port, ServerKey, UnityWebRequestAdapter.Instance);
       
        string deviceId;

        deviceId = SystemInfo.deviceUniqueIdentifier;

        if (deviceId == SystemInfo.unsupportedIdentifier)
        {
            deviceId = System.Guid.NewGuid().ToString();
        }

        // Use Nakama Device authentication to create a new session using the device identifier.
        Session = await Client.AuthenticateDeviceAsync(deviceId);

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
}
