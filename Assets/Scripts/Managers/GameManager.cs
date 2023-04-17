using UnityEngine;
using System;
using Mirror;

public class GameManager : NetworkBehaviour
{
    #region Fields and properties

    [SerializeField]
    private int _maxGameScore;

    [SyncVar]
    private HUDController _hudController;

    private bool _gameStarted = false;

    [SyncVar]
    private int _leftRacketScore = 0;
    [SyncVar]
    private int _rightRacketScore = 0;

    private Transform _leftSidePlayerTransform;
    private Transform _rightSidePlayerTransform;
    private Transform _ballTransform;

    private Action onGamePoint;
    private Action<bool> onGameFinished;

    [SyncVar]
    private bool _gameIsFinished;

    public Action<bool> OnGameFinished { set => onGameFinished = value; }

    #endregion

    #region Unity Methods

    [ServerCallback]
    private void Update()
    {
        _gameStarted = CheckIfGameHasStarted();

        if (!_gameStarted)
            return;

        if(_ballTransform.position.x > _rightSidePlayerTransform.position.x || _ballTransform.position.x < _leftSidePlayerTransform.position.x)
        {
            _leftRacketScore = (_ballTransform.position.x > _rightSidePlayerTransform.position.x) ? ++_leftRacketScore : _leftRacketScore;
            _rightRacketScore = (_ballTransform.position.x < _leftSidePlayerTransform.position.x) ? ++_rightRacketScore : _rightRacketScore;

            _hudController.UpdateScore(_leftRacketScore, _rightRacketScore);
            UpdateScore();
        }
    }

    #endregion

    #region Public Methods

    [ServerCallback]
    public void SetupGameManager(Transform leftSidePlayerTransform, Transform rightSidePlayerTransform, Transform ballTransform, HUDController hudController, Action onGamePoint)
    {
        _leftSidePlayerTransform = leftSidePlayerTransform;
        _rightSidePlayerTransform = rightSidePlayerTransform;
        _ballTransform = ballTransform;
        _hudController = hudController;


        this.onGamePoint = onGamePoint;
        onGamePoint?.Invoke();

        _leftRacketScore = 0;
        _rightRacketScore = 0;
        _hudController.UpdateScore(_leftRacketScore, _rightRacketScore);

        _gameIsFinished = false;
    }

    public void FinishMatch(bool forceMatchWinner, bool forcePlayerWon)
    {
        if (_gameIsFinished)
            return;

        _leftSidePlayerTransform = null;
        _rightSidePlayerTransform = null;
        _ballTransform = null;

        onGamePoint = null;

        SetGameFinishUI(forceMatchWinner, forcePlayerWon);
    }

    #endregion

    #region Inner Methods

    [ClientRpc]
    private void SetGameFinishUI(bool forceMatchWinner, bool forcePlayerWon)
    {
        bool playerWonConditionState = forceMatchWinner ? forcePlayerWon : ActualPlayerWonMatch(NetworkServer.active && NetworkClient.isConnected);

        onGameFinished?.Invoke(playerWonConditionState);
        _hudController.ClearScoreText();
        _gameIsFinished = true;
    }

    private void UpdateScore()
    {
        if(_leftRacketScore < _maxGameScore && _rightRacketScore < _maxGameScore)
            onGamePoint?.Invoke();
        else
            FinishMatch(false, false);
    }

    private bool ActualPlayerWonMatch(bool isServerPlayer)
    {
        return ((isServerPlayer && _leftRacketScore > _rightRacketScore) || (!isServerPlayer && _leftRacketScore < _rightRacketScore));
    }

    private bool CheckIfGameHasStarted() => _leftSidePlayerTransform && _rightSidePlayerTransform && _ballTransform;

    #endregion

}
