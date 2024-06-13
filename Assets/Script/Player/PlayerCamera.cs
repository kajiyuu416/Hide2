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
    private float distanceToPlayerM = 6.0f;    // �J�����ƃv���C���[�Ƃ̋���[m]
    private float heightM = 1.3f;            // �����_�̍���[m]          // �����_�̍���[m]
    private float SlideDistanceM = 0.0f;       // �J���������ɃX���C�h������G�v���X�̎��E�ցC�}�C�i�X�̎�����[m]
    private float RotationSensitivity = 300.0f;// �������x
    Vector3 targetPos;

    private const float min_distanceToPlayerM = 4.0f;
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
            Debug.LogError("�^�[�Q�b�g���ݒ肳��Ă��Ȃ�");
            Application.Quit();
        }
        targetPos = player.transform.position;
        rayCastCS = FindObjectOfType<RayCastCS>();
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        var current_gamepad = Gamepad.current;
        var changelock_GP = current_gamepad.leftShoulder;
        if(changelock_GP.wasPressedThisFrame|| Input.GetMouseButton(1))
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
    // Update is called once per frame
    private void LateUpdate()
    {
        var rotX = playerController.Duplicate_rightStickVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = playerController.Duplicate_rightStickVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = player.transform.position + Vector3.up * heightM;
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

    }
}
