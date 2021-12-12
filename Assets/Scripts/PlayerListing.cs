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

    public void SetPlayerInfo(Player p)
    {
        photon_player = p;
        playerName.text = p.NickName;
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
}
