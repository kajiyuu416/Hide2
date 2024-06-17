using Photon.Pun;
using UnityEngine;
using System.Collections;
public class cube : Photon.Pun.MonoBehaviourPun
{

    Color co = new Color(150, 150, 150);


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeColor(co);
        }
    }

    // Start is called before the first frame update
    public void SetColor(Color newColor)
    {
        // ここでカラーを設定するロジックを記述する
        GetComponent<Renderer>().material.color = newColor;
    }
    [PunRPC]
    public void RPC_SetColor(Color newColor)
    {
        // RPC メソッドでカラーを設定
        SetColor(newColor);
    }
    public void ChangeColor(Color newColor)
    {
        // ローカルでのカラー変更
        SetColor(newColor);
        // RPC を使って他のプレイヤーにも変更を通知
        photonView.RPC("RPC_SetColor", RpcTarget.AllBuffered, newColor);
        Debug.Log("change");
    }
}
