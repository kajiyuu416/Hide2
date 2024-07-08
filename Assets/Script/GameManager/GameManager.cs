using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager :MonoBehaviour
{
    public Image cursor;
    public PlayerController playerController;
    public Material[] skyboxs;
    private const string gamepad = "Gamepad";
    private const string keyboard_mouse = "Keyboard&Mouse";
    private Gamepad gamepad_connection;
    private Keyboard keyboard_connection;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void change_sky(int i)
    {
        RenderSettings.skybox = skyboxs[i];
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
