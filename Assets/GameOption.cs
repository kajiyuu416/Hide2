using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOption : MonoBehaviour
{
    [SerializeField] GameObject OptionPanel;
    private GameManager gameManager;
    private bool openOption;
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
    public bool Duplicate_openOption
    {
        get
        {
            return openOption;
        }
    }
}
