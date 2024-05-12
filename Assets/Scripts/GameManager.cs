using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NakamaConnection NakamaConnection;
    public GameObject MainMenu;

    private IDictionary<string, GameObject> players;

    private async void Start()
    {
        players = new Dictionary<string, GameObject>();

        await NakamaConnection.Connect();
    }
    
    void Update()
    {
        
    }
}
