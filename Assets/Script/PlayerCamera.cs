using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float DistanceToPlayerM = 2.0f;    // カメラとプレイヤーとの距離[m]
    [SerializeField] float SlideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    [SerializeField] float HeightM = 1.2f;            // 注視点の高さ[m]
    [SerializeField] float RotationSensitivity = 100.0f;// 感度
    [SerializeField] float UpperLimit = 0.3f;
    [SerializeField] float LowerLimit = -0.8f;
    [SerializeField] RayCastCS RC;

    GameObject targetObj;
    Vector3 targetPos;


    void Start()
    {
        if (Target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        targetObj = GameObject.Find("Player1");
        targetPos = targetObj.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;
        var rotX = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSensitivity;
        var rotY = Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSensitivity;
        var lookAt = Target.position + Vector3.up * HeightM;
        // 回転
        transform.RotateAround(lookAt, Vector3.up, rotX);
        // カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        if (transform.forward.y > UpperLimit && rotY < 0)
        {
            rotY = 0;
        }
        if (transform.forward.y < LowerLimit && rotY > 0)
        {
            rotY = 0;
        }
        transform.RotateAround(lookAt, transform.right, rotY);

        // カメラとプレイヤーとの間の距離を調整
        transform.position = lookAt - transform.forward * DistanceToPlayerM;

        // 注視点の設定
        transform.LookAt(lookAt);

        // カメラを横にずらして中央を開ける
        transform.position = transform.position + transform.right * SlideDistanceM;
        if (RC.metamorphosisFlag)
        {
            DistanceToPlayerM = 8.0f;
        }

    }
}
