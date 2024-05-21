using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject createRoomPalel;
    [SerializeField] GameObject Buttons;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] TextMeshProUGUI enterRoomName;
    public static PhotonManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CloseUI();
        loadingPanel.SetActive(true);
        loadingText.text = "ネットワークに接続中...";
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //UI表示、非表示
    public void CloseUI()
    {
        loadingPanel.SetActive(false);
        Buttons.SetActive(false);
        createRoomPalel.SetActive(false);
    }
    public void LobbyMenu()
    {
        CloseUI();
        Buttons.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        loadingText.text = "ルームへ参加中...";
    }

    //ロビー接続時のコールバック
    public override void OnJoinedLobby()
    {
        LobbyMenu();
    }
    public void OpenCreateRoomPanel()
    {
        CloseUI();
        createRoomPalel.SetActive(true);
    }

    public void CreateloomButton()
    {
        if(!string.IsNullOrEmpty(enterRoomName.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 5;

            //  ルーム製作   
            PhotonNetwork.CreateRoom(enterRoomName.text, options);
            CloseUI();

            loadingText.text = "ルーム作成中...";
            loadingPanel.SetActive(true);
        }
    }
}
