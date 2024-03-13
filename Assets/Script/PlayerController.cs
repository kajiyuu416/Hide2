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
    private GameInputs gameInputs;
    private Vector2 moveInputValue;
    Vector3 input;

    // Start is called before the first frame update

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        tagname = "candy";

        gameInputs = new GameInputs();
        gameInputs.Player.Move.started += OnMove;
        gameInputs.Player.Move.performed += OnMove;
        gameInputs.Player.Move.canceled += OnMove;
        gameInputs.Player.Jump.performed += OnJump;

        gameInputs.Enable();
    }
    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        gameInputs?.Dispose();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        moveInputValue = context.ReadValue<Vector2>();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        // ジャンプする力を与える
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
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");
        ////// カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        ////// 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * input.z + Camera.main.transform.right * input.x;
        //// 移動方向の力を与える
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
                Debug.Log("複製回数回復");
                Destroy(collision.gameObject);
            }
        }

    }
}
