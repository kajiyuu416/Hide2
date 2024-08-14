using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Cinemachine;

public class PlayerCamera : Photon.Pun.MonoBehaviourPun
{
    public Transform target;
    public PlayerController playerController;
    [SerializeField] private LayerMask obstructingLayer;
    [SerializeField] private Shader transparentShader;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float upper_limit = 0.9f;
    [SerializeField] private float lower_limit = -0.9f;
    [SerializeField] private float lockOn_upper_limit = -0.9f;
    [SerializeField] private float lockOn_lower_limit = 0.9f;
    private Color transparentColor = new Color(1, 1, 1, 0.5f); // 半透明の色
    private Shader originalShader;
    private Renderer[] renderers;
    private float distanceToPlayerM = 6.0f;    // カメラとプレイヤーとの距離[m]
    private float heightM = 1.3f;            // 注視点の高さ[m]          // 注視点の高さ[m]
    private float slideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    public static float rotationSensitivity;// 初期感度
    private const float min_distanceToPlayerM = 5.0f;
    private const float max_distanceToPlayerM = 7.0f;
    private const float max_slidedistanceM = 1.0f;
    private const string target_tag = "wall";
    private string childName = "cameraTarget";
    private Camera camera;
    private GameOption gameOption;

    private void Start()
    {
        camera = GetComponent<Camera>();
        if(camera.GetComponent<PhotonView>().IsMine)
        {
            camera.targetDisplay = 0;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            virtualCamera.enabled = true;
        }
        else
        {
            camera.targetDisplay = 1;
        }
        GetRenderersByTag(target_tag);
        gameOption = FindObjectOfType<GameOption>();
    }
    private void Update()
    {
        if(!photonView.IsMine || playerController == null || target == null)
            return;

        if(!gameOption.Duplicate_openOption)
            lockOn();

        if(virtualCamera.LookAt == null)
        {
            Transform childTransform = target.Find(childName);
            virtualCamera.LookAt = childTransform;
        }

        float RotationSensitivity_Lower_Limit = 10.0f;

        if(rotationSensitivity <= RotationSensitivity_Lower_Limit)
        {
            rotationSensitivity = RotationSensitivity_Lower_Limit;
        }
    }
    private void LateUpdate()
    {
        if(!photonView.IsMine || playerController == null || target == null)
            return;
        lockOnstatechange();
        TransmissionObject();
    }
    // タグで検索して、そのタグを持つすべてのゲームオブジェクトを取得
    // それぞれのゲームオブジェクトからRendererコンポーネントを取得
    private void GetRenderersByTag(string tag)
    {
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        renderers = new Renderer[gameObjectsWithTag.Length];

        for(int i = 0; i < gameObjectsWithTag.Length; i++)
        {
            renderers[i] = gameObjectsWithTag[i].GetComponent<Renderer>();
        }
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
                var raydis = 10.0f;
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
            distanceToPlayerM = min_distanceToPlayerM;
            slideDistanceM = max_slidedistanceM;
            playerController.Duplicate_lockOnMode = true;
        }
        else
        {
            playerController.Duplicate_lockOnMode = false;
            distanceToPlayerM = max_distanceToPlayerM;
            slideDistanceM = 0f;
        }
    }
    private void lockOnstatechange()
    {
        var rotX = playerController.Duplicate_rightStickVal.x * Time.deltaTime * rotationSensitivity;
        var rotY = playerController.Duplicate_rightStickVal.y * Time.deltaTime * rotationSensitivity;
        var lookAt = target.transform.position + Vector3.up * heightM;

        if(!gameOption.Duplicate_openOption)
        {

            if(!gameOption.Duplicate_camera_Left_and_Right_Flip)
            {
                transform.RotateAround(lookAt, Vector3.up, rotX);
            }
            else
            {
                transform.RotateAround(lookAt, Vector3.down, rotX);
            }

            if(!gameOption.Duplicate_camera_Upside_Down_Flip)
            {
                if(playerController.Duplicate_lockOnMode)
                {
                    if(transform.forward.y < lockOn_upper_limit && rotY < 0 || transform.forward.y > lockOn_lower_limit && rotY > 0)
                    {
                        Init(ref rotY);
                    }
                    transform.RotateAround(lookAt, -transform.right, rotY);
                }
                else
                {
                    if(transform.forward.y > upper_limit && rotY < 0 || transform.forward.y < lower_limit && rotY > 0)
                    {
                        Init(ref rotY);
                    }
                    transform.RotateAround(lookAt, transform.right, rotY);
                }
            }
            else if(gameOption.Duplicate_camera_Upside_Down_Flip)
            {
                if(playerController.Duplicate_lockOnMode)
                {
                    if(transform.forward.y > lockOn_lower_limit && rotY < 0 || transform.forward.y < lockOn_upper_limit && rotY > 0)
                    {
                        Init(ref rotY);
                    }
                    transform.RotateAround(lookAt, transform.right, rotY);
                }
                else
                {
                    if(transform.forward.y < lower_limit && rotY < 0 || transform.forward.y > upper_limit && rotY > 0)
                    {
                        Init(ref rotY);
                    }
                    transform.RotateAround(lookAt, -transform.right, rotY);
                }
            }

        }
        transform.position = lookAt - transform.forward * distanceToPlayerM;
        transform.position = transform.position + transform.right * slideDistanceM;
    }
    private void Init(ref float val)
    {
        val = 0;
    }
}
