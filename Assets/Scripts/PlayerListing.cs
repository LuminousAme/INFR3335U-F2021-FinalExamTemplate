using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;

    private Player photon_player;
    bool isReady = false;
    int thisIndex;
    bool isThisClinet = false;

    public void SetPlayerInfo(Player p, int index, bool thisClinet)
    {
        photon_player = p;
        playerName.text = p.NickName;
        thisIndex = index;
        isThisClinet = thisClinet;

        if (isThisClinet)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { "IsReady", isReady } };
            PhotonNetwork.SetPlayerCustomProperties(props);
            ReadyButton.ReadyClickedEvent += UpdateReady;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (isThisClinet)
        {
            ReadyButton.ReadyClickedEvent -= UpdateReady;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(photon_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    public void UpdateReady(bool value)
    {
        GameObject unready = transform.GetChild(1).gameObject;
        GameObject ready = transform.GetChild(2).gameObject;

        isReady = value;

        ready.SetActive(value);
        unready.SetActive(!value);

        if (isThisClinet)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { "IsReady", isReady } };
            PhotonNetwork.SetPlayerCustomProperties(props);
        }
    }

    public void UpdateFromServer(bool value)
    {
        if(!isThisClinet)
        {
            GameObject unready = transform.GetChild(1).gameObject;
            GameObject ready = transform.GetChild(2).gameObject;

            isReady = value;

            ready.SetActive(value);
            unready.SetActive(!value);
        }
    }
 }
