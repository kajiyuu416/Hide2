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
    private bool isJump;
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
        var current_GP = Gamepad.current;
        var Jump = current_GP.buttonSouth;
        if(Jump.wasPressedThisFrame &&isJump)
        {
            OnJump();
        }
    }
    private void FixedUpdate()
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
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            isJump= true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            isJump = false;
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
