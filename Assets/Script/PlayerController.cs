using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    private Insta insta;
    private string tagname = "candy";
    private int jump_frequency;
    private const int frequency = 1;
    private bool isjump;
    private bool isfall;
    private Vector3 playerMove_input;
    private Vector2 leftStickVal;
    private Vector2 rightStickVal;
    private Rigidbody rigidbody;
    private Animator animator;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        insta = FindObjectOfType<Insta>();
    }
    private void Update()
    {
        Player_Jump();
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }
    private void OnTriggerEnter(Collider collision)
    {
        int count = insta.duplicate_Count;
        if(count < 5)
        {
            if (collision.gameObject.tag == tagname)
            {
                insta.duplicate_Count++;
                Destroy(collision.gameObject);
                Debug.Log("複製回数回復");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "metamorphosis")
        {
            jump_frequency++;
        }

        if(collision.gameObject.tag == "ground")
        {
            jump_frequency = frequency;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(jump_frequency > 0)
        {
            if(collision.gameObject.tag == "ground")
            {
                jump_frequency = frequency;
                isjump = false;
                isfall = false;
            }
        }
    }


    private void PlayerMove()
    {
        playerMove_input.x = leftStickVal.x;
        playerMove_input.z = leftStickVal.y;
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * playerMove_input.z + Camera.main.transform.right * playerMove_input.x;
        var velocity = new Vector3(playerMove_input.x, 0, playerMove_input.z).normalized;
        rigidbody.velocity = moveForward * moveSpeed + new Vector3(0, rigidbody.velocity.y, 0);
        const int falldis = -2;

        if(moveForward != Vector3.zero)
        {
            Quaternion QL = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, QL, 10.0f * Time.deltaTime);
        }
        if(rigidbody.velocity.y < falldis)
        {
            isfall = true;
            isjump = false;
        }
        animator.SetFloat("movespeed",velocity.magnitude, 0.1f, Time.deltaTime);
        animator.SetBool("isfall", isfall);
        animator.SetBool("isjump", isjump);

    }
    //ジャンプキーが押され、ジャンプができる状態であればジャンプを行う
    private void Player_Jump()
    {
        var current_GP = Gamepad.current;
        var current_KB = Keyboard.current;
        var Jump_KB = current_KB.spaceKey;
        var Jump_GP = current_GP.buttonSouth;
        int max_frequency = 1;
        int min_frequency = 0;

        if(jump_frequency >= frequency)
        {

            if(Jump_GP.wasPressedThisFrame || Jump_KB.wasPressedThisFrame)
            {
                jump_frequency--;
                OnJump();
            }
        }

        //ジャンプ回数の制限
        if(jump_frequency > max_frequency)
        {
            jump_frequency = max_frequency;
        }

        if(jump_frequency <= min_frequency)
        {
            jump_frequency = min_frequency;
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

    //ジャンプ時呼ばれる関数
    private void OnJump()
    {
        rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isjump = true;
    }
    public Vector2 Duplicate_rightStickVal
    {
        get
        {
            return rightStickVal;
        }
    }
}
