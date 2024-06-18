using Photon.Pun;
using UnityEngine;

public class cube : MonoBehaviourPun
{
    // 同期したいカラー
    private Color syncedColor;

    // カラー変更のRPCメソッド
    [PunRPC]
    void ChangeColorRPC(Color newColor)
    {
        // オブジェクトに適用されているRendererを取得する
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        // Rendererが存在し、マテリアルが存在する場合は、カラーを変更する
        if(renderer != null && renderer.material != null)
        {
            renderer.material.color = newColor;
        }
    }
    // 外部からこのメソッドを呼び出してカラーの変更をRPCで同期する
    public void ChangeColor(Color newColor)
    {
        // RPCを使って全てのクライアントにカラー変更を通知する
        photonView.RPC("ChangeColorRPC", RpcTarget.All, newColor);
    }
}
