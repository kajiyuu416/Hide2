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
    [SerializeField] GameObject erroePanel;
    [SerializeField] GameObject Buttons;
    [SerializeField] GameObject roomListPanel;
    [SerializeField] GameObject roomButtonContent;
    [SerializeField] GameObject playerNameContent;
    [SerializeField] GameObject nameInputPanel;
    [SerializeField] GameObject startButton;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] TextMeshProUGUI RoomName;
    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI placeholderText;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField enterRoomName;
    [SerializeField] Room originalRoomButton;
    [SerializeField] string PlayScene;

    private Dictionary<string,RoomInfo> roomsList = new Dictionary<string, RoomInfo>();
    private List<Room> allRoomButtons = new List<Room>();
    private List<TextMeshProUGUI> allPlayerNames = new List<TextMeshProUGUI>();
    private bool setName;

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
        erroePanel.SetActive(false);
        roomListPanel.SetActive(false);
        nameInputPanel.SetActive(false);
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

        //�z�X�g�Ɠ����V�[����ǂݍ���
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //���r�[�ڑ����ɌĂ΂��֐�
    //���[�����X�g�̏�����
    public override void OnJoinedLobby()
    {
        LobbyMenu();
        roomsList.Clear();
        //�Q���҂̖��O����
        CheckName();
    }
    public void OpenCreateRoomPanel()
    {
        CloseUI();
        createRoomPalel.SetActive(true);
    }
    //todo::���[��������̏�Ԃł��A���[�����쐬�ł��Ă��܂��ׁA��̏�Ԃł͍쐬�ł��Ȃ��悤�ɂ���
    //text�̏�Ԃł͂���̏�Ԃō쐬�ł��Ă��܂����߁ATmpInputField�֕ύX���邱�Ƃ�

    public void CreateloomButton()
    {
        //  ���[������   
        if(!string.IsNullOrEmpty(enterRoomName.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(enterRoomName.text, options);
            CloseUI();
            Debug.Log(enterRoomName.text + "");
            loadingText.text = "���[���쐬��...";
            loadingPanel.SetActive(true);
        }
    }

    //���[���Q�����ɌĂ΂��֐�
    //���[���ɂ���v���C���[�̏��擾
    public override void OnJoinedRoom()
    {
        CloseUI();
        roomPanel.SetActive(true);
        RoomName.text = PhotonNetwork.CurrentRoom.Name;

        GetAllPlayer();

        CheckRoomHost();
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
    //�G���[����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseUI();
        errorText.text = "���[���̍쐬�Ɏ��s���܂���" + message;
        erroePanel.SetActive(true);
    }

    //���[������
    public void FindRoom()
    {
        CloseUI();
        roomListPanel.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomUIinitialization();
        //���X�g�ɒǉ�
        UpdateRoomList(roomList);
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];

            //�����̏ꍇ
            if(info.RemovedFromList)
            {
                roomsList.Remove(info.Name);
            }
            else
            {
                roomsList[info.Name] = info;
            }
        }
        RoomListDisp(roomsList);
    }
    //���[���{�^���̐���
    //���������{�^���Ƀ��[������o�^

    public void RoomListDisp(Dictionary<string,RoomInfo> cachedRoomList)
    {
        foreach (var roomInfo in cachedRoomList)
        {
            Room newButton = Instantiate(originalRoomButton);

            newButton.RegisterRoomDetails(roomInfo.Value);

            newButton.transform.SetParent(roomButtonContent.transform);

            allRoomButtons.Add(newButton);
        }
    }

    //���[��UI�̏d���h�~
    public void RoomUIinitialization()
    {
        foreach(Room room in allRoomButtons)
        {
            Destroy(room.gameObject);
        }
        allRoomButtons.Clear();
    }
    //���[���ւ̎Q��
    //�����̃��[���֎Q��
    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        CloseUI();
        loadingText.text = "���[���֎Q����";
        loadingPanel.SetActive(true);
    }

    //���[���ɂ���v���C���[�̏����擾
    public void GetAllPlayer()
    {
        initializePlayerList();

        PlayerDisplay();
    }

    //�v���C���[���̏d���h�~
    public void initializePlayerList()
    {
        foreach(var room in allPlayerNames)
        {
            Destroy(room.gameObject);
        }
        allPlayerNames.Clear();
    }
    //�v���C���[�\��
    //���[���ɎQ�����Ă���l������UI�𐶐�
    public void PlayerDisplay()
    {
        foreach (var players in PhotonNetwork.PlayerList)
        {

            playerTextGeneration(players);
        }
    }
    //UI�����̊֐�
    //�v���C���[�l�[���̍X�V�A���X�g�ւ̒ǉ�
    public void playerTextGeneration(Player players)
    {
        TextMeshProUGUI newPlayerText = Instantiate(playerNameText);
        newPlayerText.text = players.NickName;
        newPlayerText.transform.SetParent(playerNameContent.transform);
        allPlayerNames.Add(newPlayerText);
    }

    public void CheckName()
    {
        if(!setName)
        {
            CloseUI();
            nameInputPanel.SetActive(true);
            if(PlayerPrefs.HasKey("playerName"))
            {
                placeholderText.text = PlayerPrefs.GetString("playerName");
                nameInput.text = PlayerPrefs.GetString("playerName");
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("playerName");
                Debug.Log("���O�͂��łɌ��܂��Ă��܂��B");
            }
        }
    }

    //���O�̓o�^,�ۑ�
    public void SetName()
    {
        if(!string.IsNullOrEmpty(nameInput.text))
        {

            if(nameInput.text.Length > 10)
            {
                nameInput.text = nameInput.text[..10];
            }
            PhotonNetwork.NickName = nameInput.text;
            PlayerPrefs.SetString("playerName", nameInput.text);
            LobbyMenu();
            setName = true;
        }
    }
    //�v���C���[�������������ɌĂ΂��֐�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerTextGeneration(newPlayer);
    }
    //�v���C���[���ގ��������ɌĂ΂��֐�(�v���C���[���̍X�V)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetAllPlayer();
    }
    //�z�X�g�̂݃Q�[�����J�n�ł���\�{�^���̕\��
    public void CheckRoomHost()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }
    //�z�X�g���؂�ւ������
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }
    //�Q�[���J�n���Ă΂��֐�
    public void PlayeGame()
    {
        PhotonNetwork.LoadLevel(PlayScene);
    }
}
