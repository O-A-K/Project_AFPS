﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Just here to copy and paste it all over.
/// <summary>
public class PUN_LobbyManager : MonoBehaviourPunCallbacks
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

    // Connect/Disconnect from Server, Create/Join/Leave Rooms.
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
        PhotonNetwork.CreateRoom();
    }

    private void JoinRoom()
    {

    }

    private void LeaveRoom()
    {

    }

    #endregion

    // PUN callbacks and some added debugging.
    #region Network Callbacks & Debugging

    public override void OnConnectedToMaster()
    {
        Debug.Log("FPS_Project/PUN_LobbyManager: OnConnectedToMaster()");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("FPS_Project/PUN_LobbyManager: OnDisconnected() " + cause);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    #endregion

    #region UI Management

    public void StartMenu()
    {

    }

    public void OpenCloseMenuPanel(RectTransform vPanel, bool vOpening)
    {
        if (vOpening) // If we're opening the Panel.
        {
            if (!vPanel.gameObject.activeInHierarchy)
            {
                vPanel.gameObject.SetActive(true);
            }
        }

        else // If we're closing the Panel.
        {
            if (vPanel.gameObject.activeInHierarchy)
            {
                vPanel.gameObject.SetActive(false);
            }
        }
    }

    // Get Input Field string.
    public void GetRoomName(string vRoomName)
    {

    }

    public void GetPlayerName(string vPlayerName)
    {

    }

    #endregion
}