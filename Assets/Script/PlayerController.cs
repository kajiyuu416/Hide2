using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpPower;
    private string tagname;
    private Rigidbody rigidbody;
    private Insta insta;
    public int jump_frequency;
    private const int frequency = 1;
    private Vector3 PlayerMove_input;
    public Vector2 leftStickVal;
    public Vector2 RightStickVal;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        tagname = "candy";
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
                Debug.Log("•¡»‰ñ”‰ñ•œ");
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
            }
        }
    }


    private void PlayerMove()
    {
        PlayerMove_input.x = leftStickVal.x;
        PlayerMove_input.z = leftStickVal.y;
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * PlayerMove_input.z + Camera.main.transform.right * PlayerMove_input.x;
        rigidbody.velocity = moveForward * MoveSpeed + new Vector3(0, rigidbody.velocity.y, 0);

        if(moveForward != Vector3.zero)
        {
            Quaternion QL = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, QL, 15.0f * Time.deltaTime);
        }
    }
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
        RightStickVal = var.Get<Vector2>();
    }

    private void OnJump()
    {
        rigidbody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
    }
}
