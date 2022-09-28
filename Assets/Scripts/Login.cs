using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("Name")) {
           UIManager.Instance. DeactiveLoginScreen();
           UIManager.Instance. ActiveTestConnectScreen();
        }
    }
    public void SetName() {
        PhotonNetwork.NickName = UIManager.Instance. Name.text;
        PlayerPrefs.SetString("Name", UIManager.Instance. Name.text);
        UIManager.Instance.DeactiveLoginScreen();
        UIManager.Instance.ActiveTestConnectScreen();
    }
}
