using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class GameManager : MonoBehaviour
{
    [SerializeField] Texture2D cursor;
    public static bool Non_control;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Non_control = false;
        Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    private void Update()
    {
        var current_GP = Gamepad.current;

        var onClick = current_GP.buttonEast;
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Non_control = false;
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Non_control = true;
        }
    }
}
