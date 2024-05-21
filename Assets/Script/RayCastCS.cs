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
    // Rayを生成・Rayを投射・Rayが衝突したオブジェクトのタグを比較し、条件と一致するものだったら
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
                    metamorphosisFlag = true;
                    targetMesh = hit.collider.GetComponent<Mesh>();
                    targetRenderer_Mat = hit.collider.GetComponent<Renderer>().material;
                    targetMF = hit.collider.GetComponent<MeshFilter>();
                    targetMeshCollider_Mat = hit.collider.GetComponent<MeshCollider>().material;
                    targetObj = hit.collider.gameObject;
                    targetMesh = targetMF.mesh;
                    GetComponent<MeshFilter>().mesh = targetMF.mesh;
                    GetComponent<Renderer>().material = targetRenderer_Mat;
                    GetComponent<MeshCollider>().sharedMesh = targetMF.mesh;
                    GetComponent<MeshCollider>().material = targetMeshCollider_Mat;
                    transform.localScale = targetObj.transform.localScale;
                    string name = hit.collider.gameObject.name;
                    Debug.Log(name); // コンソールに表示
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
                Mesh hitObj_mesh = raycastHit.collider.GetComponent<Mesh>();
                GameObject hitObj = raycastHit.collider.gameObject;
                Transform hitobj_transform = hitObj.GetComponent<Transform>();
                Debug.Log(hitobj_transform);

                string name = raycastHit.collider.gameObject.name;
                Debug.Log(name + "を選択に変身可能です"); // コンソールに表示
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


