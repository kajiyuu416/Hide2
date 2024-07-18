using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.XR;
public class PlayerController : Photon.Pun.MonoBehaviourPun
{
    private float moveSpeed = 8.0f;
    private RayCastCS raycastCS;
    private CharacterController characterController;
    private GameManager gameManager;
    private int state = (int) player_state.defaultMode;
    private int oldstate;
    private const float gravity = -9.8f;
    private const float JumpPower = 5.0f;
    private float movedirY;
    private const float defaultColliderHeight = 1.6f;
    private const float defaultColliderCenter = 0.9f;
    private bool isfall;
    private bool lockOnMode;
    private Vector2 leftStickVal;
    private Vector2 rightStickVal;
    private Vector3 playerMove_input;
    private Vector3 movedir = Vector3.zero;
    private Animator animator;
    public Camera cloneCamera;
    private PlayerInput playerInput;
    private GameObject namePlate;
    public TextMeshProUGUI nameText;
    public enum player_state
    {
        defaultMode, metamorphosisMode
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        raycastCS = GetComponent<RayCastCS>();
        gameManager = FindObjectOfType<GameManager>();
        oldstate = state;

        if(!photonView.IsMine)
        {
            Destroy(playerInput);
        }
    }
    private void Start()
    {
        namePlate = nameText.transform.parent.gameObject;
    }
    private void Update()
    {
        if(!photonView.IsMine||cloneCamera == null)
        {
            return;
        }
        Change_State();
        PlayerMove();
    }
    private void LateUpdate()
    {
        if(!photonView.IsMine || cloneCamera == null)
        {
            return;
        }
        namePlate.transform.rotation = cloneCamera.transform.rotation;
    }
    [PunRPC]
    public void SetName(string name)
    {
        nameText.text = name;
    }
    private void PlayerMove()
    {
        if(!photonView.IsMine || cloneCamera == null)
        {
            return;
        }
        playerMove_input.x = leftStickVal.x;
        playerMove_input.z = leftStickVal.y;
        Vector3 cameraForward = Vector3.Scale(cloneCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * playerMove_input.z + cloneCamera.transform.right * playerMove_input.x;
        movedir = moveForward * moveSpeed;
        movedirY += Physics.gravity.y * Time.deltaTime;
        Vector3 globaldir = transform.TransformDirection(movedir);
        characterController.Move(new Vector3(movedir.x, movedirY, movedir.z) * Time.deltaTime);

        if(characterController.isGrounded)
        {
            movedirY = gravity;
            isfall = false;
            if(gameManager.Duplicate_gamepad_connection.buttonSouth.wasPressedThisFrame || gameManager.Duplicate_keyboard_connection.spaceKey.wasPressedThisFrame)
            {
                movedirY = JumpPower;
            }
        }
        if(!raycastCS.metamorphosisflag)
        {
            if(movedirY > -5 || movedirY <= -10)
            {
                isfall = true;
                characterController.height = animator.GetFloat("ColliderHeight");
                characterController.center = new Vector3(characterController.center.x, animator.GetFloat("ColliderCenter"), characterController.center.z);
            }

            if(!isfall)
            {
                characterController.height = defaultColliderHeight;
                characterController.center = new Vector3(characterController.center.x, defaultColliderCenter, characterController.center.z);
            }
        }
        //キャラクターの回転
        if(cameraForward != Vector3.zero && lockOnMode)
        {
            Quaternion QL = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, QL, 10.0f * Time.deltaTime);
        }
        else if(moveForward != Vector3.zero&& !lockOnMode)
        {
            Quaternion QL = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, QL, 10.0f * Time.deltaTime);
        }
        animator.SetFloat("movespeed", moveForward.magnitude, 0.1f, Time.deltaTime);
        animator.SetBool("isfall", isfall);
    }

    private void Change_State()
    {
        var metamorphosis_height = 0.5f;
        var metamorphosis_center = 0.4f;
        var litleJump = 4;
        if(state != oldstate)
        {
            oldstate = state;
            switch(state)
            {
                case (int) player_state.defaultMode:
                    movedirY = litleJump;
                    characterController.height = defaultColliderHeight;
                    characterController.center = new Vector3(characterController.center.x, defaultColliderCenter, characterController.center.z);
                    characterController.detectCollisions = true;
                    photonView.RPC("SyncPlayerSize", RpcTarget.AllBuffered, characterController.height, characterController.radius, characterController.center,characterController.detectCollisions);
                    Debug.Log("defaultMode");
                    break;

                case (int) player_state.metamorphosisMode:
                    characterController.height = metamorphosis_height;
                    characterController.center = new Vector3(characterController.center.x, metamorphosis_center, characterController.center.z);
                    characterController.detectCollisions = false;
                    photonView.RPC("SyncPlayerSize", RpcTarget.AllBuffered, characterController.height, characterController.radius, characterController.center, characterController.detectCollisions);
                    Debug.Log("metamorMode");
                    break;

                default:
                    break;
            }
        }
    }
    // 特定のオブジェクトにぶつかった場合の処理
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("ReSet"))
        {
            transform.position = Vector3.zero;
            gameManager.change_sky(0);
        }
        if(hit.gameObject.CompareTag("changeArea"))
        {
           transform.position =  hit.gameObject.GetComponent<transformStage>().transformPos.position;
            gameManager.change_sky(1);
        }
    }

    [PunRPC]
    public void SyncPlayerSize(float height, float radius,Vector3 center, bool detectCollisions)
    {
        // 自身以外のプレイヤーの場合、大きさを設定する
        if(!photonView.IsMine)
        {
            characterController.height = height;
            characterController.radius = radius;
            characterController.center = center;
            characterController.detectCollisions = detectCollisions;
        }
    }

    private void OnMove(InputValue var)
    {
        leftStickVal = var.Get<Vector2>();
    }
    private void OnCamera(InputValue var)
    {
        rightStickVal = var.Get<Vector2>();
    }

    public Vector2 Duplicate_rightStickVal
    {
        get
        {
            return rightStickVal;
        }
    }
    public bool Duplicate_lockOnMode
    {
        get
        {
            return lockOnMode;
        }
        set
        {
            lockOnMode = value;
        }
    }
    public PlayerInput Duplicate_PlayerInput
    {
        get
        {
            return playerInput;
        }
    }

    public int Duplicate_state
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }
    
}
