using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RayCastCS : MonoBehaviour
{
    [SerializeField] Mesh targetMesh;
    [SerializeField] Material targetMat;
    [SerializeField] MeshFilter targetMF;
    [SerializeField] GameObject originObj;
    public bool metamorphosisFlag = false;
    public GameObject targetObj;
    private float originSize = 1.0f;
    Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    { // Rayを生成・Rayを投射・Rayが衝突したオブジェクトのタグを比較し、条件と一致するものだったら
      //
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int Raydis = 20;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("metamorphosis"))
                {
                    metamorphosisFlag = true;
                    originObj.SetActive(false);
                    targetMesh = hit.collider.GetComponent<Mesh>();
                    targetMat = hit.collider.GetComponent<Renderer>().material;
                    targetMF = hit.collider.GetComponent<MeshFilter>();
                    targetObj = hit.collider.gameObject;
                    targetMesh = targetMF.mesh;
                    GetComponent<MeshFilter>().mesh = targetMF.mesh;
                    GetComponent<Renderer>().material = targetMat;
                    GetComponent<MeshCollider>().sharedMesh = targetMF.mesh;
                    this.transform.localScale = new Vector3(originSize, originSize, originSize);
                    this.transform.localScale = targetObj.transform.localScale;
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
}


