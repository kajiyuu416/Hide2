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
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject Buttons;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] TextMeshProUGUI enterRoomName;
    [SerializeField] TextMeshProUGUI RoomName;
    
    public static PhotonManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CloseUI();
        loadingPanel.SetActive(true);
        loadingText.text = "�l�b�g���[�N�ɐڑ���...";
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //UI�\���A��\��
    public void CloseUI()
    {
        loadingPanel.SetActive(false);
        Buttons.SetActive(false);
        createRoomPalel.SetActive(false);
        roomPanel.SetActive(false);
    }
    public void LobbyMenu()
    {
        CloseUI();
        Buttons.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        loadingText.text = "���[���֎Q����...";
    }

    //���r�[�ڑ����ɌĂ΂��֐�
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

            //  ���[������   
            PhotonNetwork.CreateRoom(enterRoomName.text, options);
            CloseUI();

            loadingText.text = "���[���쐬��...";
            loadingPanel.SetActive(true);
        }
    }

    //���[���Q�����ɌĂ΂��֐�
    public override void OnJoinedRoom()
    {
        CloseUI();
        roomPanel.SetActive(true);
        RoomName.text = PhotonNetwork.CurrentRoom.Name;
    }

    //���[���ޏo�̊֐�
    public void LeavRoom()
    {
        PhotonNetwork.LeaveRoom();

        CloseUI();
        loadingText.text = "�ޏo��...";
        loadingPanel.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        LobbyMenu();
    }

}
