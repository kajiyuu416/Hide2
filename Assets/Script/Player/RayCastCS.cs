using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class RayCastCS : MonoBehaviour
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
    private bool metamorphosisFlag = false;
    Vector3 center = new Vector3(Screen.width/2,Screen.height/2,0);
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    // Rayを生成・Rayを投射・Rayが衝突したオブジェクトのタグを比較し、条件と一致するものだったら
    private void Update()
    {
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
                    GetComponentInChildren<MeshFilter>().sharedMesh = target_MeshFilter.sharedMesh;
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
                GetComponentInChildren<MeshFilter>().sharedMesh = null;
                GetComponent<MeshCollider>().sharedMesh = null;
                rayHitObject = null;
                GameObject metamorphosis_unravel = Instantiate(metamorphosis_unravelEffect, transform.position, Quaternion.Euler(0f, -90f, 0f));
                playerController.Duplicate_state = (int) PlayerController.player_state.defaultMode;
                metamorphosisFlag = false;
            }
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


