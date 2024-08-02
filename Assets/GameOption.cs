using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class GameOption : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private GameObject volume_leveling_Button;
    private const string titleScene = "Title";
    private bool openOption;
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
        playerObject.GetComponent<PlayerController>().ResetPos();
        CloseUI();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Playerが新たに入室しました。");
    }
    //プレイヤーが退室した時に呼ばれる関数(プレイヤー情報の更新)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Playerが退室しました。");
    }
    public bool Duplicate_openOption
    {
        get
        {
            return openOption;
        }
    }
}
