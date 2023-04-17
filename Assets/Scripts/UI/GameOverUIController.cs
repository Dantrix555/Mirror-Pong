using TMPro;
using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{

    #region Fields and properties

    [SerializeField]
    private TMP_Text _winStateText;

    [SerializeField]
    private Button _searchOtherMatchButton;
    [SerializeField]
    private Button _exitGameButton;

    private NetworkManager _networkManager;

    #endregion

    #region Public Methods

    public void SetupGameOverUI(NetworkManager networkManager, Action onHideGameOverScreen)
    {
        _winStateText.text = string.Empty;
        _searchOtherMatchButton.onClick.AddListener(() => { HideScreenAndSearchNewGame(); onHideGameOverScreen?.Invoke(); });
        _exitGameButton.onClick.AddListener(Application.Quit);

        _networkManager = networkManager;

        gameObject.SetActive(false);
    }

    public void SetWinText(bool actualPlayerWon)
    {
        _winStateText.text = actualPlayerWon ? "You win" : "You Lose";
        gameObject.SetActive(true);
    }

    #endregion

    #region Inner Methods

    private void HideScreenAndSearchNewGame()
    {
        _winStateText.text = string.Empty;

        if (NetworkServer.active && NetworkClient.isConnected)
            _networkManager.StopHost();
        if (NetworkClient.isConnected)
            _networkManager.StopClient();

        gameObject.SetActive(false);
    }

    #endregion

}
