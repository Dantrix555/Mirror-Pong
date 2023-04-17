using UnityEngine;
using Mirror;
using TMPro;

public class HUDController : NetworkBehaviour
{

    #region Fields and properties

    [Header("HUD Components")]
    [SerializeField]
    private TMP_Text scoreLabel;

    #endregion

    #region Public Methods

    [ClientRpc]
    public void UpdateScore(int leftRacketScore, int rightRacketScore)
    {
        scoreLabel.text = $"{leftRacketScore} - {rightRacketScore}";
        gameObject.SetActive(true);
    }

    public void ClearScoreText()
    {
        scoreLabel.text = string.Empty;
        gameObject.SetActive(false);
    }

    #endregion


}
