using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the behaviour of the main menu.
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject MatchmakingPanel;
    public Button FindMatchButton;
    public Button CancelMatchmakingButton;
    public Dropdown PlayersDropdown;
    private GameManager gameManager;

    /// <summary>
    /// Called by Unity when this GameObject starts.
    /// </summary>
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        FindMatchButton.onClick.AddListener(FindMatch);
        CancelMatchmakingButton.onClick.AddListener(CancelMatchmaking);
    }

    /// <summary>
    /// Called by Unity when this GameObject is being destroyed.
    /// </summary>
    private void OnDestroy()
    {
        FindMatchButton.onClick.RemoveListener(FindMatch);
        CancelMatchmakingButton.onClick.RemoveListener(CancelMatchmaking);
    }

    /// <summary>
    /// Enables the Find Match button.
    /// </summary>
    public void EnableFindMatchButton()
    {
        FindMatchButton.interactable = true;
    }

    /// <summary>
    /// Disables the Find Match button.
    /// </summary>
    public void DisableFindMatchButton()
    {
        FindMatchButton.interactable = false;
    }

    /// <summary>
    /// Hides the main menu.
    /// </summary>
    public void DeactivateMenu()
    {
        MenuPanel.SetActive(true);
        MatchmakingPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Begins the matchmaking process.
    /// </summary>
    public async void FindMatch()
    {
        MenuPanel.SetActive(false);
        MatchmakingPanel.SetActive(true);

        await gameManager.NakamaConnection.FindMatch(int.Parse(PlayersDropdown.options[PlayersDropdown.value].text));
    }

    /// <summary>
    /// Cancels the matchmaking process.
    /// </summary>
    public async void CancelMatchmaking()
    {
        MenuPanel.SetActive(true);
        MatchmakingPanel.SetActive(false);

        await gameManager.NakamaConnection.CancelMatchmaking();
    }
}
