using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private CharacterController characterController;
    private float gravity = -9.8f;
    private float JumpPower = 6.0f;
    private bool isfall;
    private bool lockOnMode;
    private Vector3 playerMove_input;
    private Vector2 leftStickVal;
    private Vector2 rightStickVal;
    Vector3 movedir = Vector3.zero;
    private float movedirY;
    private Animator animator;
    private PlayerInput playerInput;
    private const string gamepad = "Gamepad";
    private const string keyboard_mouse = "Keyboard&Mouse";


    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        PlayerMove();

        if(playerInput.currentControlScheme.ToString() == gamepad)
        {
            Debug.Log("gamepad");
        }
        if(playerInput.currentControlScheme.ToString() == keyboard_mouse)
        {
            Debug.Log("keyboard&Mouse");
        }
    }

    private void PlayerMove()
    {
        playerMove_input.x = leftStickVal.x;
        playerMove_input.z = leftStickVal.y;
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * playerMove_input.z + Camera.main.transform.right * playerMove_input.x;
        movedir = moveForward * moveSpeed;
        movedirY += Physics.gravity.y * Time.deltaTime;
        Vector3 globaldir = transform.TransformDirection(movedir);
        characterController.Move(new Vector3(movedir.x, movedirY, movedir.z) * Time.deltaTime);

        var current_gamepad = Gamepad.current;
        var current_keyboard = Keyboard.current;
        var Jump_KB = current_keyboard.spaceKey;
        var Jump_GP = current_gamepad.buttonSouth;

        if(characterController.isGrounded)
        {
            movedirY = gravity;
            isfall = false;
            if(Jump_GP.wasPressedThisFrame || Jump_KB.wasPressedThisFrame)
            {
                movedirY = JumpPower;
            }
        }

        if(movedirY > -5 || movedirY <= -10)
        {
            isfall = true;
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
}
