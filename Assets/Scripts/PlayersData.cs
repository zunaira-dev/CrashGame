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
        UIManager.Instance.hitEnterButton.SetActive(false);
    }
    public void SelectAmount() {
        float.TryParse(UIManager.Instance.solAmount.text, out value);
        // value = UIManager.Instance.setAmount.value;
        UIManager.Instance.displayAmount.text = value.ToString();
    }
    public void SetMin() {
        value = 0.1f;
        UIManager.Instance.displayAmount.text = value.ToString();
    }
    public void SetMax() {
        value =5f;
        UIManager.Instance.displayAmount.text = value.ToString();
    }
    public void DoubleAmount() {
        value += value;
        UIManager.Instance.displayAmount.text = value.ToString();
    }
    public void HalfAmount() {
        value /= 2;
        UIManager.Instance.displayAmount.text = value.ToString();
    }
    [PunRPC]
    public void SpawnPlayer(string name,string values) {
        GameObject go=  Instantiate(UIManager.Instance.playerInstance, UIManager.Instance._content.transform);
        go.transform.GetChild(0).GetComponent<Text>().text =name;
        go.transform.GetChild(1).GetComponent<Text>().text = values;
        go.transform.GetChild(2).GetComponent<Text>().text = "-";
    }
}
