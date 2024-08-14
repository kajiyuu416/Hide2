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
    private Color transparentColor = new Color(1, 1, 1, 0.5f); // �������̐F
    private Shader originalShader;
    private Renderer[] renderers;
    private float distanceToPlayerM = 6.0f;    // �J�����ƃv���C���[�Ƃ̋���[m]
    private float heightM = 1.3f;            // �����_�̍���[m]          // �����_�̍���[m]
    private float slideDistanceM = 0.0f;       // �J���������ɃX���C�h������G�v���X�̎��E�ցC�}�C�i�X�̎�����[m]
    public static float rotationSensitivity;// �������x
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
    // �^�O�Ō������āA���̃^�O�������ׂẴQ�[���I�u�W�F�N�g���擾
    // ���ꂼ��̃Q�[���I�u�W�F�N�g����Renderer�R���|�[�l���g���擾
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
            //�w�肵�����C���[�����I�u�W�F�N�g�̏ꍇ
            if(obstructingLayer == (obstructingLayer | (1 << renderer.gameObject.layer)))
            {
                Vector3 directionToPlayer = target.position - camera.transform.position;
                RaycastHit hit;
                var raydis = 10.0f;
                if(Physics.Raycast(camera.transform.position, directionToPlayer, out hit, raydis))
                {
                    if(hit.collider.gameObject == renderer.gameObject)
                    {
                        // ���̃V�F�[�_�[��ۑ�
                        if(originalShader == null)
                        {
                            originalShader = renderer.material.shader;
                        }
                        // �����V�F�[�_�[�ɕύX
                        renderer.material.shader = transparentShader;
                        renderer.material.SetColor("_Color", transparentColor);
                    }
                    else
                    {
                        // ���̃V�F�[�_�[�ɖ߂�
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
