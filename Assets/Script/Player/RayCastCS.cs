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
    private GameManager gameManager;
    private GameOption gameOption;
    private MeshFilter target_MeshFilter;
    private MeshFilter meshFilter;
    private MeshCollider target_MeshCollider;
    private MeshCollider meshCollider;
    private Mesh defaultMesh;
    private SkinnedMeshRenderer skinnedMesh;
    private bool metamorphosisFlag = false;
    private const float raydis = 10.0f;
    private const string metamorphosisTag = "metamorphosis";
    private Vector3 center = new Vector3(Screen.width/2,Screen.height/2,0);


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameOption = FindObjectOfType<GameOption>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        defaultMesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
    }
    // Rayを生成・Rayを投射・Rayが衝突したオブジェクトのタグを比較し、条件と一致するものだったら
    private void Update()
    {
        if(!photonView.IsMine || playerController == null)
        {
            return;
        }
        if(!gameOption.Duplicate_openOption)
            Player_metamorphosis();
    }

    private void Player_metamorphosis()
    {
        if(playerController.Duplicate_lockOnMode)
        {
            var changeGP = gameManager.Duplicate_gamepad_connection.rightShoulder;

            gameManager.cursor.enabled = true;
            Ray ray = cam.ScreenPointToRay(center);
            RaycastHit raycastHit;
            if(Physics.Raycast(ray, out raycastHit, raydis))
            {
                if(raycastHit.collider.CompareTag(metamorphosisTag))
                {
                    gameManager.cursor.color = Color.red;
                }
                else
                {
                    gameManager.cursor.color = Color.blue;
                }
            }
            Debug.DrawRay(ray.origin, ray.direction * raydis, Color.green);
            //オブジェクトへ変身
            if(Input.GetMouseButtonDown(0) || changeGP.wasPressedThisFrame)
            {
                Ray _ray = cam.ScreenPointToRay(center);
                RaycastHit hit;
                if(Physics.Raycast(_ray, out hit, raydis))
                {
                    if(hit.collider.CompareTag(metamorphosisTag) && rayHitObject != hit.collider.gameObject)
                    {
                        rayHitObject = hit.collider.gameObject;
                        target_MeshFilter = rayHitObject.GetComponent<MeshFilter>();
                        target_MeshCollider = rayHitObject.GetComponent<MeshCollider>();
                        ChangeMesh();
                        PhotonNetwork.Instantiate("StarLight", transform.position, Quaternion.identity);
                        playerController.Duplicate_state = (int) PlayerController.player_state.metamorphosisMode;
                        metamorphosisFlag = true;
                    }
                    Debug.DrawRay(_ray.origin, _ray.direction * raydis, Color.red);
                }
            }
        }
        else if(!playerController.Duplicate_lockOnMode)
        {
            gameManager.cursor.enabled = false;
            gameManager.cursor.color = Color.blue;

        }

        //変身解除
        if(metamorphosisFlag)
        {
            var return_Default_KB = gameManager.Duplicate_keyboard_connection.ctrlKey;
            var return_Default_GP = gameManager.Duplicate_gamepad_connection.buttonNorth;

            if(return_Default_KB.wasPressedThisFrame || return_Default_GP.wasPressedThisFrame)
            {
                ResetPlayerMesh();
            }
        }
    }

    public void ResetPlayerMesh()
    {
        if(metamorphosisFlag)
        {
            playerController.Duplicate_state = (int) PlayerController.player_state.defaultMode;
            metamorphosisFlag = false;
            rayHitObject = null;
            PhotonNetwork.Instantiate("Smoke", transform.position, Quaternion.identity);
        }
        photonView.RPC("ResetMesh", RpcTarget.All);
    }

    // メッシュを変更する処理
    public void ChangeMesh()
    {
        Mesh mesh = target_MeshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector2[] uv = mesh.uv;
        // メッシュの情報を送信可能な形式に変換
        byte[] serializedMeshData = SerializeMeshData(vertices, triangles, uv);
        // RPCで他のクライアントに送信する
        photonView.RPC("ChangePlayerMesh", RpcTarget.All, serializedMeshData);
    }

    // メッシュデータを受信して再構築
    [PunRPC]
    public void ChangePlayerMesh(byte[] serializedMeshData)
    {
        // 受信したデータをデシリアライズしてメッシュを再構築
         Mesh receivedMesh = DeserializeMeshData(serializedMeshData);

        // 受信したメッシュを反映する処理
        skinnedMesh.sharedMesh = null;
        meshFilter.sharedMesh = receivedMesh;
        meshCollider.sharedMesh = receivedMesh;
    }
    // メッシュデータのリセット
    [PunRPC]
    public void ResetMesh()
    {
        meshFilter.sharedMesh = null;
        meshCollider.sharedMesh = null;
        skinnedMesh.sharedMesh = defaultMesh;
    }
    // メッシュデータをシリアライズするメソッド
    private byte[] SerializeMeshData(Vector3[] vertices, int[] triangles, Vector2[] uv)
    {
        // 実際のデータをバイト配列に変換
        // 頂点データをバイト配列にシリアライズ
        List<byte> byteStream = new List<byte>();

        // 頂点数を先頭に書き込む（整数として）
        byteStream.AddRange(BitConverter.GetBytes(vertices.Length));

        // 頂点座標を順番にバイト配列に追加する
        foreach(Vector3 vertex in vertices)
        {
            byteStream.AddRange(BitConverter.GetBytes(vertex.x));
            byteStream.AddRange(BitConverter.GetBytes(vertex.y));
            byteStream.AddRange(BitConverter.GetBytes(vertex.z));
        }

        // 三角形インデックスの数を次に書き込む
        byteStream.AddRange(BitConverter.GetBytes(triangles.Length));

        // 三角形インデックスを順番にバイト配列に追加する
        foreach(int triangleIndex in triangles)
        {
            byteStream.AddRange(BitConverter.GetBytes(triangleIndex));
        }

        // UV座標の数を次に書き込む
        byteStream.AddRange(BitConverter.GetBytes(uv.Length));

        // UV座標を順番にバイト配列に追加する
        foreach(Vector2 uvCoord in uv)
        {
            byteStream.AddRange(BitConverter.GetBytes(uvCoord.x));
            byteStream.AddRange(BitConverter.GetBytes(uvCoord.y));
        }

        return byteStream.ToArray();

    }

    // メッシュデータをデシリアライズしてMeshオブジェクトを再構築するメソッド
    private Mesh DeserializeMeshData(byte[] serializedMeshData)
    {
        // 実際のデシリアライズのロジックを実装
        Mesh mesh = new Mesh();
        using(MemoryStream stream = new MemoryStream(serializedMeshData))
        {
            using(BinaryReader reader = new BinaryReader(stream))
            {
                // 頂点データの復元
                int vertexCount = reader.ReadInt32();
                Vector3[] vertices = new Vector3[vertexCount];
                for(int i = 0; i < vertexCount; i++)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    float z = reader.ReadSingle();
                    vertices[i] = new Vector3(x, y, z);
                }

                // 三角形インデックスの復元
                int triangleCount = reader.ReadInt32();
                int[] triangles = new int[triangleCount];
                for(int i = 0; i < triangleCount; i++)
                {
                    triangles[i] = reader.ReadInt32();
                }

                // UV座標の復元
                int uvCount = reader.ReadInt32();
                Vector2[] uv = new Vector2[uvCount];
                for(int i = 0; i < uvCount; i++)
                {
                    float u = reader.ReadSingle();
                    float v = reader.ReadSingle();
                    uv[i] = new Vector2(u, v);
                }

                // メッシュにデータを設定する
                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.uv = uv;
            }
        }

        mesh.RecalculateNormals(); // 必要に応じて法線を再計算するなどの後処理を行う
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


