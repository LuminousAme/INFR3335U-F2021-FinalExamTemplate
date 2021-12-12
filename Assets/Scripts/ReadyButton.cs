using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ReadyButton : MonoBehaviourPun
{
    public GameObject currentPlayer;

    private GameObject ready, unready;

    public bool readyFlag = false;

    public delegate void ClickUpdated(bool value);
    public static event ClickUpdated ReadyClickedEvent;

    public void SetCurrentPlayer(GameObject playerListContent)
    {
        TMP_Text[] playerInfoList = playerListContent.GetComponentsInChildren<TMP_Text>();
        foreach(var temp in playerInfoList)
        {
            if (temp.text == PhotonNetwork.NickName)
                currentPlayer = temp.gameObject.transform.parent.gameObject;
        }
    }

    public void OnClickReady()
    {
        unready = currentPlayer.transform.GetChild(1).gameObject;
        ready = currentPlayer.transform.GetChild(2).gameObject;

        readyFlag = !readyFlag;

        ready.SetActive(readyFlag);
        unready.SetActive(!readyFlag);

        ReadyClickedEvent?.Invoke(readyFlag);
    }
}
