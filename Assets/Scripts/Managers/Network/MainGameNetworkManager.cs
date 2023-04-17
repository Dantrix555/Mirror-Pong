using UnityEngine;
using Mirror;

/// <summary>
/// Main Game controller network manager
/// </summary>
public class MainGameNetworkManager : NetworkManager
{

    #region Fields and Properties

    [Header("Controllers")]
    [SerializeField]
    private GameManager _mainGameManager;
    [SerializeField]
    private CanvasManager _canvasManager;

    [Space(5)]
    [Header("Game Network Objects")]
    [SerializeField]
    private Transform _leftPlayerSpawnPosition;

    [SerializeField]
    private Transform _rightPlayerSpawnPosition;

    [SerializeField]
    private BallController _ballPrefab;

    private BallController _cachedBall;

    #endregion

    #region Unity Methods

    public override void Awake()
    {
        base.Awake();
        _canvasManager.StartGameCanvas(this);
        _mainGameManager.OnGameFinished = _canvasManager.GameOverUIController.SetWinText;
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Escape) && NetworkClient.isConnected)
        {
            if (NetworkServer.active)
                StopHost();
            else
                StopClient();

            if (numPlayers >= 2)
                _mainGameManager.FinishMatch(true, false);
            else
                _canvasManager.GameOverUIController.SetWinText(false);
        }
    }

    #endregion

    #region Network Manager override methods

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPosition = numPlayers == 0 ? _leftPlayerSpawnPosition : _rightPlayerSpawnPosition;
        GameObject newPlayerInstance = Instantiate(playerPrefab, startPosition.position, Quaternion.identity);
        
        NetworkServer.AddPlayerForConnection(conn, newPlayerInstance);


        if (numPlayers == 2)
        {
            _cachedBall = Instantiate(_ballPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(_cachedBall.gameObject);

            _mainGameManager.SetupGameManager(_leftPlayerSpawnPosition, _rightPlayerSpawnPosition, _cachedBall.transform, _canvasManager.HudController, _cachedBall.SetupBall);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (_cachedBall != null)
            NetworkServer.Destroy(_cachedBall.gameObject);

        _mainGameManager.FinishMatch(true, true);

        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect()
    {
        //Set win text because the server disconnects and client is still connected
        _canvasManager.GameOverUIController.SetWinText(true);

        base.OnClientDisconnect();
    }

    #endregion

}
