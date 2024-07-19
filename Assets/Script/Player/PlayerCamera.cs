using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Unity.VisualScripting.Dependencies.NCalc;

public class PlayerCamera : Photon.Pun.MonoBehaviourPun
{
    public Transform target;
    public PlayerController playerController;
    private float distanceToPlayerM = 6.0f;    // �J�����ƃv���C���[�Ƃ̋���[m]
    private float heightM = 1.3f;            // �����_�̍���[m]          // �����_�̍���[m]
    private float SlideDistanceM = 0.0f;       // �J���������ɃX���C�h������G�v���X�̎��E�ցC�}�C�i�X�̎�����[m]
    private static float RotationSensitivity = 300.0f;// �������x
    private float lockOnRotationSensitivity = RotationSensitivity / 2;
    private float lockOffRotationSensitivity = RotationSensitivity;
    private const float min_distanceToPlayerM = 5.0f;
    private const float max_distanceToPlayerM = 7.0f;
    private const float max_slidedistanceM = 1.0f;
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
    private void Update()
    {
        if(!photonView.IsMine || playerController == null || target == null)
            return;

        var changelock_GP = Gamepad.current.leftShoulder;

        if(changelock_GP.wasPressedThisFrame || Input.GetMouseButtonDown(1))
        {
            RotationSensitivity = lockOnRotationSensitivity;
            distanceToPlayerM = min_distanceToPlayerM;
            SlideDistanceM = max_slidedistanceM;
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
            if(transform.forward.y < lockOn_upper_limit && rotY < 0|| transform.forward.y > lockOn_lower_limit && rotY > 0)
            {
                Init(ref rotY);
            }

            transform.RotateAround(lookAt, Vector3.up, rotX);
            transform.RotateAround(lookAt, -transform.right, rotY);
        }
        else if(!playerController.Duplicate_lockOnMode)
        {
            if(transform.forward.y > upper_limit && rotY < 0|| transform.forward.y < lower_limit && rotY > 0)
            {
                Init(ref rotY);
            }
            transform.RotateAround(lookAt, Vector3.up, rotX);
            transform.RotateAround(lookAt, transform.right, rotY);
        }

        transform.position = lookAt - transform.forward * distanceToPlayerM;
        transform.position = transform.position + transform.right * SlideDistanceM;
    }
    private void Init(ref float val)
    {
        val = 0;
    }
}
