using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
public class TestConnect : MonoBehaviourPunCallbacks {

    public ServerSettings serverSettings;
    private void Start() {
        if (!PhotonNetwork.IsConnected) {
           UIManager.Instance.DisplayInfo.text = "Wait! Connecting to Server";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1.0";
            UIManager.Instance.DisplayInfo.text = serverSettings.DevRegion;
            PhotonNetwork.NickName = "zunaira";
            PhotonNetwork.ConnectUsingSettings();
        }
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            UIManager.Instance.DisplayInfo.text = "Error. Check internet connection!";
           // RefreshUI.SetActive(true);
        }
        if (PhotonNetwork.InLobby) {
          //  createRoom.OnClick_CreateRoom();
        }
    }
    public void RefreshButton() {
        Start();
    }
    public override void OnConnectedToMaster() {
        if (!PhotonNetwork.InLobby) {
            UIManager.Instance.DisplayInfo.text = "Joining Lobby........ ";
            PhotonNetwork.ConnectToRegion("us;");
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby() {
        UIManager.Instance.DisplayInfo.text = "Connected to Server ";
        OnClick_CreateRoom();
    }
    public override void OnJoinedRoom() {
        UIManager.Instance.DeactiveTestConnectScreen();
        UIManager.Instance.ActiveGraphScreen();
    }
    public override void OnCreatedRoom() {
        Debug.Log("On created Room successfuly");
    }
    public void OnClick_CreateRoom() {
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions options = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("Room", options, TypedLobby.Default);
    }
    public override void OnDisconnected(DisconnectCause cause) {
    }

}
