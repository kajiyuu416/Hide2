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

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * input.z + Camera.main.transform.right * input.x;
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
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
                Debug.Log("複製回数回復");
                Destroy(collision.gameObject);
            }
        }

    }
}
