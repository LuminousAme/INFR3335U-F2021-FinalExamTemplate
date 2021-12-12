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

    private List<GameObject> playerListingList;
    private List<bool> playerReady;
    private int playerIndex;

    private void Start()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
        playerListingList = new List<GameObject>();
        playerReady = new List<bool>();
    }

    private void LateUpdate()
    {
        //iterate over all the players in the photon list
        int i = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {

            object isReady;
            if (player.CustomProperties.TryGetValue("IsReady", out isReady))
            {
                playerListingList[i].GetComponent<PlayerListing>().UpdateFromServer((bool)isReady);
                playerReady[i] = (bool)isReady;
            }

            i++;
        }

        //if this is the first player check if the start button should be interactable now
        if (playerIndex == 0)
        {
            //get a list of all the players
            Player[] players = PhotonNetwork.PlayerList;

            //check if all the players are ready
            for (i = 0; i < players.Length; i++)
            {
                //check if the start button should be interactable
                startButton.interactable = playerReady[i];

                //if we find one false break
                if (!playerReady[i]) break;
            }
        }
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
        for (int i = 0; i < players.Length; i++)
        {
            //create all of the player in the scroll view
            GameObject newObj = Instantiate(playerListing, playerListContent);
            newObj.GetComponent<PlayerListing>().SetPlayerInfo(players[i], i, i == players.Length - 1);
            playerListingList.Add(newObj);

            //only allow the start button to be interactable for the first player
            startButton.interactable = false;
            SettingsButton.interactable = (i == 0);
            playerIndex = i;
            playerReady.Add(false);
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
        GameObject newObj = Instantiate(playerListing, playerListContent);
        newObj.GetComponent<PlayerListing>().SetPlayerInfo(newPlayer, playerListingList.Count, false);
        playerListingList.Add(newObj);
        playerReady.Add(false);

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

    [PunRPC]
    private void UpdateReadyFlag(bool value, int thisIndex)
    {
        //update the readied
        playerReady[thisIndex] = value;

        //if this is the first player check if the start button should be interactable now
        if (playerIndex == 0)
        {
            //get a list of all the players
            Player[] players = PhotonNetwork.PlayerList;

            //check if all the players are ready
            for (int i = 0; i < players.Length; i++)
            {
                //check if the start button should be interactable
                startButton.interactable = playerReady[i];

                //if we find one false break
                if (!playerReady[i]) break;
            }
        }
    }


}
