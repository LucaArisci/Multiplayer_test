using Nakama;
using Nakama.TinyJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public NakamaConnection NakamaConnection;
    public GameObject MainMenu;

    private IUserPresence localUser;
    private IDictionary<string, GameObject> players;

    private async void Start()
    {
        players = new Dictionary<string, GameObject>();

        var mainThread = UnityMainThreadDispatcher.Instance();

        await NakamaConnection.Connect();

        MainMenu.GetComponent<MainMenu>().EnableFindMatchButton();

        NakamaConnection.Socket.ReceivedMatchmakerMatched += m => mainThread.Enqueue(() => OnReceivedMatchmakerMatched(m));
    }

    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matched)
    {
        // Cache a reference to the local user.
        localUser = matched.Self.Presence;

        // Join the match.
        var match = await NakamaConnection.Socket.JoinMatchAsync(matched);

        //// Disable the main menu UI and enable the in-game UI.
        //// In a larger game we would probably transition to a totally different scene.
        //MainMenu.GetComponent<MainMenu>().DeactivateMenu();
        //InGameMenu.SetActive(true);

        //// Play the match music.
        //AudioManager.PlayMatchTheme();

        //// Spawn a player instance for each connected user.
        //foreach (var user in match.Presences)
        //{
        //    SpawnPlayer(match.Id, user);
        //}

        //// Cache a reference to the current match.
        //currentMatch = match;
    }
}
