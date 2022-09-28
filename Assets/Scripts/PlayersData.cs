using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersData : MonoBehaviour
{
    private PhotonView photonView;
    private float value;
    private void Start() {
        photonView = GetComponent<PhotonView>();
    }
    public void HitEnter() { 
        photonView.RPC("SpawnPlayer", RpcTarget.AllBuffered, PhotonNetwork.NickName, System.Math.Round(value, 2).ToString());
    }
    public void SelectAmount() {
        value = UIManager.Instance.setAmount.value;
        UIManager.Instance.displayAmount.text = UIManager.Instance.setAmount.value.ToString();
    }
    [PunRPC]
    public void SpawnPlayer(string name,string values) {
        GameObject go=  Instantiate(UIManager.Instance.playerInstance, UIManager.Instance._content.transform);
        go.transform.GetChild(0).GetComponent<Text>().text =name;
        go.transform.GetChild(1).GetComponent<Text>().text = values;
        go.transform.GetChild(2).GetComponent<Text>().text = "-";
    }
}
