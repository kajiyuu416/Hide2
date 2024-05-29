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
        loadingText.text = "ルームへ参加中...";

        //ホストと同じシーンを読み込み
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //ロビー接続時に呼ばれる関数
    //ルームリストの初期化
    public override void OnJoinedLobby()
    {
        LobbyMenu();
        roomsList.Clear();
        //参加者の名前入力
        CheckName();
    }
    public void OpenCreateRoomPanel()
    {
        CloseUI();
        createRoomPalel.SetActive(true);
    }
    //todo::ルーム名が空の状態でも、ルームが作成できてしまう為、空の状態では作成できないようにする
    //textの状態ではからの状態で作成できてしまうため、TmpInputFieldへ変更することで

    public void CreateloomButton()
    {
        //  ルーム製作   
        if(!string.IsNullOrEmpty(enterRoomName.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(enterRoomName.text, options);
            CloseUI();
            Debug.Log(enterRoomName.text + "");
            loadingText.text = "ルーム作成中...";
            loadingPanel.SetActive(true);
        }
    }

    //ルーム参加時に呼ばれる関数
    //ルームにいるプレイヤーの情報取得
    public override void OnJoinedRoom()
    {
        CloseUI();
        roomPanel.SetActive(true);
        RoomName.text = PhotonNetwork.CurrentRoom.Name;

        GetAllPlayer();

        CheckRoomHost();
    }

    //ルーム退出の関数
    public void LeavRoom()
    {
        PhotonNetwork.LeaveRoom();

        CloseUI();
        loadingText.text = "退出中...";
        loadingPanel.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        LobbyMenu();
    }
    //エラー処理
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseUI();
        errorText.text = "ルームの作成に失敗しました" + message;
        erroePanel.SetActive(true);
    }

    //ルーム検索
    public void FindRoom()
    {
        CloseUI();
        roomListPanel.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomUIinitialization();
        //リストに追加
        UpdateRoomList(roomList);
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];

            //満室の場合
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
    //ルームボタンの生成
    //生成したボタンにルーム情報を登録

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

    //ルームUIの重複防止
    public void RoomUIinitialization()
    {
        foreach(Room room in allRoomButtons)
        {
            Destroy(room.gameObject);
        }
        allRoomButtons.Clear();
    }
    //ルームへの参加
    //引数のルームへ参加
    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        CloseUI();
        loadingText.text = "ルームへ参加中";
        loadingPanel.SetActive(true);
    }

    //ルームにいるプレイヤーの情報を取得
    public void GetAllPlayer()
    {
        initializePlayerList();

        PlayerDisplay();
    }

    //プレイヤー名の重複防止
    public void initializePlayerList()
    {
        foreach(var room in allPlayerNames)
        {
            Destroy(room.gameObject);
        }
        allPlayerNames.Clear();
    }
    //プレイヤー表示
    //ルームに参加している人数分のUIを生成
    public void PlayerDisplay()
    {
        foreach (var players in PhotonNetwork.PlayerList)
        {

            playerTextGeneration(players);
        }
    }
    //UI生成の関数
    //プレイヤーネームの更新、リストへの追加
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
                Debug.Log("名前はすでに決まっています。");
            }
        }
    }

    //名前の登録,保存
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
    //プレイヤーが入室した時に呼ばれる関数
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerTextGeneration(newPlayer);
    }
    //プレイヤーが退室した時に呼ばれる関数(プレイヤー情報の更新)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetAllPlayer();
    }
    //ホストのみゲームが開始できる表ボタンの表示
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
    //ホストが切り替わった時
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }
    //ゲーム開始時呼ばれる関数
    public void PlayeGame()
    {
        PhotonNetwork.LoadLevel(PlayScene);
    }
}
