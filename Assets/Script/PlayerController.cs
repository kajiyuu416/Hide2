using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpPower;
    [SerializeField] RayCastCS RC;
    private string tagname;
    Rigidbody rigidbody;
    Vector3 input;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        tagname = "candy";
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");

    }
    void FixedUpdate()
    {
        var velocity = new Vector3(input.x, 0, input.z).normalized;

        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveForward = cameraForward * input.z + Camera.main.transform.right * input.x;
        // �ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
        rigidbody.velocity = moveForward * MoveSpeed + new Vector3(0, rigidbody.velocity.y, 0);
        if (moveForward != Vector3.zero)
        {
            if(!RC.metamorphosisFlag)
            {
                Quaternion QL = Quaternion.LookRotation(moveForward);
                transform.rotation = Quaternion.Lerp(transform.rotation, QL, 15.0f * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(Insta.count > 0)
        {
            if (collision.gameObject.tag == tagname)
            {
                Insta.count--;
                Debug.Log("�����񐔉�");
                Destroy(collision.gameObject);
            }
        }

    }
}
