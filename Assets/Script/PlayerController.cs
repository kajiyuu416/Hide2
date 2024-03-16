using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpPower;
    [SerializeField] RayCastCS RC;
    private string tagname;
    Rigidbody rigidbody;
    public Vector2 moveInputValue;
    Vector3 input;

    // Start is called before the first frame update

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        tagname = "candy";
    }

    public void OnMove(InputValue var)
    {
        moveInputValue = var.Get<Vector2>();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        // �W�����v����͂�^����
        rigidbody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        moveInputValue.x = Input.GetAxis("Horizontal");
        moveInputValue.y = Input.GetAxis("Vertical");
        ////// �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        ////// �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveForward = cameraForward * moveInputValue.y + Camera.main.transform.right * moveInputValue.x;
        //// �ړ������̗͂�^����
        rigidbody.velocity = moveForward * MoveSpeed + new Vector3(
            moveInputValue.x,
            0,
            moveInputValue.y);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(Insta.count < 5)
        {
            if (collision.gameObject.tag == tagname)
            {
                Insta.count++;
                Debug.Log("�����񐔉�");
                Destroy(collision.gameObject);
            }
        }

    }
}
