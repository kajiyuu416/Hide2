using UnityEngine;
using UnityEngine.InputSystem;
public class RayCastCS : MonoBehaviour
{
    public GameObject targetObj;
    private Mesh targetMesh;
    private Material targetRenderer_Mat;
    private PhysicMaterial targetMeshCollider_Mat;
    private MeshFilter targetMF;
    private Rigidbody rigidbody;
    private bool metamorphosisFlag = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }
    // Ray�𐶐��ERay�𓊎ˁERay���Փ˂����I�u�W�F�N�g�̃^�O���r���A�����ƈ�v������̂�������
    private void Update()
    {
        var current_GP = Gamepad.current;
        var change = current_GP.buttonEast;

        if (Input.GetMouseButtonDown(0) || change.wasPressedThisFrame) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int Raydis = 20;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("metamorphosis"))
                {
                    GetComponent<MeshFilter>().mesh = targetMF.mesh;
                    GetComponent<Renderer>().material = targetRenderer_Mat;
                    GetComponent<MeshCollider>().sharedMesh = targetMF.mesh;
                    GetComponent<MeshCollider>().material = targetMeshCollider_Mat;
                    targetObj = hit.collider.gameObject;
                    transform.localScale = targetObj.transform.localScale;
                    metamorphosisFlag = true;
                }
                Debug.DrawRay(ray.origin, ray.direction * Raydis, Color.red, 5);
            }
        }
        if (metamorphosisFlag)
        {
            rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private void FixedUpdate()
    {
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        int Raydis2 = 20;

        if(Physics.Raycast(raycast, out raycastHit))
        {
            if(raycastHit.collider.CompareTag("metamorphosis"))
            {
                targetRenderer_Mat = raycastHit.collider.GetComponent<Renderer>().material;
                targetMF = raycastHit.collider.GetComponent<MeshFilter>();
                targetMeshCollider_Mat = raycastHit.collider.GetComponent<MeshCollider>().material;
                targetMesh = targetMF.mesh;
                string name = raycastHit.collider.gameObject.name;
                Debug.Log(name + "��I���ɕϐg�\�ł�"); // �R���\�[���ɕ\��
            }
            Debug.DrawRay(raycast.origin, raycast.direction * Raydis2, Color.green, 5);
        }
    }
    public bool metamorphosisflag 
    {
        get
        {
            return metamorphosisFlag;
        }
        set
        {
            metamorphosisFlag = value;
        } 
    }
}


