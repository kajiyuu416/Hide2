using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] RayCastCS rayCastcs;
    private PlayerController playerController;
    private float distanceToPlayerM = 6.0f;    // カメラとプレイヤーとの距離[m]
    private float heightM = 1.3f;            // 注視点の高さ[m]          // 注視点の高さ[m]
    private float SlideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    private float RotationSensitivity = 300.0f;// 初期感度
    Vector3 targetPos;

    private const float min_distanceToPlayerM = 3.0f;
    private const float max_distanceToPlayerM = 6.0f;
    private const float max_slidedistanceM = 1.0f;
    private const float lockOnRotationSensitivity = 150.0f;
    private const float lockOffRotationSensitivity = 300.0f;
    private Color objMeshColor;
    private RayCastCS rayCastCS;
    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        targetPos = player.transform.position;
        rayCastCS = FindObjectOfType<RayCastCS>();
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        var current_gamepad = Gamepad.current;
        var current_keyboard = Keyboard.current;
        var changelock_GP = current_gamepad.leftShoulder;
        var changelock_KB = current_keyboard.shiftKey;
        if(changelock_GP.wasPressedThisFrame|| changelock_KB.wasPressedThisFrame)
        {
            distanceToPlayerM = min_distanceToPlayerM;
            SlideDistanceM = max_slidedistanceM;
            RotationSensitivity = lockOnRotationSensitivity;
            playerController.Duplicate_lockOnMode = true;
        }
        else if(changelock_GP.wasReleasedThisFrame || changelock_KB.wasReleasedThisFrame)
        {
            playerController.Duplicate_lockOnMode = false;
            distanceToPlayerM = max_distanceToPlayerM;
            SlideDistanceM = 0f;
            RotationSensitivity = lockOffRotationSensitivity;
        }
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        var rotX = playerController.Duplicate_rightStickVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = playerController.Duplicate_rightStickVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = player.transform.position + Vector3.up * heightM;
        float upper_limit = 0.2f;
        float lower_limit = -0.8f;
        bool metamorphosisflag = rayCastCS.metamorphosisflag;

        transform.RotateAround(lookAt, Vector3.up, rotX);

        if(transform.forward.y > upper_limit && rotY < 0)
        {
            rotY = 0;
        }

        if(transform.forward.y < lower_limit && rotY > 0)
        {
            rotY = 0;
        }
        transform.RotateAround(lookAt, transform.right, rotY);
        transform.position = lookAt - transform.forward * distanceToPlayerM;
        transform.position = transform.position + transform.right * SlideDistanceM;

    }
}
