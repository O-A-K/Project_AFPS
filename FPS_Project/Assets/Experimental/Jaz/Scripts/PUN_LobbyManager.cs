using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// Just here to copy and paste it all over.
/// <summary>
public class PUN_LobbyManager : MonoBehaviour
{
    string mST_GameVersion = "1";

    // UI Variables
    Canvas mCV_MainMenu;

    RectTransform mPN_MainMenu;
    RectTransform mPN_NewRoom;
    RectTransform mPN_JoinRoom;
    RectTransform mPN_Settings;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        ConnectToLobby();
    }

    void Update()
    {
        
    }

    #region Network Functions

    private void ConnectToLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Join the Lobby
            PhotonNetwork.JoinLobby();
        }

        else
        {
            PhotonNetwork.GameVersion = mST_GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void CreateRoom()
    {

    }

    private void JoinRoom()
    {

    }

    private void LeaveRoom()
    {

    }

    #endregion

    #region Network Callbacks & Debugging



    #endregion

    #region UI Management

    void OpenClosePanel(RectTransform vPanel, bool vIsOpen)
    {

    }

    #endregion
}
