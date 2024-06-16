using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
public class RayCastCS : MonoBehaviourPun, IPunObservable
{
    [SerializeField] GameObject rayHitObject;
    [SerializeField] Mesh defaultMesh;
    [SerializeField] GameObject metamorphosisEffect;
    [SerializeField] GameObject metamorphosis_unravelEffect;
    public Camera cam;
    public PlayerController playerController;
    private GameManager gameManager;
    private MeshFilter target_MeshFilter;
    private Mesh meshColMesh;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;
    private bool metamorphosisFlag = false;
    Vector3 center = new Vector3(Screen.width/2,Screen.height/2,0);
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        if(photonView.IsMine)
        {
            meshRenderer.enabled = true;
            meshCollider.sharedMesh = meshCollider.sharedMesh;
            meshFilter.mesh = meshFilter.mesh;
        }
    }
    // Rayを生成・Rayを投射・Rayが衝突したオブジェクトのタグを比較し、条件と一致するものだったら
    private void Update()
    {
        if(!photonView.IsMine || playerController == null)
            return;

        var changeGP = gameManager.Duplicate_gamepad_connection.buttonEast;
        if(playerController.Duplicate_lockOnMode)
        {
            gameManager.cursor.enabled = true;
            Ray ray = cam.ScreenPointToRay(center);
            RaycastHit raycastHit;
            if(Physics.Raycast(ray, out raycastHit))
            {
                if(raycastHit.collider.CompareTag("metamorphosis"))
                {
                    gameManager.cursor.color = Color.red;
                    target_MeshFilter = raycastHit.collider.gameObject.GetComponent<MeshFilter>();
                    meshColMesh = raycastHit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh;
                    string name = raycastHit.collider.gameObject.name;
                    Debug.Log(name + "を選択に変身可能です"); // コンソールに表示
                }
                else
                {
                    gameManager.cursor.color = Color.blue;
                }
            }
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        }
        else if(!playerController.Duplicate_lockOnMode)
        {
            gameManager.cursor.enabled = false;

        }
        //オブジェクトへ変身
        if(Input.GetMouseButtonDown(0) || changeGP.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(center);
            RaycastHit hit;
            int Raydis = 10;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.CompareTag("metamorphosis") && rayHitObject != hit.collider.gameObject)
                {
                    rayHitObject = hit.collider.gameObject;
                    GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = null;
                    GetComponent<MeshFilter>().sharedMesh = target_MeshFilter.sharedMesh;
                    GetComponent<MeshCollider>().sharedMesh = meshColMesh;
                    
                    GameObject metamorphosis = Instantiate(metamorphosisEffect, transform.position, Quaternion.Euler(0f, -90f, 0f));
                    playerController.Duplicate_state = (int) PlayerController.player_state.metamorphosisMode;
                    metamorphosisFlag = true;
                }
                Debug.DrawRay(ray.origin, ray.direction * Raydis, Color.red);
            }
        }
        //変身解除
        if(metamorphosisFlag)
        {
            var return_Default_KB = gameManager.Duplicate_keyboard_connection.ctrlKey;
            var return_Default_GP = gameManager.Duplicate_gamepad_connection.buttonNorth;
            if(return_Default_KB.wasPressedThisFrame ||return_Default_GP.wasPressedThisFrame)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = defaultMesh;
                GetComponent<MeshFilter>().sharedMesh = null;
                GetComponent<MeshCollider>().sharedMesh = null;
                rayHitObject = null;
                GameObject metamorphosis_unravel = Instantiate(metamorphosis_unravelEffect, transform.position, Quaternion.Euler(0f, -90f, 0f));
                playerController.Duplicate_state = (int) PlayerController.player_state.defaultMode;
                metamorphosisFlag = false;
            }
        }
        meshRenderer.enabled = meshRenderer.enabled;
        meshCollider.sharedMesh = meshCollider.sharedMesh;
        meshFilter.mesh = meshFilter.mesh;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("情報の更新");
        if(stream.IsWriting)
        {
            // 自分のオブジェクトであれば、メッシュレンダラーの状態を送信する
            stream.SendNext(meshRenderer.enabled);
            stream.SendNext(meshCollider.sharedMesh);
            stream.SendNext(meshFilter.mesh);
        }
        else
        {
            // 他のプレイヤーのオブジェクトであれば、受信した状態を反映する
            meshRenderer.enabled = (bool) stream.ReceiveNext();
            meshCollider.sharedMesh = (Mesh) stream.ReceiveNext();
            meshFilter.mesh = (Mesh) stream.ReceiveNext();
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


