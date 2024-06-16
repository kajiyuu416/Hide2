using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerCamera : Photon.Pun.MonoBehaviourPun
{
    public Transform target;
    public PlayerController playerController;
    private float distanceToPlayerM = 6.0f;    // カメラとプレイヤーとの距離[m]
    private float heightM = 1.3f;            // 注視点の高さ[m]          // 注視点の高さ[m]
    private float SlideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    private float RotationSensitivity = 300.0f;// 初期感度
    private const float min_distanceToPlayerM = 4.0f;
    private const float max_distanceToPlayerM = 6.0f;
    private const float max_slidedistanceM = 1.0f;
    private const float lockOnRotationSensitivity = 150.0f;
    private const float lockOffRotationSensitivity = 300.0f;
    private Camera camera;
    private void Start()
    {
        camera = GetComponent<Camera>();
        if(camera.GetComponent<PhotonView>().IsMine)
        {
            camera.targetDisplay = 0;
        }
        else
        {
            camera.targetDisplay = 1;
        }
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if(!photonView.IsMine || playerController == null || target == null)
            return;
        var rotX = playerController.Duplicate_rightStickVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = playerController.Duplicate_rightStickVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = target.transform.position + Vector3.up * heightM;
        float upper_limit = 0.1f;
        float lower_limit = -0.8f;
        float lockOn_upper_limit = -0.6f;
        float lockOn_lower_limit = 0.5f;

        if(playerController.Duplicate_lockOnMode)
        {
            if(transform.forward.y < lockOn_upper_limit && rotY < 0)
            {
                rotY = 0;
            }

            if(transform.forward.y > lockOn_lower_limit && rotY > 0)
            {
                rotY = 0;
            }
            transform.RotateAround(lookAt, Vector3.up, rotX);
            transform.RotateAround(lookAt, -transform.right, rotY);
        }
        else if(!playerController.Duplicate_lockOnMode)
        {
            if(transform.forward.y > upper_limit && rotY < 0)
            {
                rotY = 0;
            }

            if(transform.forward.y < lower_limit && rotY > 0)
            {
                rotY = 0;
            }
            transform.RotateAround(lookAt, Vector3.up, rotX);
            transform.RotateAround(lookAt, transform.right, rotY);
        }

        transform.position = lookAt - transform.forward * distanceToPlayerM;
        transform.position = transform.position + transform.right * SlideDistanceM;

        var current_gamepad = Gamepad.current;
        var changelock_GP = current_gamepad.leftShoulder;
        if(changelock_GP.wasPressedThisFrame || Input.GetMouseButton(1))
        {
            distanceToPlayerM = min_distanceToPlayerM;
            SlideDistanceM = max_slidedistanceM;
            RotationSensitivity = lockOnRotationSensitivity;
            playerController.Duplicate_lockOnMode = true;
        }
        else if(changelock_GP.wasReleasedThisFrame || Input.GetMouseButtonUp(1))
        {
            playerController.Duplicate_lockOnMode = false;
            distanceToPlayerM = max_distanceToPlayerM;
            SlideDistanceM = 0f;
            RotationSensitivity = lockOffRotationSensitivity;
        }
    }
}
