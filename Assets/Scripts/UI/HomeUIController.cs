using TMPro;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIController : MonoBehaviour
{

    #region Fields and properties

    [SerializeField]
    private TMP_InputField _ipAddressInputField;

    [SerializeField]
    private Button _hostButton;
    [SerializeField]
    private Button _clientButton;
    [SerializeField]
    private Button _exitGameButton;

    private NetworkManager _networkManager;

    private const string DEFAULT_IP_ADDRESS = "localhost";

    #endregion

    #region Public Methods

    public void SetupHomeUI(NetworkManager networkManager)
    {
        _ipAddressInputField.text = $"{DEFAULT_IP_ADDRESS}";
        _ipAddressInputField.onValueChanged.AddListener(UpdateIpAddress);

        _hostButton.onClick.AddListener(() => { networkManager.StartHost(); gameObject.SetActive(false); });
        _clientButton.onClick.AddListener(() => { networkManager.StartClient(); gameObject.SetActive(false); });
        _exitGameButton.onClick.AddListener(Application.Quit);

        this._networkManager = networkManager;
        gameObject.SetActive(true);
    }

    #endregion

    #region Inner Methods

    private void UpdateIpAddress(string ipAddress) => _networkManager.networkAddress = ipAddress;

    #endregion

}
