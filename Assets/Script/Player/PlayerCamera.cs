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
    [SerializeField] LayerMask obstructingLayer;
    [SerializeField] Shader transparentShader;
    private Color transparentColor = new Color(1, 1, 1, 0.5f); // 半透明の色
    private Shader originalShader;
    private Renderer[] renderers;
    private float distanceToPlayerM = 6.0f;    // カメラとプレイヤーとの距離[m]
    private float heightM = 1.3f;            // 注視点の高さ[m]          // 注視点の高さ[m]
    private float SlideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    private static float RotationSensitivity = 300.0f;// 初期感度
    private float lockOnRotationSensitivity = RotationSensitivity / 2;
    private float lockOffRotationSensitivity = RotationSensitivity;
    private const float min_distanceToPlayerM = 5.0f;
    private const float max_distanceToPlayerM = 7.0f;
    private const float max_slidedistanceM = 1.0f;
    private Camera camera;
    private GameOption gameOption;

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
        renderers = FindObjectsOfType<Renderer>();
        gameOption = FindObjectOfType<GameOption>();
    }
    private void Update()
    {
        if(!photonView.IsMine || playerController == null || target == null)
            return;
        if(!gameOption.Duplicate_openOption)
            lockOn();
    }


    // Update is called once per frame
    private void LateUpdate()
    {
        if(!photonView.IsMine || playerController == null || target == null)
            return;
        lockOnstatechange();
        TransmissionObject();
    }
    private void Init(ref float val)
    {
        val = 0;
    }
    private void TransmissionObject()
    {
        foreach(Renderer renderer in renderers)
        {
            //指定したレイヤーを持つオブジェクトの場合
            if(obstructingLayer == (obstructingLayer | (1 << renderer.gameObject.layer)))
            {
                Vector3 directionToPlayer = target.position - camera.transform.position;
                RaycastHit hit;
                var raydis = 7.0f;
                if(Physics.Raycast(camera.transform.position, directionToPlayer, out hit, raydis))
                {
                    if(hit.collider.gameObject == renderer.gameObject)
                    {
                        // 元のシェーダーを保存
                        if(originalShader == null)
                        {
                            originalShader = renderer.material.shader;
                        }
                        // 透明シェーダーに変更
                        renderer.material.shader = transparentShader;
                        renderer.material.SetColor("_Color", transparentColor);
                    }
                    else
                    {
                        // 元のシェーダーに戻す
                        if(originalShader != null)
                        {
                            renderer.material.shader = originalShader;
                        }
                    }

                    Debug.DrawRay(camera.transform.position, directionToPlayer, Color.blue);
                }
            }
        }
    }

    private void lockOn()
    {

        var changelock_GP = Gamepad.current.leftShoulder;
        if(changelock_GP.isPressed || Input.GetMouseButton(1))
        {
            RotationSensitivity = lockOnRotationSensitivity;
            distanceToPlayerM = min_distanceToPlayerM;
            SlideDistanceM = max_slidedistanceM;
            playerController.Duplicate_lockOnMode = true;
        }
        else
        {
            playerController.Duplicate_lockOnMode = false;
            distanceToPlayerM = max_distanceToPlayerM;
            SlideDistanceM = 0f;
            RotationSensitivity = lockOffRotationSensitivity;
        }
    }


    private void lockOnstatechange()
    {
        var rotX = playerController.Duplicate_rightStickVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = playerController.Duplicate_rightStickVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = target.transform.position + Vector3.up * heightM;
        float upper_limit = 0.1f;
        float lower_limit = -0.8f;
        float lockOn_upper_limit = -0.6f;
        float lockOn_lower_limit = 0.5f;

        if(!gameOption.Duplicate_openOption)
        {
            if(playerController.Duplicate_lockOnMode)
            {
                if(transform.forward.y < lockOn_upper_limit && rotY < 0 || transform.forward.y > lockOn_lower_limit && rotY > 0)
                {
                    Init(ref rotY);
                }

                transform.RotateAround(lookAt, Vector3.up, rotX);
                transform.RotateAround(lookAt, -transform.right, rotY);
            }
            else if(!playerController.Duplicate_lockOnMode)
            {
                if(transform.forward.y > upper_limit && rotY < 0 || transform.forward.y < lower_limit && rotY > 0)
                {
                    Init(ref rotY);
                }
                transform.RotateAround(lookAt, Vector3.up, rotX);
                transform.RotateAround(lookAt, transform.right, rotY);
            }
        }
        transform.position = lookAt - transform.forward * distanceToPlayerM;
        transform.position = transform.position + transform.right * SlideDistanceM;
    }

}
