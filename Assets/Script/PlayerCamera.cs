using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    private PlayerController playerController;
    public float DistanceToPlayerM = 2.0f;    // カメラとプレイヤーとの距離[m]
    public float SlideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    public float HeightM = 1.2f;            // 注視点の高さ[m]          // 注視点の高さ[m]
    public float RotationSensitivity = 100.0f;// 感度
    Vector3 targetPos;
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
    // Update is called once per frame
    private void FixedUpdate()
    {
        var rotX = playerController.Duplicate_rightStickVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = playerController.Duplicate_rightStickVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = player.transform.position + Vector3.up * HeightM;
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
        transform.position = lookAt - transform.forward * DistanceToPlayerM;
        transform.position = transform.position + transform.right * SlideDistanceM;

        if (metamorphosisflag)
        {
            DistanceToPlayerM = 8.0f;
        }

        if(GameManager.Non_control)
        {
            rotX = 0;
            rotY = 0;
        }
    }
}
