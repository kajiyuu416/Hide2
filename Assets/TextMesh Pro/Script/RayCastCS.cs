using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class RayCastCS : MonoBehaviour
{
    [SerializeField] GameObject rayHitObject;
    [SerializeField] Camera cam;
    [SerializeField] Image cursor;
    public LayerMask layerMask;
    private PlayerController playerController;
    private Mesh targetMesh;
    private Material targetRenderer_Mat;
    private PhysicMaterial targetMeshCollider_Mat;
    private MeshFilter targetMF;
    private Rigidbody rigidbody;
    private bool metamorphosisFlag = false;
    Vector3 center = new Vector3(Screen.width/2,Screen.height/2,0);

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerController = FindObjectOfType<PlayerController>();
    }
    // Ray�𐶐��ERay�𓊎ˁERay���Փ˂����I�u�W�F�N�g�̃^�O���r���A�����ƈ�v������̂�������
    private void Update()
    {
        //var current_GP = Gamepad.current;
        //var change = current_GP.buttonEast;
        //if (Input.GetMouseButtonDown(0) || change.wasPressedThisFrame) 
        //{
        //Ray ray = Physics.Raycast(objctacleRayObject.transform.position,Vector3.up,out);

        //RaycastHit hit;
        //int Raydis = 20;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    if (hit.collider.CompareTag("metamorphosis"))
        //    {
        //        GetComponent<MeshFilter>().mesh = targetMF.mesh;
        //        GetComponent<Renderer>().material = targetRenderer_Mat;
        //        GetComponent<MeshCollider>().sharedMesh = targetMF.mesh;
        //        GetComponent<MeshCollider>().material = targetMeshCollider_Mat;
        //        rayHitObject = hit.collider.gameObject;
        //        transform.localScale = rayHitObject.transform.localScale;
        //        metamorphosisFlag = true;
        //    }
        //    Debug.DrawRay(ray.origin, ray.direction * Raydis, Color.red, 5);
        //}

        //}
        if(playerController.Duplicate_lockOnMode)
        {
            cursor.enabled = true;
            Ray ray = cam.ScreenPointToRay(center);
            RaycastHit raycastHit;

            if(Physics.Raycast(ray, out raycastHit))
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
            }
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        }
        else if(!playerController.Duplicate_lockOnMode)
        {
            cursor.enabled = false;
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


