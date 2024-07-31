using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class GameOption : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject OptionPanel;
    [SerializeField] GameObject volume_leveling_Button;
    private GameManager gameManager;
    private bool openOption;
    private string titleScene = "Title";
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        var changeGP = gameManager.Duplicate_gamepad_connection.startButton;
        if(changeGP.wasPressedThisFrame|| Input.GetKeyDown(KeyCode.Escape))
        {
            if(!openOption)
            {
                OpenOptionPanel();
            }
            else
            {
                CloseUI();
            }
        }

        if(openOption)
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if(gameManager.Duplicate_operation_gamepad)
            {
                if(selectedObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(volume_leveling_Button);
                }
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(gameManager.Duplicate_operation_keyboard_mouse)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
    public void OpenOptionPanel()
    {
        CloseUI();
        OptionPanel.SetActive(true);
        openOption = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void CloseUI()
    {
        OptionPanel.SetActive(false);
        openOption = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LeavRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(titleScene);
    }
    public void Resetposition()
    {
        PhotonView localPhotonView = PhotonNetwork.LocalPlayer.TagObject as PhotonView;
        GameObject playerObject = localPhotonView.gameObject;
        playerObject.transform.position = Vector3.zero;
        Debug.Log("playerObject");
        //PlayerController pc = playerObject.GetComponent<PlayerController>();
        //pc.ResetPos();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player���V���ɓ������܂����B");
    }
    //�v���C���[���ގ��������ɌĂ΂��֐�(�v���C���[���̍X�V)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player���ގ����܂����B");
    }
    public bool Duplicate_openOption
    {
        get
        {
            return openOption;
        }
    }
}