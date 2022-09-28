using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class UIManager :Singleton<UIManager>
{
    public InputField Name;
    public Transform _content;
    public Text
          multipllyerText,
          timeText,
          crashText,
          displayAmount,
          DisplayInfo;
    public GameObject 
        TestConnectScreen,
        LoginScreen,
        GraphScreen,
        MultiplyerScreen,
        TimeScreen,
        crashPanel,
        playerInstance,
        hitEnterButton
        ;
    public Slider 
        setAmount,
        fillImage;

    public void ActiveTestConnectScreen() {
        TestConnectScreen.SetActive(true);
    }
    public void DeactiveTestConnectScreen() {
        TestConnectScreen.SetActive(false);
    }
    public void ActiveLoginScreen() {
        LoginScreen.SetActive(true);
    }
    public void ActiveGraphScreen() {
        GraphScreen.SetActive(true);
    }
    public void DeactiveLoginScreen() {
        LoginScreen.SetActive(false);
    }
}
