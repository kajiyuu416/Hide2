using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class RayCastCS : MonoBehaviour
{
    [SerializeField] GameObject rayHitObject;
    [SerializeField] Camera cam;
    [SerializeField] Image cursor;
    private PlayerController playerController;
    public SkinnedMeshRenderer skinnedMeshren;
    private SkinnedMeshRenderer target_SkinnedMeshRenderer;
    private Rigidbody rigidbody;
    private bool metamorphosisFlag = false;
    Vector3 center = new Vector3(Screen.width/2,Screen.height/2,0);

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerController = FindObjectOfType<PlayerController>();
        skinnedMeshren = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    // Ray�𐶐��ERay�𓊎ˁERay���Փ˂����I�u�W�F�N�g�̃^�O���r���A�����ƈ�v������̂�������
    private void Update()
    {
        var current_GP = Gamepad.current;
        var change = current_GP.buttonEast;
        if(Input.GetMouseButtonDown(0) || change.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(center);
            RaycastHit hit;
            int Raydis = 10;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.CompareTag("metamorphosis"))
                {
                    rayHitObject = hit.collider.gameObject;
                    skinnedMeshren = target_SkinnedMeshRenderer;
                    GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = skinnedMeshren.sharedMesh;
                    metamorphosisFlag = true;
                }
                Debug.DrawRay(ray.origin, ray.direction * Raydis, Color.red);
            }

        }

        if(playerController.Duplicate_lockOnMode)
        {
            cursor.enabled = true;
            Ray ray = cam.ScreenPointToRay(center);
            RaycastHit raycastHit;

            if(Physics.Raycast(ray, out raycastHit))
            {
                if(raycastHit.collider.CompareTag("metamorphosis"))
                {
                    target_SkinnedMeshRenderer = raycastHit.collider.gameObject.GetComponent<SkinnedMeshRenderer>();
                    string name = raycastHit.collider.gameObject.name;
                    Debug.Log(name + "��I���ɕϐg�\�ł�"); // �R���\�[���ɕ\��
                }
            }
            //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
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


