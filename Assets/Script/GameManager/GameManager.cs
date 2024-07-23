using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager :MonoBehaviour
{
    [SerializeField] public Image cursor;
    [SerializeField] public PlayerController playerController;
    [SerializeField] Material[] Stage_skyboxs;
    [SerializeField] Material daySkybox;    // 昼用SkyBoxマテリアル
    [SerializeField] Material eveningSkybox; // 夕方用SkyBoxマテリアル
    [SerializeField] Material nightSkybox;   // 夜用SkyBoxマテリアル
    private const string gamepad = "Gamepad";
    private const string keyboard_mouse = "Keyboard&Mouse";
    private Gamepad gamepad_connection;
    private Keyboard keyboard_connection;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        change_skyBox();
    }

    private void Update()
    {
       Operation_identification();
    }

    //キーボードマウス操作、ゲームパット操作の判定
    private void Operation_identification()
    {
        gamepad_connection = Gamepad.current;
        keyboard_connection = Keyboard.current;

        if(playerController != null)
        {
            if(playerController.Duplicate_PlayerInput.currentControlScheme.ToString() == gamepad)
            {
                Debug.Log(playerController.Duplicate_PlayerInput.currentControlScheme.ToString());
            }
            else if(playerController.Duplicate_PlayerInput.currentControlScheme.ToString() == keyboard_mouse)
            {
                Debug.Log(playerController.Duplicate_PlayerInput.currentControlScheme.ToString());
            }
        }
    }
    public void change_sky(int i)
    {
        RenderSettings.skybox = Stage_skyboxs[i];
    }
    public void change_skyBox()
    {
        float hour = System.DateTime.Now.Hour;

        if(hour >= 6 && hour < 18)
        {
            RenderSettings.skybox = daySkybox;
        }
        else if(hour >= 18 && hour < 21)
        {
            RenderSettings.skybox = eveningSkybox;
        }
        else
        {
            RenderSettings.skybox = nightSkybox;
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
