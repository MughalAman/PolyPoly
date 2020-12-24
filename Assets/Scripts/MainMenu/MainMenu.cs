using TMPro;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject findMatchPanel = null;
    [SerializeField] private GameObject waitingStatusPanel = null;
    [SerializeField] private TextMeshProUGUI waitingStatusText = null;
    [SerializeField] private Button StartGame = null;
    [SerializeField] private Toggle CanJoin = null;

    private bool isConnecting = false;

    private const string GameVersion = "0.1";
    private const int MaxPlayersPerRoom = 5;

    //On awake automatically sync scene between players
    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

    //Find match for player
    public void FindMatch()
    {
        isConnecting = true;

        findMatchPanel.SetActive(false);
        waitingStatusPanel.SetActive(true);

        waitingStatusText.text = "Searching...";
        //Check if client is already connected if it's join to a random room
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //else connect using photons settings
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //When connected to photon services join a random room
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master!");
        if(isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    //Show find match screen when client gets disconnected from server and debug.log it's cause
    public override void OnDisconnected(DisconnectCause cause)
    {
        waitingStatusPanel.SetActive(false);
        findMatchPanel.SetActive(true);
        StartGame.gameObject.SetActive(false);
        CanJoin.gameObject.SetActive(false);

        Debug.Log($"Disconnected due to {cause}");
    }

    //If there is no rooms available create a room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No rooms available, creating room");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    //When client has joined a room check if there is enough players to start the match
    public override void OnJoinedRoom()
    {
        Debug.Log("Client successfully connected to a room!");

        int PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if(PlayerCount != MaxPlayersPerRoom)
        {
            waitingStatusText.text = "Waiting for players...";
            Debug.Log("Client waiting for players");
        }
        else
        {
            Debug.Log("Ready to start a match!");
            waitingStatusText.text = "Player Found!";
        }
    }
    //When another player enters clients room check if room is full and if it's then start the match
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        waitingStatusText.text = "Player joined! Players: " + PhotonNetwork.CurrentRoom.PlayerCount;

        //Give host the ability to start the game even when it's not full
        StartGame.gameObject.SetActive(true);
        CanJoin.gameObject.SetActive(true);

        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            waitingStatusText.text = "Room is full!";
            PhotonNetwork.CurrentRoom.IsOpen = CanJoin.isOn;
            Debug.Log("Ready to start a match!");

            PhotonNetwork.LoadLevel("Level_Main");
        }
    }

    public void StartAnyway()
    {
        //Start game without it being full
        waitingStatusText.text = "Starting match...";
        PhotonNetwork.CurrentRoom.IsOpen = CanJoin.isOn;
        Debug.Log("Starting match");

        PhotonNetwork.LoadLevel("Level_Main");
    }

    //Load offline level
    public void Offline()
    {
        SceneManager.LoadScene(2);
    }

}
