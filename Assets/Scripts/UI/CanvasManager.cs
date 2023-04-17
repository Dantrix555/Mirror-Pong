using Mirror;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    #region Fields and properties

    [SerializeField]
    private HomeUIController _homeUIController;
    [SerializeField]
    private HUDController _hudController;
    [SerializeField]
    private GameOverUIController _gameOverUIController;

    public HomeUIController HomeController => _homeUIController;
    public HUDController HudController => _hudController;
    public GameOverUIController GameOverUIController => _gameOverUIController;

    #endregion

    #region Unity Methods

    public void StartGameCanvas(NetworkManager networkManager)
    {
        _homeUIController.SetupHomeUI(networkManager);
        _gameOverUIController.SetupGameOverUI(networkManager, () => _homeUIController.gameObject.SetActive(true));
        _hudController.ClearScoreText();

        gameObject.SetActive(true);
    }

    #endregion

}
