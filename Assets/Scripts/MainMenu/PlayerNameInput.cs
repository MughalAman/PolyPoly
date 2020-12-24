using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButtion = null;

    private const string PlayerPrefsNameKey = "PlayerName";

    //run SetUpInputField on startup
    private void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        //Check if player has already set a name
        if(!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
        //if not grab lets name
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string defaultName)
    {
        //enable continue button if inputfield is not empty
        continueButtion.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        //Save player name
        string playerName = nameInputField.text;

        PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
    }
}
