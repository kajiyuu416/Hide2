using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class GameOption : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject camera_settingPanel;
    [SerializeField] private GameObject volume_settingPanel;
    [SerializeField] private GameObject volume_leveling_Button;
    [SerializeField] private GameObject camera_Left_and_Right_Flip_Button;
    [SerializeField] private GameObject camera_Upside_Down_Flip_Button;
    [SerializeField] private GameObject bgmSlider;
    [SerializeField] private Slider gamepad_sensitivitySlinder;
    [SerializeField] private Slider mouse_sensitivitySlinder;
    private const string titleScene = "Title";
    private bool openOption;
    private bool camera_Left_and_Right_Flip = false;//左右反転
    private bool camera_Upside_Down_Flip = false;//上下反転
    private void Update()
    {
        if(gameManager.Duplicate_operation_gamepad)
        {
            SetRotationSensitivity_gamepad(gamepad_sensitivitySlinder.value);
            gamepad_sensitivitySlinder.onValueChanged.AddListener(SetRotationSensitivity_gamepad);
        }
        else if(gameManager.Duplicate_operation_keyboard_mouse)
        {
            SetRotationSensitivity_mouse(mouse_sensitivitySlinder.value);
            mouse_sensitivitySlinder.onValueChanged.AddListener(SetRotationSensitivity_mouse);
        }

        var changeGP = gameManager.Duplicate_gamepad_connection.startButton;
        if(changeGP.wasPressedThisFrame|| Input.GetKeyDown(KeyCode.Escape))
        {
            if(!openOption)
            {
                OpenOptionPanel();
                optionUI.SetActive(false);
                AudioManager.Instance.OpenOptionSE();
            }
            else
            {
                CloseUI();
                optionUI.SetActive(true);
                AudioManager.Instance.CloseOptionSE();
            }
        }

        if(openOption)
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if(optionPanel.activeSelf)
            {
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

            if(camera_settingPanel.activeSelf)
            {
                if(gameManager.Duplicate_operation_gamepad)
                {
                    if(selectedObject == null)
                    {
                        EventSystem.current.SetSelectedGameObject(camera_Upside_Down_Flip_Button);
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
            if(volume_settingPanel.activeSelf)
            {
                if(gameManager.Duplicate_operation_gamepad)
                {
                    if(selectedObject == null)
                    {
                        EventSystem.current.SetSelectedGameObject(bgmSlider);
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
    }
    public void SetRotationSensitivity_gamepad(float volume)
    {
        PlayerCamera.rotationSensitivity = gamepad_sensitivitySlinder.value * 500.0f;
    }
    public void SetRotationSensitivity_mouse(float volume)
    {
        PlayerCamera.rotationSensitivity = mouse_sensitivitySlinder.value * 250.0f;
    }
    public void OpenOptionPanel()
    {
        CloseUI();
        optionPanel.SetActive(true);
        openOption = true;
        AudioManager.Instance.OpenOptionSE();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void OpenCameraSettingPanel()
    {
        CloseUI();
        optionPanel.SetActive(false);
        camera_settingPanel.SetActive(true);
        openOption = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        AudioManager.Instance.OpenOptionSE();
    }
    public void OpenVolumeSettingPanel()
    {
        CloseUI();
        optionPanel.SetActive(false);
        volume_settingPanel.SetActive(true);
        openOption = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        AudioManager.Instance.OpenOptionSE();
    }
    public void Push_Button_Camera_Left_Right_Change()
    {
        camera_Left_and_Right_Flip = !camera_Left_and_Right_Flip;
        TextMeshProUGUI buttonText = camera_Left_and_Right_Flip_Button.GetComponentInChildren<TextMeshProUGUI>();
        AudioManager.Instance.SelectSE();
        if(camera_Left_and_Right_Flip == true)
        {
            buttonText.text = "ON";
            buttonText.color = Color.red;
        }
        else
        {
            buttonText.text = "OFF";
            buttonText.color = Color.blue;
        }
    }

    public void Push_Button_Camera_Up_Down_Change()
    {
        camera_Upside_Down_Flip = !camera_Upside_Down_Flip;
        TextMeshProUGUI buttonText = camera_Upside_Down_Flip_Button.GetComponentInChildren<TextMeshProUGUI>();
        AudioManager.Instance.SelectSE();
        if(camera_Upside_Down_Flip == true)
        {
            buttonText.text = "ON";
            buttonText.color = Color.red;
        }
        else
        {
            buttonText.text = "OFF";
            buttonText.color = Color.blue;
        }
    }

    private void CloseUI()
    {
        optionPanel.SetActive(false);
        camera_settingPanel.SetActive(false);
        volume_settingPanel.SetActive(false);
        openOption = false;
        EventSystem.current.SetSelectedGameObject(null);
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
        PhotonNetwork.Instantiate("Smoke", playerObject.transform.position, playerObject.transform.rotation);
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
    public bool Duplicate_camera_Left_and_Right_Flip
    {
        get
        {
            return camera_Left_and_Right_Flip;
        }
    }
    public bool Duplicate_camera_Upside_Down_Flip
    {
        get
        {
            return camera_Upside_Down_Flip;
        }
    }
}
