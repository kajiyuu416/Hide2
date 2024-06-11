using UnityEngine;
using UnityEngine.InputSystem;
public class RayCastCS : MonoBehaviour
{
    [SerializeField] GameObject rayHitObject;
    public GameObject objctacleRayObject;
    public LayerMask layerMask;
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
    // Rayを生成・Rayを投射・Rayが衝突したオブジェクトのタグを比較し、条件と一致するものだったら
    private void Update()
    {
        var current_GP = Gamepad.current;
        var change = current_GP.buttonEast;

        if (Input.GetMouseButtonDown(0) || change.wasPressedThisFrame) 
        {
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

        }
        if (metamorphosisFlag)
        {
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        if(Physics.Raycast(objctacleRayObject.transform.position, transform.TransformDirection(Vector3.forward), 10))
        {
            Debug.DrawRay(objctacleRayObject.transform.position, Vector3.forward, Color.red);
            Debug.Log("ray");
        }
    }

    //private void FixedUpdate()
    //{
    //    Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit raycastHit;

    //    int Raydis2 = 20;

    //    if(Physics.Raycast(raycast, out raycastHit))
    //    {
    //        if(raycastHit.collider.CompareTag("metamorphosis"))
    //        {
    //            targetRenderer_Mat = raycastHit.collider.GetComponent<Renderer>().material;
    //            targetMF = raycastHit.collider.GetComponent<MeshFilter>();
    //            targetMeshCollider_Mat = raycastHit.collider.GetComponent<MeshCollider>().material;
    //            targetMesh = targetMF.mesh;
    //            string name = raycastHit.collider.gameObject.name;
    //            Debug.Log(name + "を選択に変身可能です"); // コンソールに表示
    //        }
    //        Debug.DrawRay(raycast.origin, raycast.direction * Raydis2, Color.green, 5);
    //    }
    //}
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


