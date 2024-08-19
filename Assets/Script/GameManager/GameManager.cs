using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public Image cursor;
    [SerializeField] public PlayerController playerController;
    [SerializeField] Material morningSkybox;    // ���pSkyBox�}�e���A��
    [SerializeField] Material daySkybox;    // ���pSkyBox�}�e���A��
    [SerializeField] Material eveningSkybox; // �[���pSkyBox�}�e���A��
    [SerializeField] Material nightSkybox;   // ��pSkyBox�}�e���A��
    private const string gamepad = "Gamepad";
    private const string keyboard_mouse = "Keyboard&Mouse";
    private bool operation_gamepad;
    private bool operation_keyboard_mouse;
    private Gamepad gamepad_connection;
    private Keyboard keyboard_connection;
    private Light light;
    private Color light_harf_color;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        light = GetComponentInChildren<Light>();
        light_harf_color = new Color(light.color.r * 0.5f, light.color.g * 0.5f, light.color.b * 0.5f, light.color.a);
        change_skyBox();
    }

    private void Update()
    {
       Operation_identification();
    }

    //�L�[�{�[�h�}�E�X����A�Q�[���p�b�g����̔���
    private void Operation_identification()
    {
        gamepad_connection = Gamepad.current;
        keyboard_connection = Keyboard.current;

        if(playerController != null)
        {
            if(playerController.Duplicate_PlayerInput.currentControlScheme.ToString() == gamepad)
            {
                operation_gamepad = true;
                operation_keyboard_mouse = false;
            }
            else if(playerController.Duplicate_PlayerInput.currentControlScheme.ToString() == keyboard_mouse)
            {
                operation_gamepad = false;
                operation_keyboard_mouse = true;
            }
        }
    }
    //�����ɉ�����skybox�̕ύX��Light�̒l��ύX����
    //�ύX�̊��o 4���`8���A8���`16���A16���`20���A20���`4��
    public void change_skyBox()
    {
        float hour = System.DateTime.Now.Hour;
        if(hour >= 4 && hour < 8)
        {
            SetSkybox(morningSkybox);
            Color modifiedColor = light_harf_color;
            light.color = modifiedColor;
            AudioManager.Instance.MorningBGM();
        }
        else if(hour >= 8 && hour < 16)
        {
            SetSkybox(daySkybox);
            light.color = Color.white;
            AudioManager.Instance.DayBGM();
        }
        else if(hour >= 16 && hour < 20)
        {
            SetSkybox(eveningSkybox);
            Color modifiedColor = light_harf_color;
            light.color = modifiedColor;
            AudioManager.Instance.Eveningbgm();
        }
        else 
        {
            SetSkybox(nightSkybox);
            light.color = Color.black;
            AudioManager.Instance.NightBGM();
        }
    }
    private void SetSkybox(Material newSkybox)
    {
        RenderSettings.skybox = newSkybox;
        DynamicGI.UpdateEnvironment();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player���V���ɓ������܂����B");
        AudioManager.Instance.EnteringroomSE();
    }
    //�v���C���[���ގ��������ɌĂ΂��֐�(�v���C���[���̍X�V)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AudioManager.Instance.LeaveroomSE();
        playerController.leave();
        Debug.Log("Player���ގ����܂����B");
    }

    public bool Duplicate_operation_gamepad
    {
        get
        {
            return operation_gamepad;
        }
    }
    public bool Duplicate_operation_keyboard_mouse
    {
        get
        {
            return operation_keyboard_mouse;
        }
    }

    public Gamepad Duplicate_gamepad_connection
    {
        get
        {
            return gamepad_connection;
        }
    }

    public Keyboard Duplicate_keyboard_connection
    {
        get
        {
            return keyboard_connection;
        }
    }
        

}
