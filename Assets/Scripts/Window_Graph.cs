
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using Photon.Pun;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    public RectTransform graphContainer;
    //public Text tst;
    private PhotonView photonView;
    private List<float> valueList;
    float crashValue;
    GameObject lastCircleGameObject;
    private void Awake() {
        photonView = GetComponent<PhotonView>();
        valueList = new List<float>();
        if (PhotonNetwork.IsMasterClient) {
            GenerategraphValue();
        }
    }
    #region Graph
    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }
    IEnumerator ShowGraph(List<float> valueList) {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 7f;
        float xSize = 50f;
        //lastCircleGameObject = null;
        photonView.RPC("On", RpcTarget.AllBuffered, 0.0f, 0.0f);
        int i = 0;
        while (i < valueList.Count) {
            //for (int i = 0; i < valueList.Count; i++) {
            if (i + 1 < valueList.Count) UIManager.Instance.multipllyerText.text = System.Math.Round(valueList[i], 2) + "X";
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            photonView.RPC("On", RpcTarget.AllBuffered, xPosition, yPosition);
            //GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            //if (lastCircleGameObject != null) {
            //    CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            //}
            //lastCircleGameObject = circleGameObject;
            i++;
            yield return new WaitForSeconds(1f);
        }
        i--;
        if (i > 1) { i -= 1; }
        yield return new WaitForSeconds(1f);
        Debug.Log(i + " c " + valueList[i]);
        photonView.RPC("CrashShow", RpcTarget.AllBuffered, valueList[i]);
        //CrashShow(i);
        yield return new WaitForSeconds(3f);
        photonView.RPC("StartTimer", RpcTarget.AllBuffered);
        //StartTimer();
        Timer();
    }
    [PunRPC]
    public void On(float x, float y) {
        GameObject circleGameObject = CreateCircle(new Vector2(x, y));
        Debug.Log(x);
          if (lastCircleGameObject != null) {
        CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
        }
        lastCircleGameObject = circleGameObject;
    }
    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
    public void GenerategraphValue() {
        valueList = new List<float>();
       // valueList.Add(0);
        int limit = Random.Range(0, 5);
        float num = 0;
        while (num <= limit) {
            float prev = num;
            num += 1f;
            num = Random.Range(prev, num);
           // Debug.Log(num);
            valueList.Add(num);
        }
        valueList.Add(0);
        StartCoroutine(ShowGraph(valueList));
    }
    [PunRPC]
    public void CrashShow(float i) {
        crashValue= (float)System.Math.Round(i, 2);
        UIManager.Instance.crashText.text = "Crash at " + crashValue;
        UIManager.Instance.crashPanel.SetActive(true);
        photonView.RPC("GetResults", RpcTarget.AllBuffered, crashValue);
    }
    [PunRPC]
    public void GetResults(float gameVal) {
        for (int i = 0; i < UIManager.Instance._content.transform.childCount; i++) {
            float playerVal = 0;
            string val = UIManager.Instance._content.transform.GetChild(i).GetChild(1).GetComponent<Text>().text;
            float.TryParse(val, out playerVal);
            if (gameVal >= playerVal) {
                UIManager.Instance._content.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = "Profit";
            } else {
                UIManager.Instance._content.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = "Loss";
            }
        } 
    }
    #endregion Graph
    #region Time
    public void Timer() {
        StartCoroutine(GetTime());
    }
    IEnumerator GetTime() {
        photonView.RPC("StartTimer", RpcTarget.AllBuffered);
        int timeValue = 10;
        while (timeValue >= 0) {
            yield return new WaitForSeconds(1);
            timeValue--;
            if (timeValue >= 0) {
                if (PhotonNetwork.IsMasterClient) {
                    photonView.RPC("SetValue", RpcTarget.AllBuffered, timeValue);
                }
            }
        }
        if (timeValue <= 0) {
            photonView.RPC("EndTimer", RpcTarget.AllBuffered);
        }
        if (PhotonNetwork.IsMasterClient) {
            GenerategraphValue();
        }
    }
    [PunRPC]
    public void SetValue(int v) {
        UIManager.Instance.fillImage.value = v;
        UIManager.Instance.timeText.text = "Time : " + v;
    }
    [PunRPC]
    public void EndGraph() {
        UIManager.Instance.TimeScreen.SetActive(false);
    }
    [PunRPC]
    public void StartTimer() {
        for (int i = 0; i < UIManager.Instance._content.childCount; i++) {
            Destroy(UIManager.Instance._content.transform.GetChild(i).gameObject);
        }
        lastCircleGameObject = null;
        UIManager.Instance.fillImage.value = 10;
        UIManager.Instance.crashPanel.SetActive(false);
        UIManager.Instance.hitEnterButton.SetActive(true);
        UIManager.Instance.MultiplyerScreen.SetActive(false);
        UIManager.Instance.TimeScreen.SetActive(true);
        for (int j =1; j < graphContainer.childCount; j++) {
            Destroy(graphContainer.GetChild(j).gameObject);
        }
    }
    [PunRPC]
    public void EndTimer() {
        UIManager.Instance.MultiplyerScreen.SetActive(true);
        UIManager.Instance.TimeScreen.SetActive(false);
        UIManager.Instance.hitEnterButton.SetActive(false);
       
       
    }
    #endregion Time
}
