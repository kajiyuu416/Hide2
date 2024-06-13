using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private const string gamepad = "Gamepad";
    private const string keyboard_mouse = "Keyboard&Mouse";
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        Operation_identification();
    }

    //�L�[�{�[�h�}�E�X����A�Q�[���p�b�g����̔���
    private void Operation_identification()
    {
        if(playerController.Duplicate_PlayerInput.currentControlScheme.ToString() == gamepad)
        {

        }
        if(playerController.Duplicate_PlayerInput.currentControlScheme.ToString() == keyboard_mouse)
        {

        }
    }
}
