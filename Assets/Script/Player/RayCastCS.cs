using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using System;
using System.IO;
public class RayCastCS : MonoBehaviourPun
{
    [SerializeField] GameObject rayHitObject;
    [SerializeField] GameObject metamorphosisEffect;
    [SerializeField] GameObject metamorphosis_unravelEffect;
    public Camera cam;
    public PlayerController playerController;
    private const float raydis = 10.0f;
    private GameManager gameManager;
    private MeshFilter target_MeshFilter;
    private MeshCollider target_MeshCollider;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Mesh defaultMesh;
    private SkinnedMeshRenderer skinnedMesh;
    private bool metamorphosisFlag = false;
    private Vector3 center = new Vector3(Screen.width/2,Screen.height/2,0);


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        defaultMesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;

    }
    // Ray�𐶐��ERay�𓊎ˁERay���Փ˂����I�u�W�F�N�g�̃^�O���r���A�����ƈ�v������̂�������
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
            if(Physics.Raycast(ray, out raycastHit, raydis))
            {
                if(raycastHit.collider.CompareTag("metamorphosis"))
                {
                    gameManager.cursor.color = Color.red;
                    string name = raycastHit.collider.gameObject.name;
                    Debug.Log(name + "�ɕϐg�\�ł�"); // �R���\�[���ɕ\��
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
            gameManager.cursor.color = Color.blue;

        }
        //�I�u�W�F�N�g�֕ϐg
        if(Input.GetMouseButtonDown(0) || changeGP.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(center);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit,raydis))
            {
                if(hit.collider.CompareTag("metamorphosis") && rayHitObject != hit.collider.gameObject)
                {
                    rayHitObject = hit.collider.gameObject;
                    target_MeshFilter = rayHitObject.GetComponent<MeshFilter>();
                    target_MeshCollider = rayHitObject.GetComponent<MeshCollider>();

                    ChangeMesh();
                    PhotonNetwork.Instantiate("StarLight", transform.position, Quaternion.identity);
                    playerController.Duplicate_state = (int) PlayerController.player_state.metamorphosisMode;
                    metamorphosisFlag = true;
                    string name = hit.collider.gameObject.name;
                    Debug.Log(name + "�ϐg���܂���"); // �R���\�[���ɕ\��
                }
                Debug.DrawRay(ray.origin, ray.direction * raydis, Color.red);
            }
        }

        //�ϐg����
        if(metamorphosisFlag)
        {
            var return_Default_KB = gameManager.Duplicate_keyboard_connection.ctrlKey;
            var return_Default_GP = gameManager.Duplicate_gamepad_connection.buttonNorth;

            if(return_Default_KB.wasPressedThisFrame ||return_Default_GP.wasPressedThisFrame)
            {
                rayHitObject = null;
                ResetPlayerMesh();
                PhotonNetwork.Instantiate("Smoke", transform.position, Quaternion.identity);
                playerController.Duplicate_state = (int) PlayerController.player_state.defaultMode;
                metamorphosisFlag = false;           
            }
        }
    }

    public void ResetPlayerMesh()
    {
        photonView.RPC("ResetMesh", RpcTarget.All);
    }

    // ���b�V����ύX���鏈��
    public void ChangeMesh()
    {
        Mesh mesh = target_MeshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector2[] uv = mesh.uv;
        // ���b�V���̏��𑗐M�\�Ȍ`���ɕϊ�
        byte[] serializedMeshData = SerializeMeshData(vertices, triangles, uv);
        // RPC�ő��̃N���C�A���g�ɑ��M����
        photonView.RPC("ChangePlayerMesh", RpcTarget.All, serializedMeshData);
    }

    // ���b�V���f�[�^����M���čč\�z
    [PunRPC]
    public void ChangePlayerMesh(byte[] serializedMeshData)
    {
        // ��M�����f�[�^���f�V���A���C�Y���ă��b�V�����č\�z
         Mesh receivedMesh = DeserializeMeshData(serializedMeshData);

        // ��M�������b�V���𔽉f���鏈��
        skinnedMesh.sharedMesh = null;
        meshFilter.sharedMesh = receivedMesh;
        meshCollider.sharedMesh = receivedMesh;
    }
    // ���b�V���f�[�^�̃��Z�b�g
    [PunRPC]
    public void ResetMesh()
    {
        meshFilter.sharedMesh = null;
        meshCollider.sharedMesh = null;
        skinnedMesh.sharedMesh = defaultMesh;
    }
    // ���b�V���f�[�^���V���A���C�Y���郁�\�b�h
    private byte[] SerializeMeshData(Vector3[] vertices, int[] triangles, Vector2[] uv)
    {
        // ���ۂ̃f�[�^���o�C�g�z��ɕϊ�
        // ���_�f�[�^���o�C�g�z��ɃV���A���C�Y
        List<byte> byteStream = new List<byte>();

        // ���_����擪�ɏ������ށi�����Ƃ��āj
        byteStream.AddRange(BitConverter.GetBytes(vertices.Length));

        // ���_���W�����ԂɃo�C�g�z��ɒǉ�����
        foreach(Vector3 vertex in vertices)
        {
            byteStream.AddRange(BitConverter.GetBytes(vertex.x));
            byteStream.AddRange(BitConverter.GetBytes(vertex.y));
            byteStream.AddRange(BitConverter.GetBytes(vertex.z));
        }

        // �O�p�`�C���f�b�N�X�̐������ɏ�������
        byteStream.AddRange(BitConverter.GetBytes(triangles.Length));

        // �O�p�`�C���f�b�N�X�����ԂɃo�C�g�z��ɒǉ�����
        foreach(int triangleIndex in triangles)
        {
            byteStream.AddRange(BitConverter.GetBytes(triangleIndex));
        }

        // UV���W�̐������ɏ�������
        byteStream.AddRange(BitConverter.GetBytes(uv.Length));

        // UV���W�����ԂɃo�C�g�z��ɒǉ�����
        foreach(Vector2 uvCoord in uv)
        {
            byteStream.AddRange(BitConverter.GetBytes(uvCoord.x));
            byteStream.AddRange(BitConverter.GetBytes(uvCoord.y));
        }

        return byteStream.ToArray();

    }

    // ���b�V���f�[�^���f�V���A���C�Y����Mesh�I�u�W�F�N�g���č\�z���郁�\�b�h
    private Mesh DeserializeMeshData(byte[] serializedMeshData)
    {
        // ���ۂ̃f�V���A���C�Y�̃��W�b�N������
        Mesh mesh = new Mesh();
        using(MemoryStream stream = new MemoryStream(serializedMeshData))
        {
            using(BinaryReader reader = new BinaryReader(stream))
            {
                // ���_�f�[�^�̕���
                int vertexCount = reader.ReadInt32();
                Vector3[] vertices = new Vector3[vertexCount];
                for(int i = 0; i < vertexCount; i++)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    float z = reader.ReadSingle();
                    vertices[i] = new Vector3(x, y, z);
                }

                // �O�p�`�C���f�b�N�X�̕���
                int triangleCount = reader.ReadInt32();
                int[] triangles = new int[triangleCount];
                for(int i = 0; i < triangleCount; i++)
                {
                    triangles[i] = reader.ReadInt32();
                }

                // UV���W�̕���
                int uvCount = reader.ReadInt32();
                Vector2[] uv = new Vector2[uvCount];
                for(int i = 0; i < uvCount; i++)
                {
                    float u = reader.ReadSingle();
                    float v = reader.ReadSingle();
                    uv[i] = new Vector2(u, v);
                }

                // ���b�V���Ƀf�[�^��ݒ肷��
                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.uv = uv;
            }
        }

        mesh.RecalculateNormals(); // �K�v�ɉ����Ė@�����Čv�Z����Ȃǂ̌㏈�����s��

        return mesh;
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


