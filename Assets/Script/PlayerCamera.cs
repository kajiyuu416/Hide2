using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] PlayerController PC;
    [SerializeField] float DistanceToPlayerM = 2.0f;    // カメラとプレイヤーとの距離[m]
    [SerializeField] float SlideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    [SerializeField] float HeightM = 1.2f;            // 注視点の高さ[m]
    [SerializeField] float RotationSensitivity = 100.0f;// 感度
    [SerializeField] float UpperLimit = 0.3f;
    [SerializeField] float LowerLimit = -0.8f;
    Vector3 targetPos;
    private RayCastCS rayCastCS;
    private void Start()
    {
        if (Player == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        targetPos = Player.transform.position;
        rayCastCS = FindObjectOfType<RayCastCS>();
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += Player.transform.position - targetPos;
        targetPos = Player.transform.position;
        var rotX = PC.RightStickVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = PC.RightStickVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = Player.transform.position + Vector3.up * HeightM;
        bool metamorphosisflag = rayCastCS.metamorphosisflag;
        if(GameManager.Non_control)
        {
            rotX = 0;
            rotY = 0;
        }

        transform.RotateAround(lookAt, Vector3.up, rotX);
        if (transform.forward.y > UpperLimit && rotY < 0)
        {
            rotY = 0;
        }
        if (transform.forward.y < LowerLimit && rotY > 0)
        {
            rotY = 0;
        }
        transform.RotateAround(lookAt, -transform.right, rotY);
        transform.position = lookAt - transform.forward * DistanceToPlayerM;
        transform.LookAt(lookAt);
        transform.position = transform.position + transform.right * SlideDistanceM;

        if (metamorphosisflag)
        {
            DistanceToPlayerM = 8.0f;
        }

    }
}
