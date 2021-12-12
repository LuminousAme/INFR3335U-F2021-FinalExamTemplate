using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;


public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject lobbyPanel;
    public GameObject roomPanel;

    public TMP_InputField createField;
    public TMP_InputField joinInput;
    public TMP_Text errorText;

    public TMP_Text roomName;
    public TMP_Text playerCount;

    public GameObject playerListing;
    public Transform playerListContent;

    public Button startButton;
    public Button SettingsButton;

    public GameObject readyButton;
    public Transform ButtonOrganizer;

    private void Start()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createField.text))
            return;
        PhotonNetwork.CreateRoom(createField.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);

        //set room name
        roomName.text = "Lobby: " + PhotonNetwork.CurrentRoom.Name;

        //get a list of all the players
        Player[] players = PhotonNetwork.PlayerList;

        //update the text
        playerCount.text = "" + players.Length + " Players";

        //spawn in the list of players
        for(int i = 0; i < players.Length; i++)
        {
            //create all of the player in the scroll view
            Instantiate(playerListing, playerListContent).GetComponent<PlayerListing>().SetPlayerInfo(players[i]);

            //only allow the start button to be interactable for the first player
            startButton.interactable = (i == 0);
            SettingsButton.interactable = (i == 0);
        }

        //spawn in the ready button
        Instantiate(readyButton, ButtonOrganizer).GetComponent<ReadyButton>().SetCurrentPlayer(playerListContent.gameObject);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Error Creating Room!";
        Debug.Log("Error creating room!");
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Loading");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListing, playerListContent).GetComponent<PlayerListing>().SetPlayerInfo(newPlayer);

        //get a list of all the players
        Player[] players = PhotonNetwork.PlayerList;

        //update the text
        playerCount.text = "" + players.Length + " Players";
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //get a list of all the players
        Player[] players = PhotonNetwork.PlayerList;

        //update the text
        playerCount.text = "" + players.Length + " Players";
    }

    public void OnClickStartGame()
    {
        PhotonNetwork.LoadLevel("Arena");
    }
}
